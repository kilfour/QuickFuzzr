using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr.UnderTheHood;

public class Genesis : ICreationEngine
{
    public object Create(State state, Type type)
        => Create(state, type, a => CreateInstance(state, type, a));

    public object Create(State state, Type type, Func<Type?, object> ctor)
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
                if (!state.Collecting.Peek() && Fuzzr.Bool()(state).Value)
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
                if (Fuzzr.Bool()(state).Value)
                    return BuildLeaf(state, leafType);
                else
                    return BuildInstance(ctor(null), state, type);
            }
        }
    }

    private FuzzrOf<object> MakeOneOfThese(Type type)
        => state =>
            new Result<object>(Create(state, type, a => CreateInstance(state, type, a)), state);

    private object BuildLeaf(State state, Type leafType)
        => BuildInstance(CreateInstanceOfExactlyThisType(state, leafType), state, leafType);

    private static object CreateInstance(State state, Type type, Type? typeToExlude)
        => CreateInstanceOfExactlyThisType(state, GetTypeToGenerate(state, type, typeToExlude));

    private static object CreateInstanceOfExactlyThisType(State state, Type typeToGenerate)
    {
        if (state.Constructors.TryGetValue(typeToGenerate, out var constructor))
        {
            var instance = constructor(state);
            ValidateInstanceType(instance, typeToGenerate);
            return instance;
        }

        var defaultCtor = typeToGenerate
            .GetConstructors(MyBinding.Flags)
            .FirstOrDefault(c => c.GetParameters().Length == 0)
                ?? throw new ConstructionException(typeToGenerate.Name);

        try
        {
            return defaultCtor.Invoke([]);
        }
        catch (MemberAccessException exception)
        {
            throw new InstantiationException(typeToGenerate.Name, exception);
        }
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

    private static Type GetTypeToGenerate(State state, Type type, Type? typeToExlude)
    {
        var typeToGenerate = type;
        if (state.InheritanceInfo.TryGetValue(typeToGenerate, out List<Type>? derivedTypes))
        {
            if (typeToExlude != null)
                derivedTypes = derivedTypes.Where(a => a != typeToExlude).ToList();
            var index = state.Random.Next(0, derivedTypes.Count);
            typeToGenerate = derivedTypes[index];
        }
        return typeToGenerate;
    }

    private object BuildInstance(object instance, State state, Type declaringType)
    {
        state.Collecting.Push(false);
        if (!state.StuffToIgnoreAll.Contains(declaringType))
            FillProperties(instance, state);
        state.Collecting.Pop();
        return instance;
    }

    private void FillProperties(object instance, State state)
    {
        _ = state.WithCustomizations
            .Where(a => a.Key.Item1.IsAssignableFrom(instance.GetType()))
            .Select(a => a.Value.Item2(a.Value.Item1(state).Value)(state))
            .ToList();
        foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
        {
            HandleProperty(instance, state, propertyInfo);
        }
    }

    private static bool ShouldGenerateProperty(PropertyInfo prop, State state)
    {
        var setter = prop.SetMethod;
        if (setter != null)
        {
            if (setter.IsPublic)
            {
                if (IsInitOnly(setter))
                    return state.PropertyAccess.HasFlag(PropertyAccess.InitOnly);
                else
                    return state.PropertyAccess.HasFlag(PropertyAccess.PublicSetters);
            }
            if (setter.IsPrivate && state.PropertyAccess.HasFlag(PropertyAccess.PrivateSetters))
                return true;
            if (setter.IsFamily && state.PropertyAccess.HasFlag(PropertyAccess.ProtectedSetters))
                return true;
            if (setter.IsAssembly && state.PropertyAccess.HasFlag(PropertyAccess.InternalSetters))
                return true;
        }
        if (setter == null && prop.GetMethod != null)
        {
            return state.PropertyAccess.HasFlag(PropertyAccess.ReadOnly);
        }
        return false;
    }

    private static bool IsInitOnly(MethodInfo? setter)
        => setter?.ReturnParameter.GetRequiredCustomModifiers()
            .Any(m => m == typeof(IsExternalInit)) == true;

    private void HandleProperty(object instance, State state, PropertyInfo propertyInfo)
    {
        if (NeedsToBeCustomized(state, propertyInfo))
        {
            CustomizeProperty(instance, propertyInfo, state);
            return;
        }

        if (NeedsToBeGenerallyCustomized(state, propertyInfo))
        {
            GenerallyCustomizeProperty(instance, propertyInfo, state);
            return;
        }
        if (!ShouldGenerateProperty(propertyInfo, state))
            return;

        if (NeedsToBeIgnored(state, propertyInfo))
            return;

        if (NeedsToBeGenerallyIgnored(state, propertyInfo))
            return;


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

        // Implement Collections et all here, if ever we decide to
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
            .Any(info => info.Item2.IsAssignableFrom(propertyInfo.ReflectedType)
                && info.Item1.Name == propertyInfo.Name);

    private static void CustomizeProperty(object target, PropertyInfo propertyInfo, State state)
    {
        var key =
            state.Customizations.Keys.First(info =>
                info.Item2.IsAssignableFrom(propertyInfo.ReflectedType)
                && info.Item1.Name == propertyInfo.Name);
        var generator = state.Customizations[key];
        SetPropertyValue(propertyInfo, target, generator(state).Value);
    }

    private static bool IsAKnownPrimitive(State state, PropertyInfo propertyInfo)
        => state.PrimitiveGenerators.ContainsKey(propertyInfo.PropertyType);

    private static void SetPrimitive(object target, PropertyInfo propertyInfo, State state)
    {
        var generator = state.PrimitiveGenerators[propertyInfo.PropertyType];
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

    private void SetObject(object target, PropertyInfo propertyInfo, State state)
    {
        var type = propertyInfo.PropertyType;
        var result = MakeOneOfThese(type)(state);
        SetPropertyValue(propertyInfo, target, result.Value);
    }

    private static void SetEnum(State state, PropertyInfo propertyInfo, object instance)
    {
        var value = Fuzzr.GetEnumValue(propertyInfo.PropertyType, state);
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
            var value = Fuzzr.GetEnumValue(genericArgument, state);
            SetPropertyValue(propertyInfo, instance, Enum.ToObject(genericArgument, value));
        }
    }

    private static void SetPropertyValue(PropertyInfo propertyInfo, object target, object value)
        => propertyInfo.SetValue(target, value, null);
}
