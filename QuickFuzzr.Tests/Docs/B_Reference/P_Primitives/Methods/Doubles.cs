using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Double()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random double-precision numbers (default 1-100).")]
public class Doubles
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Double(double min, double max)` generates a double greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var fuzzr = Fuzzr.Double(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, fuzzr.Generate());
		}
	}

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Double(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default fuzzr is (min = 1, max = 100).")]
	public void DefaultFuzzrBetweenOneAndHundred()
	{
		var fuzzr = Fuzzr.Double();
		for (int i = 0; i < 10; i++)
		{
			var val = fuzzr.Generate();
			Assert.True(val >= 1);
			Assert.True(val < 100);
		}
	}

	[Fact]
	public void Nullable()
	{
		var fuzzr = Fuzzr.Double().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(0, value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	public void Property()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, fuzzr.Generate().AProperty);
		}
	}

	[Fact]
	public void NullableProperty()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate().ANullableProperty;
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(0, value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public double AProperty { get; set; }
		public double? ANullableProperty { get; set; }
	}
}