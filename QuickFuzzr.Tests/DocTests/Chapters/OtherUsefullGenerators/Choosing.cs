using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocContent("Use `Fuzzr.ChooseFrom<T>(IEnumerable<T> values)`.")]
public class Choosing
{
	[Fact]
	[DocContent(
@"Picks a random value from a list of options.

F.i. `Fuzzr.ChooseFrom(new []{ 1, 2 })` will return either 1 or 2.")]
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

I.e. : `Fuzzr.ChooseFromThese(1, 2)`")]
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

I.e. : `Fuzzr.ChooseFromWithDefaultWhenEmpty(new List<int>())`, which returns the default, in this case zero.")]
	public void ParamsEmpty_int_list_returns_zero()
	{
		List<int> list = [];
		var generator = Fuzzr.OneOf((IEnumerable<int>)list).WithDefault();
		Assert.Equal(0, generator.Generate());
	}

	[Fact]
	[DocContent(
@"You can also pick from a set of Generators. 

I.e. : `Fuzzr.ChooseGenerator(Fuzzr.Constant(1), Fuzzr.Constant(2))`")]
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