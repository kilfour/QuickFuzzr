using QuickFuzzr.Strings;

namespace QuickFuzzr.Tests;

public class MutationArgumentValidation
{
    [Fact]
    public void Mutate_Rejects_Null_Source_And_Null_Or_Empty_Strategies_Immediately()
    {
        FuzzrOf<int> missing = null!;
        Assert.Throws<ArgumentNullException>("fuzzr", () => missing.Mutate((source => source, 1)));
        Assert.Throws<ArgumentNullException>("mutations", () => Fuzzr.Constant(1).Mutate(null!));
        Assert.Throws<ArgumentException>("mutations", () => Fuzzr.Constant(1).Mutate());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Mutate_Rejects_Null_Transformations_Even_After_A_Valid_Strategy(int count)
    {
        Func<FuzzrOf<int>, FuzzrOf<int>> missing = null!;
        Assert.Throws<ArgumentNullException>("mutation", () =>
            Fuzzr.Constant(1).Mutate((source => source, 1), (missing, count)));
    }

    [Fact]
    public void Mutate_Rejects_Negative_Fixed_Count()
    {
        Assert.Throws<ArgumentOutOfRangeException>("min", () =>
            Fuzzr.Constant(1).Mutate((source => source, -1)));
    }

    [Theory]
    [InlineData(-1, 2, "min")]
    [InlineData(0, -1, "max")]
    [InlineData(3, 2, "min")]
    public void Mutate_Rejects_Invalid_Ranges_Immediately(int min, int max, string parameter)
    {
        Assert.Throws<ArgumentOutOfRangeException>(parameter, () =>
            Fuzzr.Constant(1).Mutate((source => source, min, max)));
    }

    [Fact]
    public void String_Transformations_Reject_Null_Sources_Immediately()
    {
        FuzzrOf<string> missing = null!;
        Assert.Throws<ArgumentNullException>("fuzzr", () => missing.InsertOneOf('x'));
        Assert.Throws<ArgumentNullException>("fuzzr", () => missing.ReplaceOneOf('x'));
        Assert.Throws<ArgumentNullException>("fuzzr", () => missing.ReplaceOneWithOneOf(['x']));
        Assert.Throws<ArgumentNullException>("fuzzr", () => new StringFuzzrExt.Replacer(missing, ['x']));
    }

    [Fact]
    public void String_Transformations_Reject_Null_Choices_Immediately()
    {
        var source = Fuzzr.Constant("x");
        Assert.Throws<ArgumentNullException>("chars", () => source.InsertOneOf(null!));
        Assert.Throws<ArgumentNullException>("candidates", () => source.ReplaceOneOf(null!));
        Assert.Throws<ArgumentNullException>("chars", () => source.ReplaceOneWithOneOf(null!));
        Assert.Throws<ArgumentNullException>("replacements", () => source.ReplaceOneOf('x').WithOneOf(null!));
        Assert.Throws<ArgumentNullException>("candidates", () => new StringFuzzrExt.Replacer(source, null!));
    }

    [Fact]
    public void String_Transformations_Reject_Empty_Choices_Immediately()
    {
        var source = Fuzzr.Constant("x");
        Assert.Throws<ArgumentException>("chars", () => source.InsertOneOf());
        Assert.Throws<ArgumentException>("candidates", () => source.ReplaceOneOf());
        Assert.Throws<ArgumentException>("chars", () => source.ReplaceOneWithOneOf([]));
        Assert.Throws<ArgumentException>("replacements", () => source.ReplaceOneOf('x').WithOneOf([]));
        Assert.Throws<ArgumentException>("candidates", () => new StringFuzzrExt.Replacer(source, []));
    }

    [Fact]
    public void Valid_String_Choices_Still_Insert_And_Replace()
    {
        Assert.Equal("x", Fuzzr.Constant("").InsertOneOf('x').Generate(42));
        Assert.Equal("ayc", Fuzzr.Constant("abc").ReplaceOneOf('b').WithOneOf(['y']).Generate(42));
        Assert.Equal("y", Fuzzr.Constant("x").ReplaceOneWithOneOf(['y']).Generate(42));
    }
}
