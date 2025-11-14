using QuickFuzzr.Tests._Tools;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("Unique")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Ensures all generated values are unique within the given key scope.")]
[DocContent("Makes sure that every generated value is unique.")]
[DocSignature("ExtFuzzr.Unique<T>(this FuzzrOf<T> fuzzr, object key)")]
public class ExtFuzzrUnique
{
    [Fact]
    public void IsUnique()
    {
        var fuzzr = Fuzzr.OneOf(1, 2).Unique("TheKey").Many(2);
        for (int i = 0; i < 100; i++)
        {
            var value = fuzzr.Generate().ToArray();
            Assert.Equal(value[0] == 1 ? 2 : 1, value[1]);
        }
    }

    [Fact]
    [DocContent("- When asking for more unique values than the fuzzr can supply, an exception is thrown.")]
    public void Not_Enough_Values()
    {
        var fuzzr = Fuzzr.Constant(1).Unique("TheKey").Many(2);
        var ex = Assert.Throws<UniqueValueExhaustedException>(() => fuzzr.Generate().ToArray());
        Assert.Equal(Not_Enough_Values_Message(), ex.Message);
    }

    [CodeSnippet]
    [CodeRemove("@\"")]
    [CodeRemove("\";")]
    private static string Not_Enough_Values_Message() =>
@"Could not find a unique value of type Int32 using key ""TheKey"", after 64 attempts.

Possible solutions:
- Increase the retry limit globally: Configr.RetryLimit(256)
- Increase it locally: .Unique(""TheKey"", 256)
- Widen the value space (add more options or relax filters)
- Use a deterministic unique source (Counter for instance)
- Use a different uniqueness scope key to reset tracking
- Use a fallback: fuzzr.Unique(values).WithDefault()
";

    [Fact]
    [DocContent("- Multiple unique fuzzrs can be defined in one 'composed' fuzzr, without interfering with eachother by using a different key.")]
    public void Multiple()
    {
        for (int i = 0; i < 100; i++)
        {
            var fuzzr =
                from one in Fuzzr.OneOf(1, 2).Unique(1)
                from two in Fuzzr.OneOf(1, 2).Unique(2)
                select new[] { one, two };
            var value = fuzzr.Many(2).Generate().ToArray();
            var valueOne = value[0];
            var valueTwo = value[1];
            Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueTwo[0]);
            Assert.Equal(valueOne[1] == 1 ? 2 : 1, valueTwo[1]);
        }

    }

    [Fact]
    [DocContent("- When using the same key for multiple unique fuzzrs all values across these fuzzrs are unique.")]
    public void MultipleSameKey()
    {
        for (int i = 0; i < 100; i++)
        {
            var fuzzr =
                from one in Fuzzr.OneOf(1, 2).Unique(1)
                from two in Fuzzr.OneOf(1, 2).Unique(1)
                select new[] { one, two };

            var valueOne = fuzzr.Generate();
            Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
        }
    }

    [Fact]
    [DocContent("- An overload exist taking a function as an argument allowing for a dynamic key.")]
    public void Dynamic_Key()
    {
        for (int i = 0; i < 100; i++)
        {
            var fuzzr =
                from one in Fuzzr.OneOf(1, 2).Unique(() => 1)
                from two in Fuzzr.OneOf(1, 2).Unique(() => 1)
                select new[] { one, two };

            var valueOne = fuzzr.Generate();
            Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
        }
    }
}