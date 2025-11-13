using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Bool()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random `true` or `false` values. ")]
public class Booleans
{
	[Fact]
	[DocContent("- Generates True or False.")]
	public void DefaultFuzzrGeneratesTrueOrFalse()
	{
		CheckIf.TheseValuesAreGenerated(Fuzzr.Bool(), true, false);
	}

	[Fact]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(Fuzzr.Bool().Nullable());
	}

	[Fact]
	public void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzzr.One<PrimitivesBag<bool>>().Select(x => x.Value), true, false);
	}

	[Fact]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<bool>>().Select(x => x.NullableValue));
	}
}