using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters;

[DocFile]
[DocContent("The Fuzz class has many methods which can be used to obtain a corresponding primitive.")]
public class A_GeneratingPrimitives
{
	[Fact]
	[DocContent(
@"F.i. `Fuzzr.Int()`. 

Full details below in the chapter 'The Primitive Generators'.")]
	public void ForAnInt()
	{
		Assert.NotEqual(0, Fuzzr.Int().Generate());
	}
}