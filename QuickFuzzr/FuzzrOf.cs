using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

/// <summary>
/// Represents a generator function that produces random values of type TValue given a generation state.
/// Used as the fundamental building block for composing complex data generators through LINQ expressions.
/// </summary>
public delegate IResult<TValue> FuzzrOf<out TValue>(State input);


