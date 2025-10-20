using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.DateOnly()`.")]
public class DateOnlys
{
	[Fact]
	[DocContent("- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly higher or equal than min and lower than max.")]
	public void Zero()
	{
		var generator = Fuzzr.DateOnly(new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 5));
		for (int i = 0; i < 10; i++)
		{
			var value = generator.Generate();
			Assert.True(value >= new DateOnly(2000, 1, 1));
			Assert.True(value < new DateOnly(2000, 1, 5));
		}
	}

	[Fact]
	[DocContent("- The default generator is (min = new DateOnly(1970, 1, 1), max = new DateOnly(2020, 12, 31)).")]
	public void DefaultGeneratorNeverGeneratesZero()
	{
		var generator = Fuzzr.DateOnly();
		for (int i = 0; i < 50; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= new DateOnly(1970, 1, 1));
			Assert.True(val < new DateOnly(2020, 12, 31));
		}
	}

	[Fact]
	[DocContent("- Can be made to return `DateOnly?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.DateOnly().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(new DateOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `DateOnly` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(new DateOnly(), generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `DateOnly?` is automatically detected and generated for object properties.")]
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
				Assert.NotEqual(new DateOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public DateOnly AProperty { get; set; }
		public DateOnly? ANullableProperty { get; set; }
	}
}