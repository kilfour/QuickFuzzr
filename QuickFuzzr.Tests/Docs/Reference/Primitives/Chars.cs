using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.Char()`.")]
public class Chars
{
	private readonly char[] valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

	[Fact]
	[DocContent("- The default generator always generates a char between lower case 'a' and lower case 'z'.")]
	public void DefaultGeneratorAlwaysBetweenLowerCaseAAndLowerCaseZ()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Char(),
			("char between lower case 'a' and lower case 'z'", val => valid.Any(c => c == val)));
	}

	[Fact]
	public void IsRandom()
	{
		var generator = Fuzzr.Char();
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
	[DocContent("- Can be made to return `char?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(Fuzzr.Char().Nullable());
	}

	[Fact]
	[DocContent("- `char` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			var value = generator.Generate().AProperty;
			Assert.True(valid.Any(c => c == value), value.ToString());
		}
	}

	[Fact]
	[DocContent("- `char?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<SomeThingToGenerate>().Select(x => x.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public char AProperty { get; set; }
		public Char ACharProperty { get; set; }
		public char? ANullableProperty { get; set; }
	}
}