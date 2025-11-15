using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

/// <summary>
/// Represents a Fuzzr function that produces random values of type TValue given a generation state.
/// Used as the fundamental building block for composing complex data Fuzzrs through LINQ expressions.
/// </summary>
public delegate IResult<TValue> FuzzrOf<out TValue>(State input);


