using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<Unit> Replace<T>(this Generator<T> generator)
			where T : struct
		{
			return s =>
			{
				s.PrimitiveGenerators[typeof(T)] = generator.AsObject();
				s.PrimitiveGenerators[typeof(T?)] = generator.Nullable().AsObject();
				return new Result<Unit>(Unit.Instance, s);
			};
		}

		public static Generator<Unit> Replace<T>(this Generator<T?> generator)
			where T : struct
		{
			return s =>
			{
				s.PrimitiveGenerators[typeof(T?)] = generator.AsObject();
				return new Result<Unit>(Unit.Instance, s);
			};
		}

		public static Generator<Unit> Replace(this Generator<string> generator)
		{
			return s =>
			{
				s.PrimitiveGenerators[typeof(string)] = generator.AsObject();
				return new Result<Unit>(Unit.Instance, s);
			};
		}
	}
}