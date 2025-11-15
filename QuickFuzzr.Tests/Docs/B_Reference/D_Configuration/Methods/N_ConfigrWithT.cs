using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>.With")]
[DocColumn(Configuring.Columns.Description, "Applies configuration for T based on a generated value.")]
[DocSignature("Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)")]
[DocContent(
@"Applies configuration for type `T` based on a value produced by another Fuzzr,
allowing dynamic, data-dependent configuration inside LINQ chains.
")]
// [DocContent(
// @"This is a niche feature, but it can be necessary in some cases.  
// It is useful for solving the classic 'captured variable in closure' problem that trips people up with LINQ and functional composition,
// but you should reach for other solutions first.  

// The usage example shown here is a bit contrived, but it explains the basic idea. 
// ")]
public class N_ConfigrWithT
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
