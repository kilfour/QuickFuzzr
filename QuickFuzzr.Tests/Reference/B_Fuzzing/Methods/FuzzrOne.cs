using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
public class FuzzrOne
{

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
