namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

public class ConstructionException(string typeName) : QuickFuzzrException(BuildMessage(typeName))
{
    private static string BuildMessage(string typeName) =>
$@"Cannot generate instance of {typeName}.

Possible solutions:
• Add a parameterless constructor
• Register a custom constructor: Configr<{typeName}>.Construct(...)
• Use explicit generation: from x in Fuzzr.Int() ... select new {typeName}(x)
• Use the factory method overload: Fuzzr.One<T>(Func<T> constructor)
";
}
