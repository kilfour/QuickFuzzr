using System.Reflection;
using System.Security.Cryptography;

namespace QuickFuzzr.UnderTheHood
{
	public record DepthConstraint(int Min, int Max);

	public record DepthFrame(Type Type, int Depth);

	public class State
	{
		public int Seed { get; }
		public MoreRandom Random { get; }

		public State()
		{
			Seed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
			Random = new MoreRandom(Seed);
		}

		public State(int seed)
		{
			Seed = seed;
			Random = new MoreRandom(seed);
		}

		// ---------------------------------------------------------------------
		// Depth Control
		public readonly Dictionary<Type, DepthConstraint> DepthConstraints = [];

		private readonly Stack<DepthFrame> depthFrames = new();

		public DepthConstraint GetDepthConstraint(Type type) =>
			DepthConstraints.TryGetValue(type, out var c) ? c : new(1, 1);

		public int GetDepth(Type type) =>
			depthFrames.FirstOrDefault(f => f.Type == type)?.Depth ?? 0;

		public void PushDepthFrame(Type type)
			=> depthFrames.Push(new(type, GetDepth(type) + 1));

		public void PopDepthFrame() => depthFrames.Pop();

		public DisposableAction WithDepthFrame(Type type)
		{
			PushDepthFrame(type);
			return new DisposableAction(PopDepthFrame);
		}
		// ---------------------------------------------------------------------

		public readonly List<Type> StuffToIgnoreAll = new List<Type>();
		public readonly List<PropertyInfo> StuffToIgnore = [];
		private readonly Dictionary<object, object> generatorMemory = [];

		public T Get<T>(object key, T newValue)
		{
			if (!generatorMemory.ContainsKey(key))
				generatorMemory[key] = newValue!;
			return (T)generatorMemory[key];
		}

		public void Set<T>(object key, T value)
		{
			generatorMemory[key] = value!;
		}

		public readonly Dictionary<Type, List<Type>> InheritanceInfo = [];
		public Dictionary<Type, Type> TreeLeaves = [];

		public readonly Dictionary<Func<PropertyInfo, bool>, Generator<object>> GeneralCustomizations = [];
		public readonly Dictionary<PropertyInfo, Generator<object>> Customizations = [];
		public readonly Dictionary<Type, List<Func<State, object>>> Constructors = [];
		public readonly Dictionary<Type, List<Action<State, object>>> ActionsToApply = [];

		public void AddActionToApplyFor(Type type, Action<State, object> action)
		{
			if (!ActionsToApply.ContainsKey(type))
				ActionsToApply[type] = new List<Action<State, object>>();
			var actions = ActionsToApply[type];
			if (actions.All(a => a.GetHashCode() != action.GetHashCode()))
				actions.Add(action);
		}

		public readonly Dictionary<Type, Generator<object>> PrimitiveGenerators
			= new()
				{
					{ typeof(string), Fuzzr.String().AsObject() },
			  		{ typeof(int), Fuzzr.Int().AsObject() },
					{ typeof(int?), Fuzzr.Int().Nullable().AsObject() },
					{ typeof(char), Fuzzr.Char().AsObject() },
					{ typeof(char?), Fuzzr.Char().Nullable().AsObject() },
					{ typeof(bool), Fuzzr.Bool().AsObject() },
					{ typeof(bool?), Fuzzr.Bool().Nullable().AsObject() },
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
					{ typeof(short), Fuzzr.Short().AsObject() },
					{ typeof(short?), Fuzzr.Short().Nullable().AsObject() },
					{ typeof(TimeSpan), Fuzzr.TimeSpan().AsObject() },
					{ typeof(TimeSpan?), Fuzzr.TimeSpan().Nullable().AsObject() },
					{ typeof(DateOnly), Fuzzr.DateOnly().AsObject() },
					{ typeof(DateOnly?), Fuzzr.DateOnly().Nullable().AsObject() },
					{ typeof(TimeOnly), Fuzzr.TimeOnly().AsObject() },
					{ typeof(TimeOnly?), Fuzzr.TimeOnly().Nullable().AsObject() }
				};
	}
}