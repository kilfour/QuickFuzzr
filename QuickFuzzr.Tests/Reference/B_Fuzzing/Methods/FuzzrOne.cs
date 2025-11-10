using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.One&lt;T&gt;()")]
[DocContent(
@"Creates a generator that produces complete instances of type T using QuickFuzzr's automatic construction rules.  
Object properties are filled using default generators for their types unless configured otherwise.")]
public class FuzzrOne : ExplainMe<FuzzrOne>
{

    // Uses public, parameterless constructors by default.
    // Properties with public setters are automatically generated.
    // Enumerations are generated using Fuzzr.Enum<T>().
    // Object properties are recursively generated, respecting configured depth limits.


    // Fuzzr.One<T>(Func<T> constructor)
    // Creates a generator that produces instances of T by invoking the supplied factory on each generation. The instance then flows through the normal pipeline (composition, .Apply(...), .Many(...), etc.).
    // Behavior
    // Invokes constructor per value, yielding a fresh instance each time.
    // Plays nicely with other combinators and with Configr-based tweaks (e.g., property rules on T).
    // Deterministic under seeding for anything QuickFuzzr controls; any randomness inside your constructor is not seed-controlled by QuickFuzzr.
    // When to use
    // Types without a parameterless ctor, or when you want explicit construction but still stay in a LINQ flow.
    // One-off object creation where Configr<T>.Construct(...) would be overkill. (Prefer Configr<T>.Construct(...) for reusable/global rules.)
    // Notes
    // Exceptions thrown by your constructor bubble up unchanged.
    // Compared to Fuzzr.Constant(x), this overload avoids reference aliasing because it builds a new object each time.
    // Example

    [Fact]
    [DocContent("Trying to generate an abstract class throws an exception with the following message:")]
    [DocExample(typeof(FuzzrOne), nameof(Generating_Abstract_Class_Message), "text")]
    public void Generating_Abstract_Class_Shows_Helpfull_Exception()
    {
        var ex = Assert.Throws<InstantiationException>(() => Fuzzr.One<AbstractPerson>().Generate());
        Assert.Equal(Generating_Abstract_Class_Message(), ex.Message);
    }

    [CodeSnippet]
    [CodeRemove("@\"")]
    [CodeRemove("\";")]
    private static string Generating_Abstract_Class_Message() =>
@"Cannot generate an instance of the abstract class AbstractPerson.

Possible solution:
• Register one or more concrete subtype(s): Configr<AbstractPerson>.AsOneOf(...)
";
}
