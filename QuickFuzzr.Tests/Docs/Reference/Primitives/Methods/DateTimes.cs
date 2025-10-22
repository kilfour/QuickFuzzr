using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives.Methods;

[DocFile]
[DocFileHeader("DateTimes")]
[DocContent("Use `Fuzzr.DateTime()`.")]
public class DateTimes
{
	[Fact]
	[DocContent("- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a DateTime greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var generator = Fuzzr.DateTime(new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));
		for (int i = 0; i < 10; i++)
		{
			var value = generator.Generate().PulseToLog();
			Assert.True(value >= new DateTime(2000, 1, 1));
			Assert.True(value < new DateTime(2000, 1, 5));
		}
	}
	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.DateTime(new DateTime(2000, 1, 5), new DateTime(2000, 1, 1)).Generate());

	[Fact]
	[DocContent("- The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).")]
	public void DefaultGeneratorNeverGeneratesZero()
	{
		var generator = Fuzzr.DateTime();
		for (int i = 0; i < 50; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= new DateTime(1970, 1, 1));
			Assert.True(val < new DateTime(2020, 12, 31));
		}
	}

	[Fact]
	[DocContent("- Can be made to return `DateTime?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.DateTime().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(new DateTime(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `DateTime` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(new DateTime(), generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `DateTime?` is automatically detected and generated for object properties.")]
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
				Assert.NotEqual(new DateTime(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public DateTime AProperty { get; set; }
		public DateTime? ANullableProperty { get; set; }
	}
}