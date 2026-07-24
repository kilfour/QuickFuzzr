using System.Reflection;
using System.Security.Cryptography;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr.UnderTheHood;

public class State
{
    public ICreationEngine CreationEngine { get; set; } = new Genesis();
    public int Seed { get; }
    public MoreRandom Random { get; }

    public State(int? seed = null)
    {
        Seed = seed ?? RandomNumberGenerator.GetInt32(0, int.MaxValue);
        Random = new MoreRandom(Seed);
    }

    public State(int seed)
    {
        Seed = seed;
        Random = new MoreRandom(seed);
    }

    public PropertyAccess PropertyAccess { get; set; } = PropertyAccess.PublicSetters;
    public bool FieldAccessEnabled { get; set; }

    // ---------------------------------------------------------------------
    // Retrying Uniques et al
    public int RetryLimit { get; private set; } = 64;
    public void SetRetryLimit(int limit)
    {
        if (limit < 1 || limit > 1024)
            throw new RetryLimitOutOfRangeException(limit);
        RetryLimit = limit;
    }

    // ---------------------------------------------------------------------
    // Inheriting
    public readonly Dictionary<Type, List<Type>> InheritanceInfo = [];

    public Dictionary<Type, Type> Endings = [];

    // ---------------------------------------------------------------------
    // Depth Control
    public Stack<bool> Collecting { get; set; } = new Stack<bool>([false]);

    public readonly Dictionary<Type, DepthConstraint> DepthConstraints = [];

    public Stack<Type> BuildingEndings { get; set; } = [];

    public bool IsBuildingEnding(Type type)
        => BuildingEndings.Contains(type);

    public void StartBuildingEnding(Type type)
        => BuildingEndings.Push(type);

    public void StopBuildingEnding()
        => BuildingEndings.Pop();

    public DisposableAction BuildEnding(Type type)
    {
        StartBuildingEnding(type);
        return new DisposableAction(StopBuildingEnding);
    }

    private readonly Stack<DepthFrame> depthFrames = new();

    private static readonly DepthConstraint DefaultDepth = new(1, 1);

    public DepthConstraint GetDepthConstraint(Type type) =>
        DepthConstraints.TryGetValue(type, out var constraint) ? constraint : DefaultDepth;

    public int GetDepth(Type type) =>
        depthFrames.FirstOrDefault(f => f.Type == type)?.Depth ?? 0;

    private void PushDepthFrame(Type type)
        => depthFrames.Push(new(type, GetDepth(type) + 1));

    private void PopDepthFrame() => depthFrames.Pop();

    public DisposableAction WithDepthFrame(Type type)
    {
        PushDepthFrame(type);
        return new DisposableAction(PopDepthFrame);
    }

    // ---------------------------------------------------------------------
    // Ignoring Stuff
    public readonly Dictionary<Type, Action<object>> StuffToApply = [];

    // ---------------------------------------------------------------------
    // Ignoring Stuff
    public readonly HashSet<Func<PropertyInfo, bool>> GeneralStuffToIgnore = [];
    public readonly HashSet<Type> StuffToIgnoreAll = [];
    public readonly HashSet<PropertyInfo> StuffToIgnore = [];

    // ---------------------------------------------------------------------
    // Memory for counters and uniques
    private readonly Dictionary<object, object> fuzzrMemory = [];

    public T Get<T>(object key, T newValue)
    {
        if (!fuzzrMemory.ContainsKey(key))
            fuzzrMemory[key] = newValue!;
        return (T)fuzzrMemory[key];
    }

    public T Set<T>(object key, T value)
    {
        fuzzrMemory[key] = value!;
        return value;
    }

    // ---------------------------------------------------------------------
    // Property Customizations
    public readonly Dictionary<Func<PropertyInfo, bool>, Func<PropertyInfo, FuzzrOf<object>>> GeneralCustomizations = [];
    public readonly List<Func<PropertyInfo, bool>> GeneralCustomizationOrder = [];
    public readonly Dictionary<(Type TargetType, string PropertyName), FuzzrOf<object>> Customizations = [];
    public readonly Dictionary<(Type, Type), (FuzzrOf<object>, Func<object, FuzzrOf<Intent>>)> WithCustomizations = [];

    // ---------------------------------------------------------------------
    // Constructors
    public readonly Dictionary<Type, Func<State, object>> Constructors = [];

