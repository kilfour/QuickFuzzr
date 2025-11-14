using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>.With")]
[DocSignature("Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)")]
[DocContent(
@"Applies configuration for type `T` based on a value produced by another fuzzr,
allowing dynamic, data-dependent configuration inside LINQ chains..")]
public class L_ConfigrWithT
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr<Person>.With(
            Fuzzr.Constant(42),
            v => Configr<Person>.Property(p => p.Age, v)
        );
        // Results in =>
        // Person { Name: "...", Age: 42 }
    }

    [Fact]
    //[DocUsage]
    //[DocExample(typeof(ConfigrWithT), nameof(GetConfig))]
    public void AppliesConfigurationFromGeneratedValue()
    {
        var fuzzr =
            from _ in GetConfig()
            from person in Fuzzr.One<Person>()
            select person;

        var result = fuzzr.Generate(42);
        Assert.Equal(42, result.Age);
    }
}
