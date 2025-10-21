using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class GeneratorToLinq
{
	public static FuzzrOf<TValueTwo> Select<TValueOne, TValueTwo>(
		this FuzzrOf<TValueOne> generator,
		Func<TValueOne, TValueTwo> selector)
	{
		if (generator == null)
			throw new ArgumentNullException("generator");
		if (selector == null)
			throw new ArgumentNullException("selector");

		return s => new Result<TValueTwo>(selector(generator(s).Value), s);
	}

	// This is the Bind function
	public static FuzzrOf<TValueTwo> SelectMany<TValueOne, TValueTwo>(
		this FuzzrOf<TValueOne> generator,
		Func<TValueOne, FuzzrOf<TValueTwo>> selector)
	{
		if (generator == null)
			throw new ArgumentNullException("generator");
		if (selector == null)
			throw new ArgumentNullException("selector");

		return s => selector(generator(s).Value)(s);
	}

	public static FuzzrOf<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
		this FuzzrOf<TValueOne> generator,
		Func<TValueOne, FuzzrOf<TValueTwo>> selector,
		Func<TValueOne, TValueTwo, TValueThree> projector)
	{
		if (generator == null)
			throw new ArgumentNullException("generator");
		if (selector == null)
			throw new ArgumentNullException("selector");
		if (projector == null)
			throw new ArgumentNullException("projector");

		return generator.SelectMany(x => selector(x).Select(y => projector(x, y)));
	}
}