using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Float()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random single-precision numbers (default 1-100).")]
public class Floats
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Float(float min, float max)` generates a float greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var fuzzr = Fuzzr.Float(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, fuzzr.Generate());
		}
	}

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Float(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default fuzzr is (min = 1, max = 100).")]
	public void DefaultFuzzrBetweenOneAndHundred()
	{
		var fuzzr = Fuzzr.Float();
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
		var fuzzr = Fuzzr.Float().Nullable();
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
		public float AProperty { get; set; }
		public float? ANullableProperty { get; set; }
	}
}