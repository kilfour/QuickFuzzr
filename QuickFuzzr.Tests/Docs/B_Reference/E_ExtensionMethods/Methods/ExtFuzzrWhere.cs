using QuickFuzzr.Tests._Tools;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("Where")]
[DocColumn(FuzzrExtensionMethods.Columns.Description,
"Filters generated values so only values satisfying the predicate are returned.")]
[DocSignature("ExtFuzzr.Where(this FuzzrOf<T> fuzzr, Func<T,bool> predicate)")]
[DocContent("Filters generated values to those satisfying the predicate.")]
public class ExtFuzzrWhere
{
    [Fact]
    public void Filters_By_Predicate()
    {
        var evens = Fuzzr.Int().Where(i => i % 2 == 0).Many(16).Generate(42).ToArray();
        Assert.True(evens.All(i => i % 2 == 0));
    }

    [Fact]
    [DocExceptions]
    [DocException("PredicateUnsatisfiedException", "When no value satisfies the predicate within the retry limit.")]
    public void Throws_When_Unsatisfied()
    {
        var fuzzr = Fuzzr.Constant(1).Where(_ => false);
        var ex = Assert.Throws<PredicateUnsatisfiedException>(() => fuzzr.Generate());
    }

    [Fact]
    [DocException("ArgumentNullException", "When the predicate is `null`.")]
    public void Null_Expression()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => Fuzzr.Constant(1).Where(null!));
        Assert.Equal("Value cannot be null. (Parameter 'predicate')", ex.Message);
    }
}
