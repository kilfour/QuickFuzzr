using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration;

[DocFile]
[DocContent(
@"`Configr` provides a fluent API to influence how QuickFuzzr builds objects.
Use it to set global defaults, customize properties, control recursion depth,
select derived types, or wire dynamic behaviors that apply when calling `Fuzzr.One<T>()`.
")]
[DocHeader("Contents")]
[DocTable(nameof(Methods), Columns.Configr, Columns.Description)]
public class Configuring
{

    public static class Columns
    {
        public const string Configr = "Configr";
        public const string Description = "Description";
    }
}
