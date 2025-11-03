using QuickFuzzr.Tests._Tools.Models;

namespace QuickFuzzr.Tests;

public class Spike
{
    [Fact(Skip = "Not implemented, coz it's tricky")]
    public void ConfigrInjection()
    {
        var cfg42 = Configr<Person>.Property(a => a.Age, 42);
        var fuzzr =
            from _1 in Configr<Person>.Property(a => a.Age, 18)
            from _2 in Configr<Person>.Property(a => a.Name, "Fixed")
            from person1 in Fuzzr.One<Person>()
            from person2 in Fuzzr.One<Person>(/* cfg42 */) // <= uncomment, make it compile, make test pass
            from person3 in Fuzzr.One<Person>()
            select ((Person[])[person1, person2, person3]);
        var result = fuzzr.Generate();
        Assert.Equal(18, result[0].Age);
        Assert.Equal("Fixed", result[0].Name);
        Assert.Equal(42, result[1].Age);
        Assert.Equal("Fixed", result[1].Name);
        Assert.Equal(18, result[2].Age);
        Assert.Equal("Fixed", result[2].Name);
    }

    [Fact]
    public void Then()
    {
        var cfg42 = Configr<Person>.Property(a => a.Age, 42);
        var fuzzr = cfg42.Then(Fuzzr.One<Person>());
        Assert.Equal(42, fuzzr.Generate().Age);
    }

    [Fact]
    public void ThenAgain()
    {
        var cfg42 = Configr<Person>.Property(a => a.Age, 42);
        var fuzzr =
            from _ in Configr<Person>.Property(a => a.Age, 18)
            from person1 in Fuzzr.One<Person>()
            from person2 in cfg42.Then(Fuzzr.One<Person>())
            from person3 in Fuzzr.One<Person>()
            select ((Person[])[person1, person2, person3]);

        var result = fuzzr.Generate();
        Assert.Equal(18, result[0].Age);
        Assert.Equal(42, result[1].Age);
        Assert.Equal(42, result[2].Age); // <= correct but unintuitive
    }
}

public static class ExtFuzzrs
{
    public static FuzzrOf<T2> Then<T1, T2>(this FuzzrOf<T1> current, FuzzrOf<T2> next)
        => from _ in current
           from n in next
           select n;
}