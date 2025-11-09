using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr&lt;T&gt;.Property(...)")]
public class ConfigrTProperty
{
	[DocContent("**Usage:**")]
	[DocExample(typeof(ConfigrTProperty), nameof(GetConfig))]
	[CodeSnippet]
	[CodeRemove("return")]
	private static FuzzrOf<Intent> GetConfig()
	{
		return Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
	}

	[Fact]
	[DocContent("- The property specified will be generated using the passed in generator.")]
	public void StaysDefaultValue()
	{
		var generator =
			from c in GetConfig()
			from r in Fuzzr.One<Thing>()
			select r;
		Assert.Equal(42, generator.Generate().Id);
	}

	[CodeSnippet]
	[CodeRemove("return")]
	private static FuzzrOf<Intent> GetValueConfig()
	{
		return Configr<Thing>.Property(s => s.Id, 666);
	}

	[Fact]
	[DocContent("- An overload exists which allows for passing a value instead of a generator.")]
	[DocExample(typeof(ConfigrTProperty), nameof(GetValueConfig))]
	public void UsingValue()
	{
		var generator =
			from c in GetValueConfig()
			from r in Fuzzr.One<Thing>()
			select r;
		Assert.Equal(666, generator.Generate().Id);
	}

	[Fact]
	[DocContent("- Derived classes generated also use the custom property.")]
	public void WorksForDerived()
	{
		var generator =
			from _ in Configr<Thing>.Property(s => s.Id, 42)
			from result in Fuzzr.One<DerivedThing>()
			select result;
		Assert.Equal(42, generator.Generate().Id);
	}

	[Fact]
	[DocContent("- Trying to configure a field throws an exception with the following message:")]
	[DocExample(typeof(ConfigrTProperty), nameof(Expression_Points_To_A_Field_Message), "text")]
	public void Expression_Points_To_A_Field()
	{
		var fuzzr =
			from cfg in Configr<PersonOutInTheFields>.Property(a => a.Name, "FIXED")
			from person in Fuzzr.One<PersonOutInTheFields>()
			select person;
		var ex = Assert.Throws<PropertyConfigurationException>(() => fuzzr.Generate());
		Assert.Equal(Expression_Points_To_A_Field_Message(), ex.Message);
	}

	[CodeSnippet]
	[CodeRemove("@\"")]
	[CodeRemove("\";")]
	private static string Expression_Points_To_A_Field_Message() =>
@"Cannot configure expression 'a => a.Name'.

It does not refer to a property.
Fields and methods are not supported by default.

Possible solutions:
• Use a property selector (e.g. a => a.PropertyName).
• Then pass it to Configr<PersonOutInTheFields>.Property(...) to configure generation.
"; // • If you intended to configure a field, enable field access or use explicit construction.
}