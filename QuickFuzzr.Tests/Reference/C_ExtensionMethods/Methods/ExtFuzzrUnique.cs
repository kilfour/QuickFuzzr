using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader(".Unique&lt;T&gt;(...)")]
[DocContent("Using the `.Unique(object key)` extension method.")]
public class ExtFuzzrUnique
{
    [Fact]
    [DocContent("- Makes sure that every generated value is unique.")]
    public void IsUnique()
    {
        var generator = Fuzzr.OneOf(1, 2).Unique("TheKey").Many(2);
        for (int i = 0; i < 100; i++)
        {
            var value = generator.Generate().ToArray();
            Assert.Equal(value[0] == 1 ? 2 : 1, value[1]);
        }
    }

    [Fact]
    [DocContent("- When asking for more unique values than the generator can supply, an exception is thrown.")]
    public void Not_Enough_Values()
    {
        var generator = Fuzzr.Constant(1).Unique("TheKey").Many(2);
        var ex = Assert.Throws<UniqueValueExhaustedException>(() => generator.Generate().ToArray());
        Assert.Equal(Not_Enough_Values_Message(), ex.Message);
    }

    [CodeSnippet]
    [CodeRemove("@\"")]
    [CodeRemove("\";")]
    private static string Not_Enough_Values_Message() =>
@"Could not find a unique value of type Int32 using key ""TheKey"", after 64 attempts.

Possible solutions:
• Increase the retry limit globally: Configr.RetryLimit(256)
• Increase it locally: .Unique(""TheKey"", 256)
• Widen the value space (add more options or relax filters)
• Use a deterministic unique source (Counter for instance)
• Use a different uniqueness scope key to reset tracking
• Use a fallback: fuzzr.Unique(values).WithDefault()
";

    [Fact]
    [DocContent("- Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.")]
    public void Multiple()
    {
        for (int i = 0; i < 100; i++)
        {
            var generator =
                from one in Fuzzr.OneOf(1, 2).Unique(1)
                from two in Fuzzr.OneOf(1, 2).Unique(2)
                select new[] { one, two };
            var value = generator.Many(2).Generate().ToArray();
            var valueOne = value[0];
            var valueTwo = value[1];
            Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueTwo[0]);
            Assert.Equal(valueOne[1] == 1 ? 2 : 1, valueTwo[1]);
        }

    }

    [Fact]
    [DocContent("- When using the same key for multiple unique generators all values across these generators are unique.")]
    public void MultipleSameKey()
    {
        for (int i = 0; i < 100; i++)
        {
            var generator =
                from one in Fuzzr.OneOf(1, 2).Unique(1)
                from two in Fuzzr.OneOf(1, 2).Unique(1)
                select new[] { one, two };

            var valueOne = generator.Generate();
            Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
        }
    }

    [Fact]
    [DocContent("- An overload exist taking a function as an argument allowing for a dynamic key.")]
    public void Dynamic_Key()
    {
        for (int i = 0; i < 100; i++)
        {
            var generator =
                from one in Fuzzr.OneOf(1, 2).Unique(() => 1)
                from two in Fuzzr.OneOf(1, 2).Unique(() => 1)
                select new[] { one, two };

            var valueOne = generator.Generate();
            Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
        }
    }
}