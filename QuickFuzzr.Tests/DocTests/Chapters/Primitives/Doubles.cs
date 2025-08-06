using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[DocContent("Use `Fuzz.Double()`.")]
public class Doubles
{
	[Fact]
	[DocContent("- The overload `Fuzz.Double(double min, double max)` generates a double higher or equal than min and lower than max.")]
	public void Zero()
	{
		var generator = Fuzz.Double(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, generator.Generate());
		}
	}

	[Fact]
	[DocContent("- Throws an ArgumentException if min > max.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzz.Double(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGeneratorBetweenOneAndHundred()
	{
		var generator = Fuzz.Double();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= 1);
			Assert.True(val < 100);
		}
	}

	[Fact]
	[DocContent("- Can be made to return `double?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzz.Double().Nullable();
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
	[DocContent("- `double` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzz.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `double?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		var generator = Fuzz.One<SomeThingToGenerate>();
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
		public double AProperty { get; set; }
		public double? ANullableProperty { get; set; }
	}
}