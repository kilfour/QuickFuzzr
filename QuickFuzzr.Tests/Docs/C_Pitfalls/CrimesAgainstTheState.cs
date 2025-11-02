namespace QuickFuzzr.Tests.Docs.C_Pitfalls;

public class CrimesAgainstTheState
{
    // QuickFuzzr follows LINQ rules: outer bindings happen once, and inner queries reuse them unless you explicitly rebind.
    // generators are monads, not “auto-refreshing” value streams.
}