using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr&lt;T&gt;AsOneOf(params Type[] types)")]
[DocContent(
@"Configures inheritance resolution for BaseType, 
allowing QuickFuzzr to randomly select one of the specified derived types when generating instances.  

Useful when generating domain hierarchies where multiple concrete subtypes exist.  
")]
public class ConfigrAsOneOf
{
    [Fact]
    public void DocIt()
    {
        Explain.OnlyThis<ConfigrAsOneOf>("temp.md");
    }

    [CodeSnippet]
    [CodeRemove("42")]
    [CodeRemove("return ")]
    private static IEnumerable<Person> Generate()
    {
        var personFuzzr =
            from asOneOf in Configr<Person>.AsOneOf(typeof(Person), typeof(Employee))
            from item in Fuzzr.One<Person>()
            select item;
        return personFuzzr.Many(2).Generate(42);
        // Results in =>
        // [
        //     Employee {
        //         Email: "dn",
        //         SocialSecurityNumber: "gs",
        //         Name: "etggni",
        //         Age: 38
        //     },
        //     Person { Name: "avpkdc", Age: 70 }
        // ]
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ConfigrAsOneOf), nameof(Generate))]
    public void ConfigrAsOneOf_GetConfig_ReturnsFuzzr()
    {
        var result = Generate().ToList();
        Assert.Equal(2, result.Count);
        // Employee
        Assert.IsType<Employee>(result[0]);
        var emp = (Employee)result[0];
        Assert.Equal("dn", emp.Email);
        Assert.Equal("gs", emp.SocialSecurityNumber);
        Assert.Equal("etggni", emp.Name);
        Assert.Equal(38, emp.Age);
        // Person
        Assert.IsType<Person>(result[1]);
        var person = result[1];
        Assert.Equal("avpkdc", person.Name);
        Assert.Equal(70, person.Age);
    }

    [Fact]
    [DocContent("- Throws a `DerivedTypeNotAssignableException` if any listed type is not a valid subclass of `BaseType`.")]
    public void ConfigrAsOneOf_DerivedTypeNotAssignable_Throws()
    {
        var fuzzr =
            from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(Agenda))
            from item in Fuzzr.One<Person>()
            select item;

        var ex = Assert.Throws<DerivedTypeNotAssignableException>(() => fuzzr.Generate());
        Assert.Equal(DerivedTypeNotAssignable_Message(), ex.Message);
    }

    private static string DerivedTypeNotAssignable_Message() =>
@"The type Agenda is not assignable to the base type Person.

Possible solutions:
• Use compatible types in Configr<Person>.AsOneOf(...).
• Ensure Agenda inherits from or implements Person.
";

    [Fact]
    public void ConfigrAsOneOf_DerivedTypeNotAssignable_Multiple_Throws()
    {
        var fuzzr =
            from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(string), typeof(Agenda))
            from item in Fuzzr.One<Person>()
            select item;

        var ex = Assert.Throws<DerivedTypeNotAssignableException>(() => fuzzr.Generate());
        Assert.Equal(DerivedTypeNotAssignable_Multiple_Message(), ex.Message);
    }

    private static string DerivedTypeNotAssignable_Multiple_Message() =>
@"The following types are not assignable to the base type Person:
• String
• Agenda

Possible solutions:
• Use compatible types in Configr<Person>.AsOneOf(...).
• Ensure all listed types inherit from or implement Person.
";

    [Fact]
    [DocContent("- Throws a `DerivedTypeListEmptyException` if any listed type is not a valid subclass of `BaseType`.")]
    public void ConfigrAsOneOf_NoDerivedTypes_Throws()
    {
        var fuzzr =
            from inheritance in Configr<Person>.AsOneOf()
            from item in Fuzzr.One<Person>()
            select item;
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => fuzzr.Generate());
        //Assert.Contains("At least one derived type must be specified", ex.Message);
    }

    // [Fact]
    // public void ConfigrAsOneOf_BaseTypeIncluded_Throws()
    // {
    //     var fuzzr =
    //         from _ in Configr<Person>.AsOneOf(typeof(Person)) // base in list
    //         from item in Fuzzr.One<Person>()
    //         select item;

    //     var ex = Assert.Throws<BaseTypeIncludedInAsOneOfException>(() => fuzzr.Generate());
    //     Assert.StartsWith($"The base type {nameof(Person)} cannot be listed in AsOneOf.", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_Empty_Throws()
    // {
    //     var fuzzr =
    //         from _ in Configr<Person>.AsOneOf(Array.Empty<Type>())
    //         from item in Fuzzr.One<Person>()
    //         select item;

    //     Assert.Throws<EmptyDerivedTypesException>(() => fuzzr.Generate());
    // }

    // [Fact]
    // public void ConfigrAsOneOf_Duplicates_Throws()
    // {
    //     var fuzzr =
    //         from _ in Configr<Person>.AsOneOf(typeof(Agenda), typeof(Agenda))
    //         from item in Fuzzr.One<Person>()
    //         select item;

    //     var ex = Assert.Throws<DuplicateDerivedTypesException>(() => fuzzr.Generate());
    //     Assert.Contains(nameof(Agenda), ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_NullDerivedType_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), null)
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("Derived types cannot be null", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsAbstract_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(AbstractPerson))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is abstract and cannot be instantiated", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsInterface_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(IPerson))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is an interface and cannot be instantiated", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsGeneric_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(GenericPerson<>))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is an open generic type and cannot be instantiated", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsValueType_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(int))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is a value type and cannot be assigned to", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsSealed_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(SealedPerson))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is sealed and cannot be inherited from", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsNotClass_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(void))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is not a class type", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsNull_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), null!)
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("Derived types cannot be null", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsNotSubclass_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(object))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is not assignable to", ex.Message);
    // }

    // [Fact]
    // public void ConfigrAsOneOf_DerivedTypeIsAbstractClass_ThrowsArgumentException()
    // {
    //     var fuzzr =
    //         from inheritance in Configr<Person>.AsOneOf(typeof(Person), typeof(AbstractPerson))
    //         from item in Fuzzr.One<Person>()
    //         select item;
    //     var ex = Assert.Throws<ArgumentException>(() => fuzzr.Generate());
    //     Assert.Contains("is abstract and cannot be instantiated", ex.Message);
    // }
}