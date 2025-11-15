using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Ignore")]
[DocColumn(Configuring.Columns.Description, "Globally ignores all properties matching the predicate.")]
[DocContent(
@"Skips all properties matching the predicate across all types during generation.  
Use to exclude recurring patterns like identifiers, foreign keys, or audit fields.")]
[DocSignature("Configr.Ignore(Func<PropertyInfo, bool> predicate)")]
public class B_ConfigrIgnore
{

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Person, FileEntry)> GetFuzzr()
    {
        return
        from ignore in Configr.Ignore(a => a.Name == "Name")
        from person in Fuzzr.One<Person>()
        from fileEntry in Fuzzr.One<FileEntry>()
        select (person, fileEntry);
        // Results in => 
        // ( Person { Name: "", Age: 67 }, FileEntry { Name: "" } )
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(B_ConfigrIgnore), nameof(GetFuzzr))]
    public void StaysDefaultValue()
    {
        var (person, fileEntry) = GetFuzzr().Generate(42);
        Assert.Equal(string.Empty, person.Name);
        Assert.Equal(67, person.Age);
        Assert.Equal(string.Empty, fileEntry.Name);
    }

    [Fact]
    [DocExceptions]
    [DocContent("  - `ArgumentNullException`: When the expression is `null`.")]
    public void Null()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => Configr.Ignore(null!));
        Assert.Equal("Value cannot be null. (Parameter 'predicate')", ex.Message);
    }
}
