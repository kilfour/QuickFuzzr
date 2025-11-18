using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.A_Continuous;

[DocContent(
@"Values come from a dense numeric space (floating-point types).

For these, the upper bound is conceptually exclusive ([min, max)),
but floating-point rounding may occasionally allow max to appear.
This behaviour is explicitly tested and documented.
")]
[DocInclude(typeof(Decimals))]
[DocInclude(typeof(Doubles))]
[DocInclude(typeof(Floats))]
[DocInclude(typeof(Halfs))]
public class Continuous
{

}