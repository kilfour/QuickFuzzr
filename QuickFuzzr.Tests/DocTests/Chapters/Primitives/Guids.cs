using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[DocContent("Use `Fuzz.Guid()`. *There is no overload.*")]
public class Guids
{
	[Fact]
	[DocContent("- The default generator never generates Guid.Empty.")]
	public void NeverGuidEmpty()
	{
		var generator = Fuzzr.Guid();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.NotEqual(Guid.Empty, val);
		}
	}

	[Fact]
	[DocContent("- Can be made to return `Guid?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzzr.Guid().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(Guid.Empty, value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `Guid` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(Guid.Empty, generator.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `Guid?` is automatically detected and generated for object properties.")]
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
				Assert.NotEqual(Guid.Empty, value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public Guid AProperty { get; set; }
		public Guid? ANullableProperty { get; set; }
	}
}