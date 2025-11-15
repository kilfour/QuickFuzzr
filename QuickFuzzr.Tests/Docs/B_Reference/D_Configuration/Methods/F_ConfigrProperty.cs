using System.Reflection;
using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Property")]
[DocColumn(Configuring.Columns.Description, "Applies a custom Fuzzr or value to all matching properties across all types.")]
[DocContent("Any property matching the predicate will use the specified Fuzzr during generation.")]
[DocSignature("Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)")]
public class F_ConfigrProperty
{
    [DocUsage]
    [DocExample(typeof(F_ConfigrProperty), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
    }

    [Fact]
    public void IsApplied()
    {
        var fuzzr =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [Fact]
    [DocOverloads]
    [DocOverload("Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)")]
    [DocContent("  Allows you to pass in a value instead of a Fuzzr.")]
    [CodeSnippet]
    public void IsApplied_Constant()
    {
        var fuzzr =
           from _ in Configr.Property(a => a.Name == "Id", 42)
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [Fact]
    [DocOverload("Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, FuzzrOf<TProperty>> factory)")]
    [DocContent("  Allows you to create a Fuzzr dynamically using a factory method.")]
    public void IsApplied_Factory()
    {
        var fuzzr =
           from _ in Configr.Property(a => a.Name == "Id", a => Fuzzr.Constant(42))
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [Fact]
    [DocOverload("Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, TProperty> factory)")]
    [DocContent("  Allows you to create a value dynamically using a factory method.")]
    public void IsApplied_Factory_Constant()
    {
        var fuzzr =
           from _ in Configr.Property(a => a.Name == "Id", a => 42)
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [Fact]
    public void Multiple()
    {
        var fuzzr =
            from n1 in Configr.Property(a => a.Name == "Name", "One")
            from n2 in Configr.Property(a => a.Name == "Name", "Two")
            from age in Configr.Property(a => a.Name == "Age", Fuzzr.Counter("age"))
            from person in Fuzzr.One<Person>()
            select person;
        var result = fuzzr.Many(2).Generate().ToArray();
        Assert.Equal("Two", result[0].Name);
        Assert.Equal(1, result[0].Age);
        Assert.Equal("Two", result[1].Name);
        Assert.Equal(2, result[1].Age);
    }

    [Fact]
    public void Overriding()
    {
        var fuzzr =
            from n1 in Configr.Property(a => a.Name == "Name", "One")
            from p1 in Fuzzr.One<Person>()
            from n2 in Configr.Property(a => a.Name == "Name", "Two")
            from p2 in Fuzzr.One<Person>()
            select (p1, p2);
        var (person1, person2) = fuzzr.Generate();
        Assert.Equal("One", person1.Name);
        Assert.Equal("Two", person2.Name);
    }

    [Fact]
    [DocExceptions]
    [DocException("ArgumentNullException", "When the predicate is `null`.")]
    public void Null_Expression()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(null!, "FIXED"));
        Assert.Equal("Value cannot be null. (Parameter 'predicate')", ex.Message);

        ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(null!, Fuzzr.Constant(42)));
        Assert.Equal("Value cannot be null. (Parameter 'predicate')", ex.Message);

        ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(null!, () => Fuzzr.Constant(42)));
        Assert.Equal("Value cannot be null. (Parameter 'predicate')", ex.Message);

        ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(null!, () => 42));
        Assert.Equal("Value cannot be null. (Parameter 'predicate')", ex.Message);
    }

    [Fact]
    [DocException("ArgumentNullException", "When the Fuzzr is `null`.")]
    public void Null_Fuzzr()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(a => a.Name == "Name", (FuzzrOf<int>)null!));
        Assert.Equal("Value cannot be null. (Parameter 'fuzzr')", ex.Message);
    }

    [Fact]
    [DocException("ArgumentNullException", "When the factory function is `null`.")]
    public void Null_Factroy()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(a => a.Name == "Name", (Func<PropertyInfo, FuzzrOf<string>>)null!));
        Assert.Equal("Value cannot be null. (Parameter 'factory')", ex.Message);

        ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Property(a => a.Name == "Name", (Func<PropertyInfo, string>)null!));
        Assert.Equal("Value cannot be null. (Parameter 'factory')", ex.Message);
    }
}