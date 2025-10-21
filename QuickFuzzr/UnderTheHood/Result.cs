﻿namespace QuickFuzzr.UnderTheHood;

public interface IResult<out TValue>
{
	TValue Value { get; }
}

public class Result<TValue> : IResult<TValue>
{
	public TValue Value { get; private set; }
	public readonly State State;
	public Result(TValue value, State state) { Value = value; State = state; }
}

public static class Result
{
	public static Result<Intent> Unit(State state) => new(Intent.Fixed, state);
}