using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>AsOneOf(params Type[] types)")]
[DocContent(
@"Configures inheritance resolution for BaseType, 
allowing QuickFuzzr to randomly select one of the specified derived types when generating instances.  

Useful when generating domain hierarchies where multiple concrete subtypes exist.  
")]
public class H_ConfigrAsOneOfT
{
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
    [DocExample(typeof(H_ConfigrAsOneOfT), nameof(Generate))]
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
    [DocExceptions]
    [DocException("EmptyDerivedTypesException", "When no types are provided.")]
    public void ConfigrAsOneOf_NoDerivedTypes_Throws()
    {
        var ex = Assert.Throws<EmptyDerivedTypesException>(() => Configr<Person>.AsOneOf());
        Assert.Equal(NoDerivedTypes_Message(), ex.Message);
    }

    private static string NoDerivedTypes_Message() =>
@"No derived types were provided to AsOneOf for base type Person.

Possible solutions:
- Provide at least one derived type in Configr<Person>.AsOneOf(...).
- Ensure that the derived types array is not empty.
";

    [Fact]
    [DocException("DuplicateDerivedTypesException", "When the list of derived types contains duplicates.")]
    [DocContent("  - `DuplicateDerivedTypesException`: When the list of derived types contains duplicates.")]
    public void ConfigrAsOneOf_Duplicates_Throws()
    {
        var ex = Assert.Throws<DuplicateDerivedTypesException>(() => Configr<Person>.AsOneOf(typeof(Employee), typeof(Employee)));
        Assert.Equal(Duplicates_Message(), ex.Message);
    }

    private static string Duplicates_Message() =>
@"A duplicate derived type was provided to AsOneOf for base type Person: Employee.

Possible solutions:
- Ensure Employee only appears once in Configr<Person>.AsOneOf(...).
";

    [Fact]
    public void ConfigrAsOneOf_Duplicates_Multiple_Throws()
    {
        var ex = Assert.Throws<DuplicateDerivedTypesException>(
            () => Configr<Person>.AsOneOf(
                typeof(Employee),
                typeof(Employee),
                typeof(HousedEmployee),
                typeof(HousedEmployee)));
        Assert.Equal(Duplicates_Multiple_Message(), ex.Message);
    }

    private static string Duplicates_Multiple_Message() =>
@"Duplicate derived types were provided to AsOneOf for base type Person:
- Employee
- HousedEmployee

Possible solutions:
- Ensure each derived type in Configr<Person>.AsOneOf(...) is unique.
";

    [Fact]
    [DocException("DerivedTypeNotAssignableException", "If any listed type is not a valid subclass of `BaseType`.")]
    public void ConfigrAsOneOf_DerivedTypeNotAssignable_Throws()
    {
        var ex = Assert.Throws<DerivedTypeNotAssignableException>(
            () => Configr<Person>.AsOneOf(typeof(Person), typeof(Agenda)));
        Assert.Equal(DerivedTypeNotAssignable_Message(), ex.Message);
    }

    private static string DerivedTypeNotAssignable_Message() =>
@"The type Agenda is not assignable to the base type Person.

Possible solutions:
- Use a compatible type in Configr<Person>.AsOneOf(...).
- Ensure Agenda inherits from or implements Person.
";

    [Fact]
    public void ConfigrAsOneOf_DerivedTypeNotAssignable_Multiple_Throws()
    {
        var ex = Assert.Throws<DerivedTypeNotAssignableException>(
            () => Configr<Person>.AsOneOf(
                typeof(Person),
                typeof(string),
                typeof(Agenda)));
        Assert.Equal(DerivedTypeNotAssignable_Multiple_Message(), ex.Message);
    }

    private static string DerivedTypeNotAssignable_Multiple_Message() =>
@"The following types are not assignable to the base type Person:
- String
- Agenda

Possible solutions:
- Use compatible types in Configr<Person>.AsOneOf(...).
- Ensure all listed types inherit from or implement Person.
";

    [Fact]
    [DocException("DerivedTypeIsNullException", "If any listed type is `null`.")]
    public void ConfigrAsOneOf_DerivedType_Null_Throws()
    {
        var ex = Assert.Throws<DerivedTypeIsNullException>(() => Configr<Person>.AsOneOf(typeof(Person), null!));
        Assert.Equal(DerivedType_Null_Message(), ex.Message);
    }

    private static string DerivedType_Null_Message() =>
@"A null derived type was provided to AsOneOf for base type Person.

Possible solutions:
- Ensure that all derived types in Configr<Person>.AsOneOf(...) are non-null.
";
}