using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;


[DocContent("Use the `.Where(Func<T, bool>)` extension method.")]
public class FilteringValues
{
	[Fact]
	[DocContent("Makes sure that every generated value passes the supplied predicate.")]
	public void Filters()
	{
		var generator = Fuzzr.OneOf(1, 2, 3).Where(a => a != 1);
		for (int i = 0; i < 100; i++)
		{
			var value = generator.Generate();
			Assert.NotEqual(1, value);
		}
	}

	[Fact]
	public void WorksWithAllGenerators()
	{
		var generator = Fuzzr.Int(1, 5).Where(a => a != 1);
		for (int i = 0; i < 100; i++)
		{
			var value = generator.Generate();
			Assert.NotEqual(1, value);
		}
	}
}