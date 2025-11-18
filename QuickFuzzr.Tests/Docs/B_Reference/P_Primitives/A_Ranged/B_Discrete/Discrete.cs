using QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Exclusive;
using QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Inclusive;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete;

[DocContent(
@"Values come from a countable set (integers, shorts, bytes, dates snapped to seconds, etc.).

These appear in two flavours:")]
[DocHeader("Inclusive upper bound.")]
[DocContent(
@"The maximum value can be produced.  
Used when C# conventions or data types naturally use [min, max] (e.g. DateOnly, Byte, Char).")]
[DocInclude(typeof(Bytes))]
[DocInclude(typeof(Chars))]
[DocInclude(typeof(DateOnlys))]
[DocInclude(typeof(DateTimes))]
[DocHeader("Exclusive upper bound.")]
[DocContent(
@"The maximum value is never produced.  
Matches the C# RNG convention used by Random.Next(min, max) and applies to most integer fuzzrs.")]
[DocInclude(typeof(Ints))]
[DocInclude(typeof(Longs))]
[DocInclude(typeof(Shorts))]
[DocInclude(typeof(TimeOnlys))]
[DocInclude(typeof(TimeSpans))]
[DocInclude(typeof(UInts))]
[DocInclude(typeof(ULongs))]
[DocInclude(typeof(UShorts))]
public class Discrete
{

}