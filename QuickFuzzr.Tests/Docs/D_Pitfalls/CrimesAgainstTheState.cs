namespace QuickFuzzr.Tests.Docs.C_Pitfalls;

public class CrimesAgainstTheState
{
    // QuickFuzzr follows LINQ rules: outer bindings happen once, and inner queries reuse them unless you explicitly rebind.
    // generators are monads, not “auto-refreshing” value streams.

    //     Chapter: Common Pitfalls (clear, findable)

    // "Thought Crimes Against the State" (closure issues)

    // "Public Enemies vs Private Citizens" (access modifier problems)

    // "Memory Holes" (state corruption issues)
}