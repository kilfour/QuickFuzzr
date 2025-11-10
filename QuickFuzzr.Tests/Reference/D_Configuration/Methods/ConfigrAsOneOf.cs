using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr&lt;T&gt;AsOneOf(params Type[] types)")]
public class ConfigrAsOneOf
{
    // Configures inheritance resolution for BaseType, allowing QuickFuzzr to randomly select one of the specified derived types when generating instances.

    // Enables polymorphic object graphs with controlled subtype variety.

    // The selection is random but reproducible when seeded.

    // Throws an ArgumentException if any listed type is not a valid subclass of BaseType.

    // Useful when generating domain hierarchies such as messages, events, or file system entries where multiple concrete subtypes exist.
    [DocUsage]
    [DocExample(typeof(ConfigrAsOneOf), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr<Person>.AsOneOf(typeof(Person), typeof(Employee));
    }
}