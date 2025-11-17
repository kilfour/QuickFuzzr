using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.B_Other;

[DocFile]
[DocContent("Use `Fuzzr.Bool()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random `true` or `false` values. ")]
public class Booleans : Primitive<bool>
{
	[Fact]
	[DocContent("- Generates `true` or `false`.")]
	public void DefaultFuzzrGeneratesTrueOrFalse()
	{
		CheckIf.TheseValuesAreGenerated(Fuzzr.Bool(), true, false);
	}

	protected override FuzzrOf<bool> CreateFuzzr() => Fuzzr.Bool();

	[Fact]
	public override void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzzr.One<PrimitivesBag<bool>>().Select(x => x.Value), true, false);
	}
}