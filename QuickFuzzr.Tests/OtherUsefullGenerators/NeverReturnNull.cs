using QuickPulse.Explains;

namespace QuickFuzzr.Tests.OtherUsefullGenerators
{
	[Doc(Order = "1-12-1", Caption = "'Never return null",
		Content = "Use the `.NeverReturnNull()` extension method.`.")]
	public class NeverReturnNull
	{
		[Fact]
		[Doc(Order = "1-12-1-1",
			Content =
@"Only available on generators that provide `Nullable<T>` values, this one makes sure that, you guessed it, the nullable generator never returns null.")]
		public void NeverNull()
		{
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(42, Fuzz.Constant(42).Nullable().NeverReturnNull().Generate());
			}
		}
	}
}