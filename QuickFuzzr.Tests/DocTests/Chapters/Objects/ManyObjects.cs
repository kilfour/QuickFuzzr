using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Objects;

[Doc(Order = "1-3-5",
Caption = "Many Objects",
	Content = "Use The `.Many(int number)` generator extension.")]
public class ManyObjects
{
	[Fact]
	[Doc(Order = "1-3-5-1",
		Content =
@"The generator will generate an IEnumerable<T> of `int number` elements where T is the result type of the extended generator.")]
	public void CorrectAmountOfElements()
	{
		var values = Fuzz.One<SomeThingToGenerate>().Many(2).Generate();
		Assert.Equal(2, values.Count());
		Assert.IsAssignableFrom<IEnumerable<SomeThingToGenerate>>(values);
	}

	[Fact]
	[Doc(Order = "1-3-5-2",
		Content =
@"An overload exists (`.Many(int min, int max`) where the number of elements is in between the specified arguments.")]
	public void CorrectAmountOfElementsMinMax()
	{
		var values = Fuzz.One<SomeThingToGenerate>().Many(2, 2).Generate();
		Assert.Equal(2, values.Count());
		Assert.IsAssignableFrom<IEnumerable<SomeThingToGenerate>>(values);
	}

	public class SomeThingToGenerate { }
}