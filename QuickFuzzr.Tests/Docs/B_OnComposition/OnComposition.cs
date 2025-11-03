using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.Tests.Docs.A_YourFirstFuzzr;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_OnComposition;


[DocFile]
[DocContent("In the previous chapter, the examples kind of went from zero to sixty in under three, so let's step back a bit and have a look under the hood.")]
public class OnComposition
{
    [Fact]
    [DocHeader("Fuzzr")]
    [DocContent(
@"The basic building block of QuickFuzzr is a `FuzzrOf<T>`. This is in essence a function that returns a random value of type `T`.  
As you have seen, QuickFuzzr is *`LINQ`-enabled*, meaning these building blocks can be chained together using `LINQ`.  
The first way to create `FuzzrOf<T>` instances is by calling the methods on the static factory class `Fuzzr`.   

Let's look at the *social security number Fuzzr* from before, for instance:")]
    [DocExample(typeof(YourFirstFuzzr), nameof(YourFirstFuzzr.SsnFuzzr))]
    [DocContent(
@"Here the `Fuzzr.Int(...)` calls produce a `FuzzrOf<int>`, so that in the left side of the `LINQ` statements, the range variables are of type `int`.  
The `select` then combines those using string interpolation, meaning that the whole thing is also a `Fuzzr` but now of type `FuzzrOf<string>`.  


*Sidenote:* There's usually more than one way to get things done in QuickFuzzr.
As an example of that, here is another social security generator, this time using `char`'s and `string`'s:")]
    [DocExample(typeof(OnComposition), nameof(SsnFuzzr))]
    public void Composition()
    {
        var result = SsnFuzzr().Generate(42);
        Assert.Equal("114-26-1622", result);
    }

    [CodeSnippet]
    [CodeRemove("return ssnFuzzr;")]
    private static FuzzrOf<string> SsnFuzzr()
    {
        var digit = Fuzzr.Char('0', '9');
        var ssnFuzzr =
            from a in Fuzzr.String(digit, 3)
            from b in Fuzzr.String(digit, 2)
            from c in Fuzzr.String(digit, 4)
            select $"{a}-{b}-{c}";
        // Results in => "114-26-1622"
        return ssnFuzzr;
    }

    [Fact]
    [DocHeader("Configr")]
    [DocContent(
@"The second way of getting a hold of `FuzzrOf<T>` building blocks is by calling the methods on the static factory class `Configr`.
These however return a *special* type: a `FuzzrOf<Intent>`.  
This essentially means they do not return a value. In functional speak `Intent` is QuickFuzzr's `Unit` type.
The range variable in the `LINQ` chain is always ignored.

The calls to `Configr` in effect do not generate values, they exist to **influence generation** down the line.  
")]
    public void Configr_Just_Another_Fuzzr()
    {
        Assert.IsType<FuzzrOf<Intent>>(Configr.IgnoreAll());
    }

    [Fact]
    [DocContent("The fact that they are a form of `FuzzrOf<T>` however, allows us to sprinkle them into the `LINQ` chain and configure generation on the fly.")]
    [DocExample(typeof(OnComposition), nameof(Configr_inlines_Example))]
    public void Configr_inlines()
    {
        var result = Configr_inlines_Example().Generate(123);
        Assert.Equal("wsusbadepw", result.Name);
        Assert.Equal(720, result.Age);
    }

    [CodeSnippet]
    [CodeRemove("return personFuzzr;")]
    private static FuzzrOf<Person> Configr_inlines_Example()
    {
        var personFuzzr =
            from _ in Configr<Person>.Property(p => p.Age, Fuzzr.Int(666, 777))
            from person in Fuzzr.One<Person>()
            select person;
        // Results in => { Name: "wsusbadepw", Age: 720 }
        return personFuzzr;
    }

    [Fact]
    [DocHeader("Extension Methods")]
    [DocContent(
@"Lastly, there are some extension methods defined for `FuzzrOf<T>`. I think the most obvious one is `.Many(...)`.  
These extension methods wrap the `FuzzrOf<T>` they are attached to and again, influence generation.  
In the case of `.Many(...)`, it runs the original `Fuzzr` as many times as specified in the arguments and returns the resulting values as an `IEnumerable<T>`.  

Example:")]
    [DocExample(typeof(OnComposition), nameof(ExtensionMethods_Example))]
    public void ExtensionMethods()
    {
        var result = ExtensionMethods_Example().Generate(42).ToList();
        Assert.Equal(67, result[0]);
        Assert.Equal(14, result[1]);
        Assert.Equal(13, result[2]);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<IEnumerable<int>> ExtensionMethods_Example()
    {
        return Fuzzr.Int().Many(3);
        // Results in => [ 67, 14, 13 ]
    }
}