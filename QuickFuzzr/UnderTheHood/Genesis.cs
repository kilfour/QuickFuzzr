using System.Collections;
using System.Collections.Concurrent;
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
            state.Endings.TryGetValue(type, out var endingType);
            if (endingType == null)
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
                if (state.IsBuildingEnding(type))
                    return null!;
                var currentDepth = state.GetDepth(type);
                var (min, max) = state.GetDepthConstraint(type);
                if (currentDepth == max)
                    return BuildEnding(state, type, endingType);
                if (currentDepth < min)
                    return BuildInstance(ctor(endingType), state, type);
                if (Fuzzr.Bool()(state).Value)
                    return BuildEnding(state, type, endingType);
                else
                    return BuildInstance(ctor(null), state, type);
            }
        }
    }

    private FuzzrOf<object> MakeOneOfThese(Type type)
        => state =>
            new Result<object>(Create(state, type, a => CreateInstance(state, type, a)), state);

    private object BuildEnding(State state, Type type, Type endingType)
    {
        using (state.BuildEnding(type))
            return BuildInstance(CreateInstanceOfExactlyThisType(state, endingType), state, endingType);
    }

    private static object CreateInstance(State state, Type type, Type? typeToExlude)
        => CreateInstanceOfExactlyThisType(state, GetTypeToGenerate(state, type, typeToExlude));

    private static object CreateInstanceOfExactlyThisType(State state, Type typeToGenerate)
    {
        if (state.Constructors.TryGetValue(typeToGenerate, out var constructor))
        {
            return constructor(state);
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

    // private static Type GetTypeToGenerate(State state, Type type, Type? typeToExlude)
    // {
    //     var typeToGenerate = type;
    //     if (state.InheritanceInfo.TryGetValue(typeToGenerate, out List<Type>? derivedTypes))
    //     {
    //         if (typeToExlude != null)
    //             derivedTypes = derivedTypes.Where(a => a != typeToExlude).ToList();
    //         var index = state.Random.Next(0, derivedTypes.Count);
    //         typeToGenerate = derivedTypes[index];
    //     }
    //     return typeToGenerate;
    // }

    private static Type GetTypeToGenerate(State state, Type type, Type? typeToExclude)
    {
        if (!state.InheritanceInfo.TryGetValue(type, out var derivedTypes))
            return type;

        if (typeToExclude is null)
            return derivedTypes[state.Random.Next(0, derivedTypes.Count)];

        // fast path: if exclude is not in list → skip filtering
        int idx = derivedTypes.IndexOf(typeToExclude);
        if (idx < 0)
            return derivedTypes[state.Random.Next(0, derivedTypes.Count)];

        int count = derivedTypes.Count - 1;
        if (count == 0)
            return type; // only excluded type available → fallback to base type

        int r = state.Random.Next(0, count);

        // If r >= idx, skip over the excluded slot
        return derivedTypes[r >= idx ? r + 1 : r];
    }

    private object BuildInstance(object instance, State state, Type declaringType)
    {
        if (instance == null)
        {
            throw new FactoryConstructionException(declaringType.Name);
        }
        state.Collecting.Push(false);
        if (!state.StuffToIgnoreAll.Contains(declaringType))
            FillProperties(instance, state);

        var apply = state.StuffToApply.FirstOrDefault(a => a.Key.IsAssignableFrom(declaringType));
        if (apply.Key != null) apply.Value(instance);

        state.Collecting.Pop();
        return instance;
    }

    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    private static PropertyInfo[] GetCachedProperties(Type type) =>
        PropertyCache.GetOrAdd(type, t => t.GetProperties(MyBinding.Flags));

    private void FillProperties(object instance, State state)
    {
        var instanceType = instance.GetType();
        foreach (var customization in state.WithCustomizations)
        {
            if (!customization.Key.Item1.IsAssignableFrom(instanceType))
                continue;
            var (fuzzr, applier) = customization.Value;
            applier(fuzzr(state).Value)(state);
        }

        foreach (var propertyInfo in GetCachedProperties(instanceType))
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

    private static readonly ConcurrentDictionary<MethodInfo, bool> IsInitOnlyCache = new();

    private static bool IsInitOnly(MethodInfo? setter)
    {
        if (setter == null) return false;
        return IsInitOnlyCache.GetOrAdd(setter, s => s.ReturnParameter.GetRequiredCustomModifiers()
            .Any(m => m == typeof(IsExternalInit)) == true);
    }

    // private static bool IsInitOnly(MethodInfo? setter)
    //     => setter?.ReturnParameter.GetRequiredCustomModifiers()
    //         .Any(m => m == typeof(IsExternalInit)) == true;

    private void HandleProperty(object instance, State state, PropertyInfo propertyInfo)
    {
        if (CustomizeProperty(instance, propertyInfo, state)) return;
        if (GenerallyCustomizeProperty(instance, propertyInfo, state)) return;

        if (!ShouldGenerateProperty(propertyInfo, state)) return;

        if (NeedsToBeGenerallyIgnored(state, propertyInfo)) return;
        if (NeedsToBeIgnored(state, propertyInfo)) return;

        if (SetPrimitive(instance, propertyInfo, state)) return;


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

    private static bool GenerallyCustomizeProperty(object target, PropertyInfo propertyInfo, State state)
    {
        Func<PropertyInfo, FuzzrOf<object>>? factory = null;
        for (var i = state.GeneralCustomizationOrder.Count - 1; i >= 0; i--)
        {
            var predicate = state.GeneralCustomizationOrder[i];
            if (predicate(propertyInfo))
            {
                factory = state.GeneralCustomizations[predicate];
                break;
            }
        }
        if (factory is null)
            return false;

        var fuzzr = factory(propertyInfo);
        var value = fuzzr(state).Value;
        SetPropertyValue(propertyInfo, target, value);
        return true;
    }

    private static bool CustomizeProperty(object target, PropertyInfo propertyInfo, State state)
    {
        var fuzzr = FindCustomizationFor(state, propertyInfo);
        if (fuzzr is not null)
        {
            SetPropertyValue(propertyInfo, target, fuzzr(state).Value);
            return true;
        }
        return false;
    }

    private static FuzzrOf<object>? FindCustomizationFor(
        State state,
        PropertyInfo propertyInfo)
    {
        var name = propertyInfo.Name;
        var type = propertyInfo.ReflectedType ?? propertyInfo.DeclaringType;
        while (type is not null)
        {
            if (state.Customizations.TryGetValue((type, name), out var fuzzr))
                return fuzzr;
            type = type.BaseType;
        }
        return null;
    }

    private static bool SetPrimitive(object target, PropertyInfo propertyInfo, State state)
    {
        if (!state.PrimitiveFuzzrs.TryGetValue(propertyInfo.PropertyType, out FuzzrOf<object>? fuzzr))
            return false;
        if (propertyInfo.PropertyType == typeof(string) && StringAllowsNull(propertyInfo))
        {
            fuzzr = fuzzr.NullableRef()!;
        }
        SetPropertyValue(propertyInfo, target, fuzzr(state).Value);
        return true;
    }

    private static readonly NullabilityInfoContext Nullability = new();
    private static readonly Dictionary<PropertyInfo, bool> StringNullability = new();

    private static bool StringAllowsNull(PropertyInfo propertyInfo)
    {
        if (!StringNullability.TryGetValue(propertyInfo, out var allowsNull))
        {
            lock (StringNullability)
            {
                if (!StringNullability.TryGetValue(propertyInfo, out allowsNull))
                {
                    var info = Nullability.Create(propertyInfo);
                    allowsNull = info.ReadState == NullabilityState.Nullable;
                    StringNullability[propertyInfo] = allowsNull;
                }
            }
        }
        return allowsNull;
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

        if (prop != null && prop.CanWrite)
            prop.SetValue(target, value, null);
    }

    private static FieldInfo? GetBackingField(PropertyInfo property)
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
