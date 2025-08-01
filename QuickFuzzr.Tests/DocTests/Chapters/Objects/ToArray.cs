using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Objects;

[Doc(Order = "1-3-7",
Caption = "ToArray",
		Content = "Use The `.ToArray()` generator extension.")]
public class ToArray
{
	[Fact]
	[Doc(Order = "1-3-7-1",
		Content =
@"The `Many` generator above returns an IEnumerable.
This means it's value would be regenerated if we were to iterate over it more than once.
Use `ToArray` to *fix* the IEnumerable in place, so that it will return the same result with each iteration.
It can also be used to force evaluation in case the IEnumerable is not enumerated over because there's nothing in your select clause
referencing it. 
")]
	public void SameValues()
	{
		var values =
			(from ints in Fuzz.Int().Many(2).ToArray()
			 from one in Fuzz.Constant(ints)
			 from two in Fuzz.Constant(ints)
			 select new { one, two })
			.Generate();
		Assert.IsType<int[]>(values.one);
		Assert.Equal(values.one.ElementAt(0), values.two.ElementAt(0));
		Assert.Equal(values.one.ElementAt(1), values.two.ElementAt(1));
	}

	public class SomeThingToGenerate { }
}