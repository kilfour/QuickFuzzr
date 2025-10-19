using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocContent("Various extension methods allow for casting the generated value.")]
public class CastingGenerators
{
	[Fact]
	[DocContent(
@" - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<T>` to `Generator<string>`. 
Useful f.i. to generate numeric strings.")]
	public void AsString()
	{
		Assert.IsType<string>(Fuzzr.Int().AsString().Generate());
	}

	[Fact]
	[DocContent(
@" - `.AsObject()` : Simply casts the generator itself from `Generator<T>` to `Generator<object>`. Mostly used internally.")]
	public void AsObject()
	{
		Assert.IsType<Generator<object>>(Fuzzr.Int().AsObject());
	}

	[Fact]
	[DocContent(
@" - `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.")]
	public void Nullable()
	{
		var generator = Fuzzr.Int().Nullable();
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
@" - `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .")]
	public void NullableWithArgument()
	{
		// really don't know how to test this one
	}
}