using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.OtherUsefullGenerators
{
	[Doc(Order = "1-5-6", Caption = "How About Null(s)?",
		Content = "Various extension methods allow for influencing null generation.")]
	public class HowAboutNull
	{
		[Fact]
		[Doc(Order = "1-5-6-1",
			Content =
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
		[Doc(Order = "1-5-6-2",
			Content =
@"- `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .")]
		public void NullableWithArgument()
		{
			// really don't know how to test this one
		}

		[Fact]
		[Doc(Order = "1-5-6-3",
			Content =
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
		[Doc(Order = "1-5-6-4",
			Content =
@"- `.NullableRef(int timesBeforeResultIsNullAproximation)` : overload of `NullableRef()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .")]
		public void NullableRefWithArgument()
		{
			// really don't know how to test this one
		}
	}
}