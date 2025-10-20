using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.Decimal()`.")]
public class Decimals
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var generator = Fuzzr.Decimal(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, generator.Generate());
		}
	}
	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Decimal(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGeneratorBetweenOneAndHundred()
	{
		var generator = Fuzzr.Decimal();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= 1);
			Assert.True(val < 100);
		}
	}

	[Fact]
	[DocContent("- Can be made to return `decimal?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.Decimal().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
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
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `decimal?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate().ANullableProperty;
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