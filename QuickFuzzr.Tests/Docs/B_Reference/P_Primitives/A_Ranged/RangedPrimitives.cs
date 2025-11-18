using QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.A_Continuous;
using QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged;

[DocContent(
@"Ranged primitives generate numeric or temporal values between a minimum and a maximum.

For all of these the following is true:
- They have a paremeterless function which returns a value in a default range.
- They have an overload which allows you to specify a min and max value.
- They throw a `ArgumentOutOfRangeException` if the min is greater than the max.

Furthermore, there are two types of ranged primitives:
")]
[DocInclude(typeof(Continuous))]
[DocInclude(typeof(Discrete))]
public class RangedPrimitives
{

}

