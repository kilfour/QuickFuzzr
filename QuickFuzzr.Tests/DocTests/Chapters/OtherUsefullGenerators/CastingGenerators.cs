using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullFuzzrs;

[DocContent("Various extension methods allow for casting the generated value.")]
public class CastingFuzzrs
{
	[Fact]
	[DocContent(
@" - `.AsObject()` : Simply casts the fuzzr itself from `FuzzrOf<T>` to `FuzzrOf<object>`. Mostly used internally.")]
	public void AsObject()
	{
		Assert.IsType<FuzzrOf<object>>(Fuzzr.Int().AsObject());
	}

	[Fact]
	[DocContent(
@" - `.Nullable()` : Casts a `FuzzrOf<T>` to `FuzzrOf<T?>`. In addition generates null 1 out of 5 times.")]
	public void Nullable()
	{
		var fuzzr = Fuzzr.Int().Nullable();
		var seenNull = false;
		var seenValue = false;
		var tries = 0;
		while (tries++ < 1000 && !(seenNull && seenValue))
		{
			var value = fuzzr.Generate();
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