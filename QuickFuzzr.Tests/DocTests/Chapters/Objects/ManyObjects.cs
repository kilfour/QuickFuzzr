using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Objects;

[DocFile]
[DocContent("Use The `.Many(int number)` generator extension.")]
public class ManyObjects
{
	[Fact]
	[DocContent("The generator will generate an IEnumerable<T> of `int number` elements where T is the result type of the extended generator.")]
	public void CorrectAmountOfElements()
	{
		var values = Fuzz.One<SomeThingToGenerate>().Many(2).Generate();
		Assert.Equal(2, values.Count());
		Assert.IsAssignableFrom<IEnumerable<SomeThingToGenerate>>(values);
	}

	[Fact]
	[DocContent("An overload exists (`.Many(int min, int max`) where the number of elements is in between the specified arguments.")]
	public void CorrectAmountOfElementsMinMax()
	{
		var values = Fuzz.One<SomeThingToGenerate>().Many(2, 2).Generate();
		Assert.Equal(2, values.Count());
		Assert.IsAssignableFrom<IEnumerable<SomeThingToGenerate>>(values);
	}

	public class SomeThingToGenerate { }
}