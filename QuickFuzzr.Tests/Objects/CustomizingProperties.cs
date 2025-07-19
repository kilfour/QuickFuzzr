using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr.Tests.Objects
{
	[CustomizingProperties(
		Content =
@"Use the `Fuzz.For<T>().Customize<TProperty>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
Fuzz.For<SomeThingToGenerate>().Customize(s => s.MyProperty, Fuzz.Constant(42))
```",
		Order = 0)]
	public class CustomizingProperties
	{
		[Fact]
		[CustomizingProperties(
			Content = "The property specified will be generated using the passed in generator.",
			Order = 1)]
		public void StaysDefaultValue()
		{
			var generator =
				from c in Fuzz.For<SomeThingToGenerate>().Customize(s => s.AnInt, Fuzz.Constant(42))
				from r in Fuzz.One<SomeThingToGenerate>()
				select r;
			Assert.Equal(42, generator.Generate().AnInt);
		}

		[Fact]
		[CustomizingProperties(
			Content = "An overload exists which allows for passing a value instead of a generator.",
			Order = 2)]
		public void UsingValue()
		{
			var generator =
				from c in Fuzz.For<SomeThingToGenerate>().Customize(s => s.AnInt, 42)
				from r in Fuzz.One<SomeThingToGenerate>()
				select r;
			Assert.Equal(42, generator.Generate().AnInt);
		}

		[Fact]
		[CustomizingProperties(
			Content = "Derived classes generated also use the custom property.",
			Order = 3)]
		public void WorksForDerived()
		{
			var generator =
				from _ in Fuzz.For<SomeThingToGenerate>().Customize(s => s.AnInt, 42)
				from result in Fuzz.One<SomeThingDerivedToGenerate>()
				select result;
			Assert.Equal(42, generator.Generate().AnInt);
		}

		//[Fact(Skip="WIP")]
		//[CustomizingProperties(
		//    Content = "This does not work for fields yet.",
		//    Order = 4)]
		//public void Field()
		//{
		//    var generator =
		//        from _ in Fuzz.For<SomeThingToGenerate>().Customize(s => s.AnIntField, 42)
		//        from result in Fuzz.One<SomeThingDerivedToGenerate>()
		//        select result;
		//    Assert.Equal(42, generator.Generate().AnIntField);
		//}

		[Fact]
		[CustomizingProperties(
			Content = "*Note :* The Customize combinator does not actually generate anything, it only influences further generation.",
			Order = 99)]
		public void ReturnsUnit()
		{
			var generator = Fuzz.For<SomeThingToGenerate>().Customize(s => s.AnInt, 42);
			Assert.Equal(Unit.Instance, generator.Generate());
		}
		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
			public int AnIntField;
		}

		public class SomeThingDerivedToGenerate : SomeThingToGenerate
		{
		}

		public class CustomizingPropertiesAttribute : GeneratingObjectsAttribute
		{
			public CustomizingPropertiesAttribute()
			{
				Caption = "Customizing properties.";
				CaptionOrder = 5;
			}
		}
	}
}