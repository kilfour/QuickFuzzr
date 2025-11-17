using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>.EndOn")]
[DocColumn(Configuring.Columns.Description, "Replaces deeper recursion with the specified end type.")]
[DocContent(
@"Configures a recursion stop condition for type `T`,
instructing QuickFuzzr to generate `TEnd` instances instead of continuing deeper.")]
[DocSignature("Configr<T>.EndOn<TEnd>()")]
[DocContent(
@"")]
public class I_ConfigrEndOnT
{
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Turtle> GetFuzzr()
    {
        return
        from ending in Configr<Turtle>.EndOn<MoreTurtles>()
        from turtle in Fuzzr.One<Turtle>()
        select turtle;
        // Results in => 
        // MoreTurtles { Down: null }
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(I_ConfigrEndOnT), nameof(GetFuzzr))]
    public void Example()
    {
        var result = GetFuzzr().Generate(43);
        Assert.NotNull(result);
        Assert.Null(result.Down);
        Assert.IsType<MoreTurtles>(result);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Turtle> GetDepthFuzzr()
    {
        return
        from depth in Configr<Turtle>.Depth(1, 3)
        from ending in Configr<Turtle>.EndOn<MoreTurtles>()
        from turtle in Fuzzr.One<Turtle>()
        select turtle;
        // Results in => 
        // Turtle { Down: Turtle { Down: MoreTurtles { Down: null } } }
    }

    [Fact]
    [DocContent("With depth constraints, QuickFuzzr respects the specified min/max depth when applying the `EndOn<TEnd>()` rule.")]
    [DocExample(typeof(I_ConfigrEndOnT), nameof(GetDepthFuzzr))]
    public void WithDepth()
    {
        var result = GetDepthFuzzr().Generate(1);
        Assert.NotNull(result);
        Assert.NotNull(result.Down);
        Assert.NotNull(result.Down.Down);
        Assert.Null(result.Down.Down.Down);
        Assert.IsType<Turtle>(result);
        Assert.IsType<Turtle>(result.Down);
        Assert.IsType<MoreTurtles>(result.Down.Down);

        result = GetDepthFuzzr().Generate(2);
        Assert.NotNull(result);
        Assert.Null(result.Down);
        Assert.IsType<MoreTurtles>(result);
    }

    [Fact]
    [DocExceptions]
    [DocException("DerivedTypeNotAssignableException", "When `TEnd` is not assignable to `T`.")]
    public void End_Type_Is_Not_Derived()
    {
        var ex = Assert.Throws<DerivedTypeNotAssignableException>(Configr<Turtle>.EndOn<string>);
        Assert.Equal(End_Type_Is_Not_Derived_Message(), ex.Message);
    }

    private static string End_Type_Is_Not_Derived_Message() =>
@"The type String is not assignable to the base type Turtle.

Possible solutions:
- Use a compatible type in Configr<Turtle>.EndOn<String>().
- Ensure String inherits from or implements Turtle.
";

    [Fact]
    public void Configr_InChain()
    {
        var fuzzr =
            from _1 in Configr<Turtle>.EndOn<MoreTurtles>()
            from e1 in Fuzzr.One<Turtle>()
            from _2 in Configr<Turtle>.EndOn<Turtle>()
            from e2 in Fuzzr.One<Turtle>()
            select (e1, e2);
        var result = fuzzr.Generate(42);
        Assert.IsType<MoreTurtles>(result.e1);
        Assert.IsType<Turtle>(result.e2);
    }

    [Fact]
    public void Configr_DoesNotMultiply()
    {
        var fuzzr =
            from _1 in Configr<Turtle>.EndOn<MoreTurtles>()
            from i in Fuzzr.Int()
            select i;
        var state = new State();
        fuzzr.Many(2)(state);
        Assert.Single(state.Endings);
    }
}