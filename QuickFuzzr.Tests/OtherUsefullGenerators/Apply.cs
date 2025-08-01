using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.OtherUsefullGenerators;

[Doc(Order = "1-5-1", Caption = "Apply",
	Content = "Use the `.Apply<T>(Func<T, T> func)` extension method.")]
public class Apply
{
	[Fact]
	[Doc(Order = "1-5-1-1",
		Content =
@"Applies the specified Function to the generated value, returning the result.
F.i. `Fuzz.Constant(41).Apply(i =>  i + 1)` will return 42.")]
	public void FunctionIsApplied()
	{
		var generator = Fuzz.Constant(41).Apply(i => i + 1);
		Assert.Equal(42, generator.Generate());
	}

	[Fact]
	[Doc(Order = "1-5-1-2",
		Content =
@"Par example, when you want all decimals to be rounded to a certain precision : 
```
var generator = 
	from _ in Fuzz.Decimal().Apply(d => Math.Round(d, 2)).Replace()
	from result in Fuzz.One<SomeThingToGenerate>()
	select result;
```")]
	public void RoundingExample()
	{
		var generator =
			from _ in Fuzz.Decimal().Apply(d => Math.Round(d, 2)).Replace()
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;
		var value = generator.Generate().MyProperty;
		//var count = BitConverter.GetBytes(decimal.GetBits(generator.Generate().MyProperty)[3])[2];
		var count = value.ToString().Split('.', ',')[1].Count();
		Assert.Equal(2, count);
	}

	[Fact]
	[Doc(Order = "1-5-1-3",
		Content =
@"An overload exists with signature `Apply<T>(Action<T> action)`.
This is useful when dealing with objects and you just don't want to return said object.
E.g. `Fuzz.One<SomeThingToGenerate>().Apply(session.Save)`.")]
	public void ActionIsApplied()
	{
		var generator = Fuzz.One<SomeThingToGenerate>().Apply(thing => thing.MyProperty = 42);

		Assert.Equal(42, generator.Generate().MyProperty);
	}

	[Fact]
	[Doc(Order = "1-5-1-4",
		Content =
@"This function also exists as a convention instead of a generator.

E.g. `Fuzz.For<SomeThingToGenerate>().Apply(session.Save)`.

In this case nothing is generated but instead the function will be applied to all objects of type T during generation.

There is no `Fuzz.For<T>().Apply(Func<T, T> func)` as For can only be used for objects, so there is no need for it really.
")]
	public void AsConventionWithGenerator()
	{
		var generator = Fuzz.For<SomeThingToGenerate>().Apply(thing => thing.MyProperty = 42);
		Assert.Equal(Unit.Instance, generator.Generate());

		var newGenerator =
			from g in generator
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;

		Assert.Equal(42, newGenerator.Generate().MyProperty);
	}

	[Fact]
	[Doc(Order = "1-5-1-5",
		Content =
@"Lastly the convention based `Apply` has an overload which takes another generator.
This generator then provides a value which can be used in the action parameter.

E.g. : 
```
var parents = ...
Fuzz.For<SomeChild>().Apply(Fuzz.ChooseFrom(parents), (child, parent) => parent.Add(child))
```
")]
	public void AsConvention()
	{
		var generator =
			from convention in
				Fuzz.For<SomeThingToGenerate>()
					.Apply(Fuzz.Int(1, 3).Unique("SomeKey"), (thing, i) => thing.MyProperty = i)
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;

		var valueGen = generator.Many(2).Generate();
		var value = valueGen.ToList();
		var valueOne = value[0].MyProperty;
		var valueTwo = value[1].MyProperty;
		if (valueOne == 1)
			Assert.Equal(2, valueTwo);
		else
			Assert.Equal(1, valueTwo);
	}

	public class SomeThingToGenerate
	{
		public decimal MyProperty { get; set; }
	}

}