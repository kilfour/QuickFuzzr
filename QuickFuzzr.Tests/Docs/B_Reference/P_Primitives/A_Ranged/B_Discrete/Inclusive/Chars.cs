using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Inclusive;


[DocContent("Use `Fuzzr.Char()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random lowercase letters or characters within a specified range.")]
[DocContent("- **Default Range:** min = 'a', max = 'z'.")]
public class Chars : RangedPrimitive<char>
{
	protected override FuzzrOf<char> CreateFuzzr() => Fuzzr.Char();
	protected override FuzzrOf<char> CreateRangedFuzzr(char min, char max) => Fuzzr.Char(min, max);
	protected override (char Min, char Max) DefaultRange => ('a', 'z');
	protected override (char Min, char Max) ExampleRange => ('5', '7');
	protected override (char Min, char Max) MinimalRange => ('0', '1');
	protected override bool UpperBoundExclusive => false;
}