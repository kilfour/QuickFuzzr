using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.OtherUsefullGenerators;

[Doc(Order = "1-5-3", Caption = "Generating unique values",
	Content = "Use the `.Unique(object key)` extension method.")]
public class Unique
{
	[Fact]
	[Doc(Order = "1-5-3-1",
		Content =
			@"Makes sure that every generated value is unique.")]
	public void IsUnique()
	{
		var generator = Fuzz.ChooseFromThese(1, 2).Unique("TheKey").Many(2);
		for (int i = 0; i < 100; i++)
		{
			var value = generator.Generate().ToArray();
			Assert.Equal(value[0] == 1 ? 2 : 1, value[1]);
		}
	}

	[Fact]
	[Doc(Order = "1-5-3-2",
		Content =
			@"When asking for more unique values than the generator can supply, an exception is thrown.")]
	public void Throws()
	{
		var generator = Fuzz.Constant(1).Unique("TheKey").Many(2);
		var ex = Assert.Throws<HeyITriedFiftyTimesButCouldNotGetADifferentValue>(() => generator.Generate().ToArray());
		Assert.Contains("TheKey", ex.Message);
	}

	[Fact]
	[Doc(Order = "1-5-3-3",
		Content =
@"Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.")]
	public void Multiple()
	{
		for (int i = 0; i < 100; i++)
		{
			var generator =
				from one in Fuzz.ChooseFromThese(1, 2).Unique(1)
				from two in Fuzz.ChooseFromThese(1, 2).Unique(2)
				select new[] { one, two };
			var value = generator.Many(2).Generate().ToArray();
			var valueOne = value[0];
			var valueTwo = value[1];
			Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueTwo[0]);
			Assert.Equal(valueOne[1] == 1 ? 2 : 1, valueTwo[1]);
		}

	}

	[Fact]
	[Doc(Order = "1-5-3-4",
		Content =
@"When using the same key for multiple unique generators all values across these generators are unique.")]
	public void MultipleSameKey()
	{
		for (int i = 0; i < 100; i++)
		{
			var generator =
				from one in Fuzz.ChooseFromThese(1, 2).Unique(1)
				from two in Fuzz.ChooseFromThese(1, 2).Unique(1)
				select new[] { one, two };

			var valueOne = generator.Generate();
			Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
		}
	}

	[Fact]
	[Doc(Order = "1-5-3-5",
		Content =
@"An overload exist taking a function as an argument allowing for a dynamic key.")]
	public void Dynamic_Key()
	{
		for (int i = 0; i < 100; i++)
		{
			var generator =
				from one in Fuzz.ChooseFromThese(1, 2).Unique(() => 1)
				from two in Fuzz.ChooseFromThese(1, 2).Unique(() => 1)
				select new[] { one, two };

			var valueOne = generator.Generate();
			Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
		}
	}
}