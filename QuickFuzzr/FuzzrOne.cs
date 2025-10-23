using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<T> One<T>()
		=> state =>
			new Result<T>((T)DepthControlledCreation(state, typeof(T), a => (T)CreateInstance(state, typeof(T), a)), state);

	public static FuzzrOf<T> One<T>(Func<T> constructor)
		=> state =>
			new Result<T>((T)DepthControlledCreation(state, typeof(T), _ => constructor()!), state);

	private static FuzzrOf<object> One(Type type)
		=> state =>
			new Result<object>(DepthControlledCreation(state, type, a => CreateInstance(state, type, a)), state);

	private static object DepthControlledCreation(State state, Type type, Func<Type?, object> ctor)
	{
		using (state.WithDepthFrame(type))
		{
			state.TreeLeaves.TryGetValue(type, out var leafType);
			if (leafType == null)
			{
				var currentDepth = state.GetDepth(type);
				var (min, max) = state.GetDepthConstraint(type);
				if (currentDepth <= min)
					return BuildInstance(ctor(null), state, type);
				if (currentDepth > max)
					return null!;
				if (Bool()(state).Value)
					return null!;
				else
					return BuildInstance(ctor(null), state, type);
			}
			else
			{
				var currentDepth = state.GetDepth(type);
				var (min, max) = state.GetDepthConstraint(type);
				if (currentDepth == max)
					return BuildLeaf(state, leafType);
				if (currentDepth < min)
					return BuildInstance(ctor(leafType), state, type);
				if (Bool()(state).Value)
					return BuildLeaf(state, leafType);
				else
					return BuildInstance(ctor(null), state, type);
			}
		}
	}

	private static object BuildLeaf(State state, Type leafType)
		=> BuildInstance(CreateInstanceOfExactlyThisType(state, leafType), state, leafType);

	private static object CreateInstance(State state, Type type, Type? typeToExlude)
		=> CreateInstanceOfExactlyThisType(state, GetTypeToGenerate(state, type, typeToExlude));


	private static object CreateInstanceOfExactlyThisType(State state, Type typeToGenerate)
	{
		// If we have a registered constructor generator, use it
		if (state.Constructors.TryGetValue(typeToGenerate, out var constructor))
		{
			var instance = constructor(state);
			ValidateInstanceType(instance, typeToGenerate);
			return instance;
		}

		// Fallback to default constructor
		var defaultCtor = typeToGenerate
			.GetConstructors(MyBinding.Flags)
			.FirstOrDefault(c => c.GetParameters().Length == 0);

		if (defaultCtor == null)
			throw new InvalidOperationException($"No constructor or Construct(...) rule found for type {typeToGenerate}");

		var defaultInstance = defaultCtor.Invoke([]);
		//ValidateInstanceType(defaultInstance, typeToGenerate);
		return defaultInstance;
	}

	private static void ValidateInstanceType(object instance, Type declaredType)
	{
		var actualType = instance?.GetType();
		if (actualType is not null &&
			actualType != declaredType &&
			!declaredType.IsAssignableFrom(actualType))
		{
			throw new InvalidOperationException(
				$"Fuzz created an instance of type '{actualType}' " +
				$"but expected a type assignable to '{declaredType}'. " +
				$"This might be due to an internal framework subclass " +
				$"like 'JsonValueCustomized<T>'. Consider using IgnoreAll() " +
				$"on '{actualType.BaseType}' or normalizing the type.");
		}
	}

	private static Type GetTypeToGenerate(State s, Type type, Type? typeToExlude)
	{
		var typeToGenerate = type;
		if (s.InheritanceInfo.ContainsKey(typeToGenerate))
		{

			var derivedTypes = s.InheritanceInfo[typeToGenerate];
			if (typeToExlude != null)
				derivedTypes = derivedTypes.Where(a => a != typeToExlude).ToList();
			var index = s.Random.Next(0, derivedTypes.Count);
			typeToGenerate = derivedTypes[index];
		}
		return typeToGenerate;
	}

	private static object BuildInstance(object instance, State state, Type declaringType)
	{
		if (!state.StuffToIgnoreAll.Contains(declaringType))
			FillProperties(instance, state);
		return instance;
	}

	private static void FillProperties(object instance, State state)
	{
		foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
		{
			HandleProperty(instance, state, propertyInfo);
		}
	}

	private static void HandleProperty(object instance, State state, PropertyInfo propertyInfo)
	{
		if (NeedsToBeIgnored(state, propertyInfo))
			return;

		if (NeedsToBeGenerallyIgnored(state, propertyInfo))
			return;

		if (NeedsToBeGenerallyCustomized(state, propertyInfo))
		{
			GenerallyCustomizeProperty(instance, propertyInfo, state);
			return;
		}

		if (NeedsToBeCustomized(state, propertyInfo))
		{
			CustomizeProperty(instance, propertyInfo, state);
			return;
		}

		if (IsAKnownPrimitive(state, propertyInfo))
		{
			SetPrimitive(instance, propertyInfo, state);
			return;
		}

		if (propertyInfo.PropertyType.IsEnum)
		{
			SetEnum(state, propertyInfo, instance);
			return;
		}

		if (IsANullableEnum(propertyInfo))
		{
			SetNullableEnum(state, propertyInfo, instance);
			return;
		}

		// Implement Lists et all here
		if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
			return;

		if (IsObject(propertyInfo))
		{
			SetObject(instance, propertyInfo, state);
			return;
		}
	}

	private static bool NeedsToBeIgnored(State state, PropertyInfo propertyInfo)
		=> state.StuffToIgnore
			.Any(info => info.ReflectedType!.IsAssignableFrom(propertyInfo.ReflectedType)
				&& info.Name == propertyInfo.Name);

	private static bool NeedsToBeGenerallyIgnored(State state, PropertyInfo propertyInfo)
		=> state.GeneralStuffToIgnore.Any(info => info(propertyInfo));

	private static bool NeedsToBeGenerallyCustomized(State state, PropertyInfo propertyInfo)
		=> state.GeneralCustomizations.Keys.Any(info => info(propertyInfo));

	private static void GenerallyCustomizeProperty(object target, PropertyInfo propertyInfo, State state)
	{
		var key = state.GeneralCustomizations.Keys.First(info => info(propertyInfo));
		var generator = state.GeneralCustomizations[key](propertyInfo);
		SetPropertyValue(propertyInfo, target, generator(state).Value);
	}

	private static bool NeedsToBeCustomized(State state, PropertyInfo propertyInfo)
		=> state.Customizations.Keys
			.Any(info => info.ReflectedType!.IsAssignableFrom(propertyInfo.ReflectedType)
				&& info.Name == propertyInfo.Name);

	private static void CustomizeProperty(object target, PropertyInfo propertyInfo, State state)
	{
		var key =
			state.Customizations.Keys.First(info =>
				info.ReflectedType!.IsAssignableFrom(propertyInfo.ReflectedType)
				&& info.Name == propertyInfo.Name);
		var generator = state.Customizations[key];
		SetPropertyValue(propertyInfo, target, generator(state).Value);
	}

	private static bool IsAKnownPrimitive(State state, PropertyInfo propertyInfo)
	{
		return state.PrimitiveGenerators.ContainsKey(propertyInfo.PropertyType);
	}

	private static void SetPrimitive(object target, PropertyInfo propertyInfo, State state)
	{
		var generator = state.PrimitiveGenerators[propertyInfo.PropertyType];
		// if (t.IsValueType) unify ???
		if (propertyInfo.PropertyType == typeof(string))
		{
			var ctx = new NullabilityInfoContext();
			var nullabilityInfo = ctx.Create(propertyInfo);
			bool allowsNull = nullabilityInfo.ReadState == NullabilityState.Nullable;
			if (allowsNull)
				generator = generator.NullableRef()!;
		}
		SetPropertyValue(propertyInfo, target, generator(state).Value);
	}

	private static bool IsObject(PropertyInfo propertyInfo)
	{
		if (propertyInfo.Name == "EqualityContract" &&
			propertyInfo.PropertyType == typeof(Type) &&
			propertyInfo.DeclaringType is { IsClass: true } &&
			typeof(IEquatable<>).MakeGenericType(propertyInfo.DeclaringType).IsAssignableFrom(propertyInfo.DeclaringType))
			return false;
		return propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string);
	}

	private static void SetObject(object target, PropertyInfo propertyInfo, State state)
	{
		var type = propertyInfo.PropertyType;
		var result = One(type)(state);
		SetPropertyValue(propertyInfo, target, result.Value);
	}

	public static bool IsGenericTypeOf(this Type type, Type openGeneric)
	{
		return type.IsGenericType && type.GetGenericTypeDefinition() == openGeneric;
	}

	private static void SetEnum(State state, PropertyInfo propertyInfo, object instance)
	{
		var value = GetEnumValue(propertyInfo.PropertyType, state);
		SetPropertyValue(propertyInfo, instance, value);
	}

	private static bool IsANullableEnum(PropertyInfo propertyInfo)
	{
		if (!propertyInfo.PropertyType.IsGenericType)
			return false;
		var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
		if (genericType != typeof(Nullable<>))
			return false;
		var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];
		return genericArgument.IsEnum;
	}

	private static void SetNullableEnum(State state, PropertyInfo propertyInfo, object instance)
	{
		if (state.Random.Next(0, 5) == 0)
		{
			SetPropertyValue(propertyInfo, instance, null!);
		}
		else
		{
			var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];
			var value = GetEnumValue(genericArgument, state);
			SetPropertyValue(propertyInfo, instance, System.Enum.ToObject(genericArgument, value));
		}
	}

	private static void SetPropertyValue(PropertyInfo propertyInfo, object target, object value)
	{
		var prop = propertyInfo;
		if (!prop.CanWrite)
		{
			var field = GetBackingField(propertyInfo);
			if (field is not null)
			{
				field.SetValue(target, value);
				return;
			}
			prop = propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name);
		}


		if (prop != null && prop.CanWrite) // todo check this
			prop.SetValue(target, value, null);
	}

	public static FieldInfo? GetBackingField(PropertyInfo property)
	{
		if (property == null)
			throw new ArgumentNullException(nameof(property));

		if (!property.CanRead
			|| property.GetMethod is null
			|| !property.GetMethod.IsDefined(typeof(CompilerGeneratedAttribute)))
			return null;

		var backingFieldName = $"<{property.Name}>k__BackingField";
		var backingField = property.DeclaringType?.GetField(
			backingFieldName,
			BindingFlags.Instance | BindingFlags.NonPublic);

		return backingField;
	}
}