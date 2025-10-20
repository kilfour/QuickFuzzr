namespace QuickFuzzr.Tests.Docs;

public class CounterGeneratorTests
{
    [Fact]
    public void Counter_Generates_One()
        => Assert.Equal(1, Fuzzr.Counter().Generate());

    [Fact]
    public void Counter_Many_Produces_Expected_Run()
        => Assert.Equal([1, 2, 3, 4, 5], Fuzzr.Counter().Many(5).Generate());

    [Fact]
    public void Counter_Instances_Are_Independent()
    {
        var gen =
            from a in Fuzzr.Counter()
            from b in Fuzzr.Counter().Apply(x => x + 11)
            select (a, b);
        var result = gen.Many(2).Generate().ToArray();
        Assert.Equal(1, result[0].a);
        Assert.Equal(2, result[1].a);
        Assert.Equal(12, result[0].b);
        Assert.Equal(13, result[1].b);
    }

    [Fact]
    public void Counter_Resets_Between_Runs()
    {
        var gen = Fuzzr.Counter();
        Assert.Equal([1, 2, 3], gen.Many(3).Generate());
        Assert.Equal([1, 2, 3], gen.Many(3).Generate());
    }
}