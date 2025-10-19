using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[DocContent("Use `Fuzz.Int()`.")]
public class Ints
{
	[Fact]
	[DocContent("- The overload `Fuzz.Int(int min, int max)` generates an int higher or equal than min and lower than max.")]
	public void Zero()
	{
		var generator = Fuzzr.Int(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, generator.Generate());
		}
	}

	[Fact]
	[DocContent("- Throws an ArgumentException if min > max.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Int(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGeneratorNeverGeneratesZero()
	{
		var generator = Fuzzr.Int();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= 1);
			Assert.True(val < 100);
		}
	}

	[Fact]
	[DocContent("- Can be made to return `int?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.Int().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 100; i++)
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
	[DocContent("- `int` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `Int32` is automatically detected and generated for object properties.")]
	public void Int32Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AnInt32Property);
		}
	}

	[Fact]
	[DocContent("- `int?` is automatically detected and generated for object properties.")]
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
		public int AProperty { get; set; }
		public Int32 AnInt32Property { get; set; }
		public int? ANullableProperty { get; set; }
	}
}