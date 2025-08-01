

using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Primitives
{
	[Doc(Order = "1-1",
		Caption = "Generating Primitives",
		Content = "The Fuzz class has many methods which can be used to obtain a corresponding primitive.")]
	public class JustAnExample
	{
		[Fact]
		[Doc(Order = "1-1-1",
			Caption = "Introduction",
			Content =
@"F.i. `Fuzz.Int()`. 

Full details below in the chapter 'The Primitive Generators'.")]
		public void ForAnInt()
		{
			Assert.NotEqual(0, Fuzz.Int().Generate());
		}
	}
}