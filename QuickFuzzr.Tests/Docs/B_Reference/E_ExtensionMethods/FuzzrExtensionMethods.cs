using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods;

[DocFile]
[DocContent(
@"QuickFuzzr provides a collection of extension methods that enhance the expressiveness and composability of `FuzzrOf<T>`.
These methods act as modifiers, they wrap existing fuzzrs to alter behavior, add constraints,
or chain side-effects without changing the underlying LINQ-based model.
")]
[DocHeader("Contents")]
[DocTable(nameof(Methods), Columns.Method, Columns.Description)]
public class FuzzrExtensionMethods
{
    public static class Columns
    {
        public const string Method = "Method";
        public const string Description = "Description";
    }
}