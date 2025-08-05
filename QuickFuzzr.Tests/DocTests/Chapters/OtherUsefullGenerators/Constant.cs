using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocFileHeader("'Generating' constants")]
[DocContent("Use `Fuzz.Constant<T>(T value)`.")]
public class Constant
{
	[Fact]
	[DocContent(
@"This generator is most useful in combination with others and is used to inject constants into combined generators.")]
	public void JustReturnsValue()
	{
		Assert.Equal(42, Fuzz.Constant(42).Generate());
	}
}