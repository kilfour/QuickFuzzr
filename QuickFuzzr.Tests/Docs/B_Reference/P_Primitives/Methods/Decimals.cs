using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Decimal()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random decimal numbers (default 1-100).")]
public class Decimals
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var fuzzr = Fuzzr.Decimal(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, fuzzr.Generate());
		}
	}
	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Decimal(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default fuzzr is (min = 1, max = 100).")]
	public void DefaultFuzzrBetweenOneAndHundred()
	{
		var fuzzr = Fuzzr.Decimal();
		for (int i = 0; i < 10; i++)
		{
			var val = fuzzr.Generate();
			Assert.True(val >= 1);
			Assert.True(val < 100);
		}
	}

	[Fact]
	[DocContent("- Can be made to return `decimal?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var fuzzr = Fuzzr.Decimal().Nullable();
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
	[DocContent("- `decimal` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, fuzzr.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `decimal?` is automatically detected and generated for object properties.")]
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
		public decimal AProperty { get; set; }
		public decimal? ANullableProperty { get; set; }
	}
}