using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.T_Reference.D_Configuration.Methods;


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
	[DocContent("The property specified will be generated using the passed in generator.")]
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
	[DocContent("An overload exists which allows for passing a value instead of a generator.")]
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
	[DocContent("Derived classes generated also use the custom property.")]
	public void WorksForDerived()
	{
		var generator =
			from _ in Configr<Thing>.Property(s => s.Id, 42)
			from result in Fuzzr.One<DerivedThing>()
			select result;
		Assert.Equal(42, generator.Generate().Id);
	}
}