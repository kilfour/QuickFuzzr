using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Primitives;

[Doc(Order = "1-6-2", Caption = "Chars",
	Content = "Use `Fuzz.Char()`. \n\nNo overload exists.")]
public class CharGeneration
{
	private readonly char[] valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

	[Fact]
	[Doc(Order = "1-6-2-1",
		Content = "The default generator always generates a char between lower case 'a' and lower case 'z'.")]
	public void DefaultGeneratorAlwaysBetweenLowerCaseAAndLowerCaseZ()
	{
		var generator = Fuzz.Char();
		for (int i = 0; i < 100; i++)
		{
			var val = generator.Generate();
			Assert.True(valid.Any(c => c == val), val.ToString());
		}
	}

	[Fact]
	public void IsRandom()
	{
		var generator = Fuzz.Char();
		var val = generator.Generate();
		var differs = false;
		for (int i = 0; i < 10; i++)
		{
			if (val != generator.Generate())
				differs = true;
		}
		Assert.True(differs);
	}

	[Fact]
	[Doc(Order = "1-6-2-2",
		Content = "Can be made to return `char?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzz.Char().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.True(valid.Any(c => c == value.Value), value.Value.ToString());
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[Doc(Order = "1-6-2-3",
		Content = " - `char` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzz.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			var value = generator.Generate().AProperty;
			Assert.True(valid.Any(c => c == value), value.ToString());
		}
	}

	[Fact]
	[Doc(Order = "1-6-2-4",
		Content = " - `char?` is automatically detected and generated for object properties.")]
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
				Assert.True(valid.Any(c => c == value.Value), value.Value.ToString());
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public char AProperty { get; set; }
		public Char ACharProperty { get; set; }
		public char? ANullableProperty { get; set; }
	}
}