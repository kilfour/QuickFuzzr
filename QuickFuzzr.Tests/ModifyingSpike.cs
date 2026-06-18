using QuickFuzzr.Tests._Tools.Models;

namespace QuickFuzzr.Tests;

public class ModifyingSpike
{
    [Fact]
    public void Riffing()
    {
        var result = Fuzzr.One<Person>().Generate(42);
        Assert.Equal("ddnegsn", result.Name);
        Assert.Equal(18, result.Age);
        var modifier =
            from _ in Configr<Person>.Ignore(a => a.Name)
            from p in Fuzzr.One(() => result)
            select p;
        modifier.Generate(666);
        Assert.Equal("ddnegsn", result.Name);
        Assert.Equal(66, result.Age);
    }
}