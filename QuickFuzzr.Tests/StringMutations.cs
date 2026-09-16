using QuickFuzzr.Strings;

namespace QuickFuzzr.Tests;

public class StringMutations
{
    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void No_Matching_Candidate_Preserves_The_String(string text)
    {
        var fuzzr = Fuzzr.Constant(text).ReplaceOneOf('x').WithOneOf(['y']);
        Assert.Equal(text, fuzzr.Generate(42));
    }

    [Fact]
    public void Replacing_In_An_Empty_String_Preserves_It()
    {
        Assert.Equal("", Fuzzr.Constant("").ReplaceOneWithOneOf(['x']).Generate(42));
    }

    [Fact]
    public void Repeated_Replacement_Can_Exhaust_The_Candidates()
    {
        var fuzzr = Fuzzr.Constant("aaa").Mutate(
            (source => source.ReplaceOneOf('a').WithOneOf(['b']), 5));
        Assert.Equal("bbb", fuzzr.Generate(42));
    }

    [Fact]
    public void Repeated_Replacement_Of_Empty_Strings_Composes_With_Insertion()
    {
        var fuzzr = Fuzzr.Constant("")
            .Mutate((source => source.ReplaceOneWithOneOf(['x']), 3))
            .InsertOneOf('y');
        Assert.Equal("y", fuzzr.Generate(42));
    }

    [Fact]
    public void Replacement_Changes_Exactly_One_Eligible_Position()
    {
        foreach (var seed in Enumerable.Range(0, 100))
        {
            var targeted = Fuzzr.Constant("aba").ReplaceOneOf('a').WithOneOf(['x']).Generate(seed);
            Assert.Contains(targeted, new[] { "xba", "abx" });
            var any = Fuzzr.Constant("abc").ReplaceOneWithOneOf(['x']).Generate(seed);
            Assert.Contains(any, new[] { "xbc", "axc", "abx" });
        }
    }
}
