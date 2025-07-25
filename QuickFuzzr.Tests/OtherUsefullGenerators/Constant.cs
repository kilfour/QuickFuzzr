using QuickPulse.Explains;

namespace QuickFuzzr.Tests.OtherUsefullGenerators
{
	[Doc(Order = "1-5-7", Caption = "'Generating' constants",
		Content = "Use `Fuzz.Constant<T>(T value)`.")]
	public class Constant
	{
		[Fact]
		[Doc(Order = "1-5-7-1",
			Content =
@"This generator is most useful in combination with others and is used to inject constants into combined generators.")]
		public void JustReturnsValue()
		{
			Assert.Equal(42, Fuzz.Constant(42).Generate());
		}
	}
}