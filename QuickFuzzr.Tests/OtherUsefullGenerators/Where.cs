using QuickPulse.Explains;

namespace QuickFuzzr.Tests.OtherUsefullGenerators;

[Doc(Order = "1-5-4",
	Content = "Use the `.Where(Func<T, bool>)` extension method.")]
public class Where
{
	[Fact]
	[Doc(Order = "1-5-4-1",
		Content =
			@"Makes sure that every generated value passes the supplied predicate.")]
	public void Filters()
	{
		var generator = Fuzz.ChooseFromThese(1, 2, 3).Where(a => a != 1);
		for (int i = 0; i < 100; i++)
		{
			var value = generator.Generate();
			Assert.NotEqual(1, value);
		}
	}

	[Fact]
	public void WorksWithAllGenerators()
	{
		var generator = Fuzz.Int(1, 5).Where(a => a != 1);
		for (int i = 0; i < 100; i++)
		{
			var value = generator.Generate();
			Assert.NotEqual(1, value);
		}
	}
}