using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileCodeHeader("Configr<T>.Property")]
[DocColumn(Configuring.Columns.Description, "Sets a custom Fuzzr or value for one property on type T.")]
[DocContent("The property specified will be generated using the passed in Fuzzr.")]
[DocSignature("Configr<T>.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)")]
public class E_ConfigrPropertyT
{
	[CodeSnippet]
	[CodeRemove("return")]
	private static FuzzrOf<Intent> GetConfig()
	{
		return Configr<Person>.Property(s => s.Age, Fuzzr.Constant(42));
	}

	[Fact]
	[DocUsage]
	[DocExample(typeof(E_ConfigrPropertyT), nameof(GetConfig))]
	public void Example()
	{
		var fuzzr =
			from c in GetConfig()
			from r in Fuzzr.One<Person>()
			select r;
		Assert.Equal(42, fuzzr.Generate().Age);
	}

	[Fact]
	[DocContent("- Derived classes generated also use the custom property.")]
	public void WorksForDerived()
	{
		var fuzzr =
			from _ in Configr<Person>.Property(s => s.Age, 42)
			from result in Fuzzr.One<Employee>()
			select result;
		Assert.Equal(42, fuzzr.Generate().Age);
	}

	[Fact]
	public void ComplexDerived()
	{
		var fuzzr =
			from _1 in Configr<Person>.Property(s => s.Age, 42)
			from _2 in Configr<Employee>.Property(s => s.Age, 43)
			from p in Fuzzr.One<Person>()
			from e in Fuzzr.One<Employee>()
			from he in Fuzzr.One<HousedEmployee>()
			select (p, e, he);
		var (person, employee, housedEmployee) = fuzzr.Generate();
		Assert.Equal(42, person.Age);
		Assert.Equal(43, employee.Age);
		Assert.Equal(43, housedEmployee.Age);
	}

	[Fact]
	[DocOverloads]
	[DocOverload("Configr<T>.Property<TProperty>(Func<PropertyInfo, bool> predicate, TProperty value)")]
	[DocContent("  Allows for passing a value instead of a Fuzzr.")]
	public void UsingValue()
	{
		var fuzzr =
			from c in Configr<Person>.Property(s => s.Age, 666)
			from r in Fuzzr.One<Person>()
			select r;
		Assert.Equal(666, fuzzr.Generate().Age);
	}


	[Fact]
	[DocExceptions]
	[DocException("ArgumentNullException", "When the expression is `null`.")]
	public void Null_Expression()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<PersonOutInTheFields>.Property(null!, "FIXED"));
		Assert.Equal("Value cannot be null. (Parameter 'expression')", ex.Message);

		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<PersonOutInTheFields>.Property<int>(null!, Fuzzr.Constant(42)));
		Assert.Equal("Value cannot be null. (Parameter 'expression')", ex.Message);
	}

	[Fact]
	[DocException("ArgumentNullException", "When the Fuzzr is `null`.")]
	public void Null_Fuzzr()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<PersonOutInTheFields>.Property(a => a.Age, null!));
		Assert.Equal("Value cannot be null. (Parameter 'fuzzr')", ex.Message);
	}

	[Fact]
	[DocException("PropertyConfigurationException", "When the expression points to a field instead of a property.")]
	public void Expression_Points_To_A_Field()
	{
		var ex = Assert.Throws<PropertyConfigurationException>(() => Configr<PersonOutInTheFields>.Property(a => a.Name, "FIXED"));
		Assert.Equal(Expression_Points_To_A_Field_Message(), ex.Message);
	}

	private static string Expression_Points_To_A_Field_Message() =>
@"Cannot configure expression 'a => a.Name'.

It does not refer to a property.
Fields and methods are not supported by default.

Possible solutions:
- Use a property selector (e.g. a => a.PropertyName).
- Then pass it to Configr<PersonOutInTheFields>.Property(...) to configure generation.
"; // - If you intended to configure a field, enable field access or use explicit construction.

}