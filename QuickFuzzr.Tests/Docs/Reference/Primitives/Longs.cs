using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.Long()`.")]
public class Longs
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Long(long min, long max)` generates a long higher or equal than min and lower than max.")]
	public void Zero()
	{
		var generator = Fuzzr.Long(0, 0);
		for (long i = 0; i < 10; i++)
		{
			Assert.Equal(0, generator.Generate());
		}
	}

	[Fact]
	[DocContent("Throws an ArgumentException if min > max.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Long(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGeneratorNeverGeneratesZero()
	{
		var generator = Fuzzr.Long();
		for (long i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate());
		}
	}

	[Fact]
	[DocContent("- Can be made to return `long?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.Long().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (long i = 0; i < 50; i++)
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
	[DocContent("- `long` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (long i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `Int64` is automatically detected and generated for object properties.")]
	public void Long32Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (long i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AnInt64Property);
		}
	}

	[Fact]
	[DocContent("- `long?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (long i = 0; i < 50; i++)
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
		public long AProperty { get; set; }
		public Int64 AnInt64Property { get; set; }
		public long? ANullableProperty { get; set; }
	}
}