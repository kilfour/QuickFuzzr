using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.OtherUsefullGenerators;

[Doc(Order = "1-5-5", Caption = "Casting Generators",
	Content = "Various extension methods allow for casting the generated value.")]
public class Casting
{
	[Fact]
	[Doc(Order = "1-5-5-1",
		Content =
@" - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<T>` to `Generator<string>`. 
Useful f.i. to generate numeric strings.")]
	public void AsString()
	{
		Assert.IsType<string>(Fuzz.Int().AsString().Generate());
	}

	[Fact]
	[Doc(Order = "1-5-5-2",
		Content =
@" - `.AsObject()` : Simply casts the generator itself from `Generator<T>` to `Generator<object>`. Mostly used internally.")]
	public void AsObject()
	{
		Assert.IsType<Generator<object>>(Fuzz.Int().AsObject());
	}

	[Fact]
	[Doc(Order = "1-5-5-3",
		Content =
@" - `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.")]
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
	[Doc(Order = "1-5-5-4",
		Content =
@" - `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .")]
	public void NullableWithArgument()
	{
		// really don't know how to test this one
	}
}