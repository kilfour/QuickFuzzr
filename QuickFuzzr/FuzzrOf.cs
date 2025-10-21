using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public delegate IResult<TValue> FuzzrOf<out TValue>(State input);


