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
					{ typeof(string), Fuzz.String().AsObject() },
			  		{ typeof(int), Fuzz.Int().AsObject() },
					{ typeof(int?), Fuzz.Int().Nullable().AsObject() },
					{ typeof(char), Fuzz.Char().AsObject() },
					{ typeof(char?), Fuzz.Char().Nullable().AsObject() },
					{ typeof(bool), Fuzz.Bool().AsObject() },
					{ typeof(bool?), Fuzz.Bool().Nullable().AsObject() },
					{ typeof(decimal), Fuzz.Decimal().AsObject() },
					{ typeof(decimal?), Fuzz.Decimal().Nullable().AsObject() },
					{ typeof(DateTime), Fuzz.DateTime().AsObject() },
					{ typeof(DateTime?), Fuzz.DateTime().Nullable().AsObject() },
					{ typeof(long), Fuzz.Long().AsObject() },
					{ typeof(long?), Fuzz.Long().Nullable().AsObject() },
					{ typeof(double), Fuzz.Double().AsObject() },
					{ typeof(double?), Fuzz.Double().Nullable().AsObject() },
					{ typeof(float), Fuzz.Float().AsObject() },
					{ typeof(float?), Fuzz.Float().Nullable().AsObject() },
					{ typeof(Guid), Fuzz.Guid().AsObject() },
					{ typeof(Guid?), Fuzz.Guid().Nullable().AsObject() },
					{ typeof(short), Fuzz.Short().AsObject() },
					{ typeof(short?), Fuzz.Short().Nullable().AsObject() },
					{ typeof(TimeSpan), Fuzz.TimeSpan().AsObject() },
					{ typeof(TimeSpan?), Fuzz.TimeSpan().Nullable().AsObject() },
					{ typeof(DateOnly), Fuzz.DateOnly().AsObject() },
					{ typeof(DateOnly?), Fuzz.DateOnly().Nullable().AsObject() }
				};
	}
}