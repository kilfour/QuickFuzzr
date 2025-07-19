namespace QuickFuzzr.UnderTheHood
{
	public delegate IResult<TValue> Generator<out TValue>(State input);
}
