using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class GeneratorToLinq
{
	public static FuzzrOf<TValueTwo> Select<TValueOne, TValueTwo>(
		this FuzzrOf<TValueOne> fuzzr,
		Func<TValueOne, TValueTwo> selector)
	{
		ArgumentNullException.ThrowIfNull(fuzzr);
		ArgumentNullException.ThrowIfNull(selector);
		return s => new Result<TValueTwo>(selector(fuzzr(s).Value), s);
	}

	public static FuzzrOf<TValueTwo> SelectMany<TValueOne, TValueTwo>(
		this FuzzrOf<TValueOne> fuzzr,
		Func<TValueOne, FuzzrOf<TValueTwo>> selector)
	{
		ArgumentNullException.ThrowIfNull(fuzzr);
		ArgumentNullException.ThrowIfNull(selector);
		return s => selector(fuzzr(s).Value)(s);
	}

	public static FuzzrOf<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
		this FuzzrOf<TValueOne> fuzzr,
		Func<TValueOne, FuzzrOf<TValueTwo>> selector,
		Func<TValueOne, TValueTwo, TValueThree> projector)
	{
		ArgumentNullException.ThrowIfNull(fuzzr);
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentNullException.ThrowIfNull(projector);
		return fuzzr.SelectMany(x => selector(x).Select(y => projector(x, y)));
	}
}