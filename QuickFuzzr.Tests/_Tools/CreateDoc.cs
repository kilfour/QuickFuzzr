using System.Text;
using QuickFuzzr;
using QuickFuzzr.Tests.DocTests;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools;

public class CreateDoc
{
	[Fact]
	public void Now()
	{
		Explain.This<QuickFuzzrTests>("README-TEMP.md");
	}
}
