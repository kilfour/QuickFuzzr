using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Objects;

[Doc(Order = "1-3-6",
	Caption = "Inheritance",
	Content =
@"Use The `Fuzz.For<T>().GenerateAsOneOf(params Type[] types)` method chain.

F.i. :
```
Fuzz.For<SomeThingAbstract>().GenerateAsOneOf(
	typeof(SomethingDerived), typeof(SomethingElseDerived))
```")]
public class Inheritance
{
	[Fact]
	[Doc(Order = "1-3-6-1",
		Content =
@"When generating an object of type T, an object of a random chosen type from the provided list will be generated instead.")]
	public void UsingDerived()
	{
		var generator =
			from _ in Fuzz.For<SomeThingAbstract>().GenerateAsOneOf(typeof(SomeThingDerivedToGenerate))
			from thing in Fuzz.One<SomeThingAbstract>()
			select thing;
		var result = generator.Generate();
		Assert.IsType<SomeThingDerivedToGenerate>(result);
	}

	[Fact]
	[Doc(Order = "1-3-6-2",
		Content = "**Note :** The `GenerateAsOneOf(...)` combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Fuzz.For<SomeThingAbstract>().GenerateAsOneOf(typeof(SomeThingDerivedToGenerate));
		Assert.Equal(Unit.Instance, generator.Generate());
	}

	public abstract class SomeThingAbstract
	{
		public int AnInt { get; set; }
	}

	public class SomeThingDerivedToGenerate : SomeThingAbstract { }
}