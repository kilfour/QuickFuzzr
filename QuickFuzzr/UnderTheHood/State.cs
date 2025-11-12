using System.Reflection;
using System.Security.Cryptography;
using QuickFuzzr.Instruments;
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

	public DepthConstraint GetDepthConstraint(Type type) =>
		DepthConstraints.TryGetValue(type, out var c) ? c : new(1, 1);

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

	public readonly HashSet<Func<PropertyInfo, bool>> GeneralStuffToIgnore = [];
	public readonly HashSet<Type> StuffToIgnoreAll = [];
	public readonly HashSet<PropertyInfo> StuffToIgnore = [];
	private readonly Dictionary<object, object> fuzzrMemory = [];

	public T Get<T>(object key, T newValue)
	{
		if (!fuzzrMemory.ContainsKey(key))
			fuzzrMemory[key] = newValue!;
		return (T)fuzzrMemory[key];
	}

	public T Set<T>(object key, T value)
		=> Chain.It(() => fuzzrMemory[key] = value!, value);

	public readonly Dictionary<Type, List<Type>> InheritanceInfo = [];

	public Dictionary<Type, Type> Endings = [];

	public readonly Dictionary<Func<PropertyInfo, bool>, Func<PropertyInfo, FuzzrOf<object>>> GeneralCustomizations = [];
	public readonly Dictionary<(PropertyInfo, Type), FuzzrOf<object>> Customizations = [];
	public readonly Dictionary<(Type, Type), (FuzzrOf<object>, Func<object, FuzzrOf<Intent>>)> WithCustomizations = [];
	public readonly Dictionary<Type, Func<State, object>> Constructors = [];

	public readonly Dictionary<Type, FuzzrOf<object>> PrimitiveFuzzrs
		= new()
			{
				{ typeof(string), Fuzzr.String().AsObject() },
				{ typeof(int), Fuzzr.Int().AsObject() },
				{ typeof(int?), Fuzzr.Int().Nullable().AsObject() },
				{ typeof(char), Fuzzr.Char().AsObject() },
				{ typeof(char?), Fuzzr.Char().Nullable().AsObject() },
				{ typeof(bool), Fuzzr.Bool().AsObject() },
				{ typeof(bool?), Fuzzr.Bool().Nullable().AsObject() },
				{ typeof(byte), Fuzzr.Byte().AsObject() },
				{ typeof(byte?), Fuzzr.Byte().Nullable().AsObject() },
				{ typeof(decimal), Fuzzr.Decimal().AsObject() },
				{ typeof(decimal?), Fuzzr.Decimal().Nullable().AsObject() },
				{ typeof(DateTime), Fuzzr.DateTime().AsObject() },
				{ typeof(DateTime?), Fuzzr.DateTime().Nullable().AsObject() },
				{ typeof(long), Fuzzr.Long().AsObject() },
				{ typeof(long?), Fuzzr.Long().Nullable().AsObject() },
				{ typeof(double), Fuzzr.Double().AsObject() },
				{ typeof(double?), Fuzzr.Double().Nullable().AsObject() },
				{ typeof(float), Fuzzr.Float().AsObject() },
				{ typeof(float?), Fuzzr.Float().Nullable().AsObject() },
				{ typeof(Guid), Fuzzr.Guid().AsObject() },
				{ typeof(Guid?), Fuzzr.Guid().Nullable().AsObject() },
				{ typeof(Half), Fuzzr.Half().AsObject() },
				{ typeof(Half?), Fuzzr.Half().Nullable().AsObject() },
				{ typeof(short), Fuzzr.Short().AsObject() },
				{ typeof(short?), Fuzzr.Short().Nullable().AsObject() },
				{ typeof(TimeSpan), Fuzzr.TimeSpan().AsObject() },
				{ typeof(TimeSpan?), Fuzzr.TimeSpan().Nullable().AsObject() },
				{ typeof(DateOnly), Fuzzr.DateOnly().AsObject() },
				{ typeof(DateOnly?), Fuzzr.DateOnly().Nullable().AsObject() },
				{ typeof(TimeOnly), Fuzzr.TimeOnly().AsObject() },
				{ typeof(TimeOnly?), Fuzzr.TimeOnly().Nullable().AsObject() },
				{ typeof(ushort), Fuzzr.UShort().AsObject() },
				{ typeof(ushort?), Fuzzr.UShort().Nullable().AsObject() },
				{ typeof(ulong), Fuzzr.ULong().AsObject() },
				{ typeof(ulong?), Fuzzr.ULong().Nullable().AsObject() },
				{ typeof(uint), Fuzzr.UInt().AsObject() },
				{ typeof(uint?), Fuzzr.UInt().Nullable().AsObject() }
			};
}