    // ---------------------------------------------------------------------
    // Primitive Fuzzr Registry
    public readonly Dictionary<Type, FuzzrOf<object>> PrimitiveFuzzrs
        = new()
            {
                { typeof(string), Fuzzr.String(Fuzzr.Char(), 1, 10).AsObject() },
                { typeof(int), Fuzzr.Int(1, 100).AsObject() },
                { typeof(int?), Fuzzr.Int(1, 100).Nullable().AsObject() },
                { typeof(char), Fuzzr.Char('a', 'z').AsObject() },
                { typeof(char?), Fuzzr.Char('a', 'z').Nullable().AsObject() },
                { typeof(bool), BuiltInBool().AsObject() },
                { typeof(bool?), BuiltInBool().Nullable().AsObject() },
                { typeof(byte), Fuzzr.Byte(byte.MinValue, byte.MaxValue).AsObject() },
                { typeof(byte?), Fuzzr.Byte(byte.MinValue, byte.MaxValue).Nullable().AsObject() },
                { typeof(decimal), Fuzzr.Decimal(1, 100, 2).AsObject() },
                { typeof(decimal?), Fuzzr.Decimal(1, 100, 2).Nullable().AsObject() },
                { typeof(DateTime), Fuzzr.DateTime(new DateTime(1970, 1, 1), new DateTime(2020, 12, 31)).AsObject() },
                { typeof(DateTime?), Fuzzr.DateTime(new DateTime(1970, 1, 1), new DateTime(2020, 12, 31)).Nullable().AsObject() },
                { typeof(long), Fuzzr.Long(1, 100).AsObject() },
                { typeof(long?), Fuzzr.Long(1, 100).Nullable().AsObject() },
                { typeof(double), Fuzzr.Double(1, 100).AsObject() },
                { typeof(double?), Fuzzr.Double(1, 100).Nullable().AsObject() },
                { typeof(float), Fuzzr.Float(1, 100).AsObject() },
                { typeof(float?), Fuzzr.Float(1, 100).Nullable().AsObject() },
                { typeof(Guid), BuiltInGuid().AsObject() },
                { typeof(Guid?), BuiltInGuid().Nullable().AsObject() },
                { typeof(Half), Fuzzr.Half((Half)1, (Half)100).AsObject() },
                { typeof(Half?), Fuzzr.Half((Half)1, (Half)100).Nullable().AsObject() },
                { typeof(short), Fuzzr.Short(1, 100).AsObject() },
                { typeof(short?), Fuzzr.Short(1, 100).Nullable().AsObject() },
                { typeof(TimeSpan), Fuzzr.TimeSpan(1, 1000).AsObject() },
                { typeof(TimeSpan?), Fuzzr.TimeSpan(1, 1000).Nullable().AsObject() },
                { typeof(DateOnly), Fuzzr.DateOnly(new DateOnly(1970, 1, 1), new DateOnly(2020, 12, 31)).AsObject() },
                { typeof(DateOnly?), Fuzzr.DateOnly(new DateOnly(1970, 1, 1), new DateOnly(2020, 12, 31)).Nullable().AsObject() },
                { typeof(TimeOnly), Fuzzr.TimeOnly(System.TimeOnly.MinValue, System.TimeOnly.MaxValue).AsObject() },
                { typeof(TimeOnly?), Fuzzr.TimeOnly(System.TimeOnly.MinValue, System.TimeOnly.MaxValue).Nullable().AsObject() },
                { typeof(ushort), Fuzzr.UShort(1, 100).AsObject() },
                { typeof(ushort?), Fuzzr.UShort(1, 100).Nullable().AsObject() },
                { typeof(ulong), Fuzzr.ULong(1, 100).AsObject() },
                { typeof(ulong?), Fuzzr.ULong(1, 100).Nullable().AsObject() },
                { typeof(uint), Fuzzr.UInt(1, 100).AsObject() },
                { typeof(uint?), Fuzzr.UInt(1, 100).Nullable().AsObject() }
            };

    private static FuzzrOf<bool> BuiltInBool() =>
        state => new Result<bool>(state.Random.Next(0, 2) > 0, state);

    private static FuzzrOf<Guid> BuiltInGuid() =>
        state =>
        {
            var bytes = new byte[16];
            state.Random.NextBytes(bytes);
            return new Result<Guid>(new Guid(bytes), state);
        };
}
