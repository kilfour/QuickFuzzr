using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocContent("Use `Fuzz.ChooseFrom<T>(IEnumerable<T> values)`.")]
public class Choosing
{
	[Fact]
	[DocContent(
@"Picks a random value from a list of options.

F.i. `Fuzz.ChooseFrom(new []{ 1, 2 })` will return either 1 or 2.")]
	public void Enumerable()
	{
		var generator = Fuzzr.OneOf((IEnumerable<int>)(new[] { 1, 2 }));
		var one = false;
		var two = false;
		for (int i = 0; i < 20; i++)
		{
			var value = generator.Generate();
			one = one || value == 1;
			two = two || value == 2;
		}
		Assert.True(one);
		Assert.True(two);
	}

	[Fact]
	[DocContent(
@"A helper method exists for ease of use when you want to pass in constant values as in the example above. 

I.e. : `Fuzz.ChooseFromThese(1, 2)`")]
	public void Params()
	{
		var generator = Fuzzr.OneOf(1, 2);
		var one = false;
		var two = false;
		for (int i = 0; i < 20; i++)
		{
			var value = generator.Generate();
			one = one || value == 1;
			two = two || value == 2;
		}
		Assert.True(one);
		Assert.True(two);
	}

	[Fact]
	[DocContent(
@"Another method provides a _semi-safe_ way to pick from what might be an empty list. 

I.e. : `Fuzz.ChooseFromWithDefaultWhenEmpty(new List<int>())`, which returns the default, in this case zero.")]
	public void ParamsEmpty_int_list_returns_zero()
	{
		var generator = Fuzzr.OneOfOrDefault(new List<int>());
		Assert.Equal(0, generator.Generate());
	}

	[Fact]
	[DocContent(
@"You can also pick from a set of Generators. 

I.e. : `Fuzz.ChooseGenerator(Fuzz.Constant(1), Fuzz.Constant(2))`")]
	public void Gens()
	{
		var generator = Fuzzr.OneOf(Fuzzr.Constant(1), Fuzzr.Constant(2));
		var one = false;
		var two = false;
		for (int i = 0; i < 20; i++)
		{
			var value = generator.Generate();
			one = one || value == 1;
			two = two || value == 2;
		}
		Assert.True(one);
		Assert.True(two);
	}
}