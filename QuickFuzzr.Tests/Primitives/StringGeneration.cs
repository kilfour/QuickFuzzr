using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Primitives;

[Doc(Order = "1-6-3",
	Content = "Use `Fuzz.String()`.")]
public class StringGeneration
{
	[Fact]
	[Doc(Order = "1-6-3", Caption = "Strings",
		Content =
"The generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.")]
	public void DefaultGeneratorStringElementsAlwaysBetweenLowerCaseAAndLowerCaseZ()
	{
		var valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
		var generator = Fuzz.String();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val.All(s => valid.Any(c => c == s)), val);
		}
	}

	[Fact]
	[Doc(Order = "1-6-3-1",
		Content = "The overload `Fuzz.String(int min, int max)` generates an string of length higher or equal than min and lower than max.")]
	public void Zero()
	{
		var generator = Fuzz.String(5, 7);
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val.Length >= 5, string.Format("Length : {0}", val.Length));
			Assert.True(val.Length < 7, string.Format("Length : {0}", val.Length));
		}
	}

	[Fact]
	[Doc(Order = "1-6-3-2",
		Content = "The Default generator generates a string of length higher than 0 and lower than 10.")]
	public void DefaultGeneratorStringIsBetweenOneAndTen()
	{
		var generator = Fuzz.String();
		for (int i = 0; i < 10; i++)
		{
			var val = generator.Generate();
			Assert.True(val.Length > 0);
			Assert.True(val.Length < 10);
		}
	}

	[Fact]
	[Doc(Order = "1-6-3-3",
		Content = " - `string` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var generator = Fuzz.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			var value = generator.Generate().AProperty;
			Assert.NotNull(value);
			Assert.NotEqual("", value);
		}
	}

	[Fact]
	[Doc(Order = "1-6-3-4",
		Content = "Can be made to return `string?` using the `.NullableRef()` combinator.")]
	public void Nullable()
	{
		var generator = Fuzz.String().NullableRef();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = generator.Generate();
			if (value == null)
			{
				isSomeTimesNull = true;
			}
			else
				isSomeTimesNotNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public string? AProperty { get; set; }
	}
}