using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.Primitives.Methods;

[DocFile]
[DocFileHeader("TimeOnlys")]
[DocContent("Use `Fuzzr.TimeOnly()`.")]
public class TimeOnlys
{
	[Fact]
	[DocContent("- The overload `Fuzzr.TimeOnly(TimeOnly min, TimeOnly max)` generates a TimeOnly greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var generator = Fuzzr.TimeOnly(new TimeOnly(1, 0), new TimeOnly(1, 1));
		for (int i = 0; i < 10; i++)
		{
			var value = generator.Generate();
			Assert.True(value >= new TimeOnly(1, 0));
			Assert.True(value < new TimeOnly(1, 1));
		}
	}

	[Fact]
	[DocContent("- The default generator is (min = 00:00:00, max = 23:59:59.9999999).")]
	public void DefaultGeneratorNeverGeneratesZero()
	{
		var generator = Fuzzr.TimeOnly();
		for (int i = 0; i < 50; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= new TimeOnly(0));
			Assert.True(val <= TimeOnly.MaxValue);
		}
	}

	[Fact]
	[DocContent("- Can be made to return `TimeOnly?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.TimeOnly().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(new TimeOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `TimeOnly` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(new TimeOnly(), generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `TimeOnly?` is automatically detected and generated for object properties.")]
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
				Assert.NotEqual(new TimeOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public TimeOnly AProperty { get; set; }
		public TimeOnly? ANullableProperty { get; set; }
	}
}