namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

// [Shorts(
// 	Content = "Use `Fuzz.Short()`.",
// 	Order = 0)]
public class Shorts
{
	[Fact]
	// [Shorts(
	// 	Content = "The overload `Fuzz.Short(short min, short max)` generates a short higher or equal than min and lower than max.",
	// 	Order = 1)]
	public void Zero()
	{
		var generator = Fuzz.Short(0, 0);
		for (int i = 0; i < 10; i++)
		{
			Assert.Equal(0, generator.Generate());
		}
	}

	[Fact]
	// [Shorts(
	// 	Content = "The default generator is (min = 1, max = 100).",
	// 	Order = 2)]
	public void DefaultGeneratorBetweenOneAndHundred()
	{
		var generator = Fuzz.Short();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val >= 1);
			Assert.True(val < 100);
		}
	}

	[Fact]
	// [Shorts(
	// 	Content = "Can be made to return `short?` using the `.Nullable()` combinator.",
	// 	Order = 3)]
	public void Nullable()
	{
		var generator = Fuzz.Short().Nullable();
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
	// [Shorts(
	// 	Content = " - `short` is automatically detected and generated for object properties.",
	// 	Order = 4)]
	public void Property()
	{
		var generator = Fuzz.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(0, generator.Generate().AProperty);
		}
	}

	[Fact]
	// [Shorts(
	// 	Content = " - `short?` is automatically detected and generated for object properties.",
	// 	Order = 5)]
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
		public short AProperty { get; set; }
		public short? ANullableProperty { get; set; }
	}
}