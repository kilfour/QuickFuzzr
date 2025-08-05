﻿using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocFileHeader("How About Null(s) ?")]
[DocContent("Various extension methods allow for influencing null generation.")]
public class HowAboutNull
{
	[Fact]
	[DocContent(
@"- `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.  
> Used for value types.")]
	public void Nullable()
	{
		var generator = Fuzz.Int().Nullable();
		var seenNull = false;
		var seenValue = false;
		var tries = 0;
		while (tries++ < 1000 && !(seenNull && seenValue))
		{
			var value = generator.Generate();
			if (value is null)
				seenNull = true;
			else
				seenValue = true;
		}
		Assert.True(seenNull, "Never saw null in 1000 tries");
		Assert.True(seenValue, "Never saw non-null in 1000 tries");
	}

	[Fact]
	[DocContent(
@"- `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .")]
	public void NullableWithArgument()
	{
		// really don't know how to test this one
	}

	[Fact]
	[DocContent(
@"- `.NullableRef()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.  
> Used for reference types, including `string`.")]
	public void NullableRef()
	{
		var generator = Fuzz.String().NullableRef();
		var seenNull = false;
		var seenValue = false;
		var tries = 0;
		while (tries++ < 1000 && !(seenNull && seenValue))
		{
			var value = generator.Generate();
			if (value is null)
				seenNull = true;
			else
				seenValue = true;
		}
		Assert.True(seenNull, "Never saw null in 1000 tries");
		Assert.True(seenValue, "Never saw non-null in 1000 tries");
	}

	[Fact]
	[DocContent(
@"- `.NullableRef(int timesBeforeResultIsNullAproximation)` : overload of `NullableRef()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .")]
	public void NullableRefWithArgument()
	{
		// really don't know how to test this one
	}

	[Fact]
	[DocContent(
@"- `.NeverReturnNull()` : Only available on generators that provide `Nullable<T>` values, this one makes sure that, you guessed it, the nullable generator never returns null.")]
	public void NeverNull()
	{
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(42, Fuzz.Constant(42).Nullable().NeverReturnNull().Generate());
		}
	}
}