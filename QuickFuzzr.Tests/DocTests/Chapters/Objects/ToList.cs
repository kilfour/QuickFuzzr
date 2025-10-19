using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent("Use The `.ToList()` generator extension.")]
public class ToList
{
	[Fact]
	[DocContent("Similar to the `ToArray` method. But instead of an Array, this one returns, you guessed it, a List.")]
	public void SameValues()
	{
		var values =
			(from ints in Fuzzr.Int().Many(2).ToList()
			 from one in Fuzzr.Constant(ints)
			 from two in Fuzzr.Constant(ints)
			 select new { one, two })
			.Generate();
		Assert.IsType<List<int>>(values.one);
		Assert.Equal(values.one.ElementAt(0), values.two.ElementAt(0));
		Assert.Equal(values.one.ElementAt(1), values.two.ElementAt(1));
	}

	public class SomeThingToGenerate { }
}