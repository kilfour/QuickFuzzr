using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.A_Guide.A_YourFirstFuzzr;


[DocFile]
public class YourFirstFuzzr
{
    [Fact]
    [DocContent("Starting simple. Suppose you have this class:")]
    [DocExample(typeof(Person))]
    [DocContent("You can generate a fully randomized instance like so:")]
    [DocExample(typeof(YourFirstFuzzr), nameof(Person_Example_GetResult))]
    [DocOutput]
    [DocCode("Person { Name = \"ddnegsn\", Age = 18 }", "text")]
    [DocContent(
@"And that's it, ... no configuration required.  
QuickFuzzr walks the `Person` type, detects its properties,
and fills them in using the default Fuzzrs.")]
    public void Person_Example()
    {
        var result = Person_Example_GetResult();
        Assert.Equal("ddnegsn", result.Name);
        Assert.Equal(18, result.Age);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("42")]
    private static Person Person_Example_GetResult()
    {
        return Fuzzr.One<Person>().Generate(42);
    }

    [Fact]
    [DocHeader("Your First Customization")]
    [DocContent(
@"QuickFuzzr is primarily designed to generate inputs for **fuzz-testing**, 
 so the default settings are mainly aimed towards that goal.  
 It is relatively easy however to *customize* the generation.  
 Let's for instance generate a *real* name and ensure our `Person` is eligible to vote:")]
    [DocExample(typeof(YourFirstFuzzr), nameof(Person_Example_GetResult_Customized))]
    [DocOutput]
    [DocCode("Person { Name = \"George Lennon\", Age = 25 }", "text")]
    public void Person_Example_Customized()
    {
        var result = Person_Example_GetResult_Customized();
        Assert.Equal("George Lennon", result.Name);
        Assert.Equal(25, result.Age);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("42")]
    private static Person Person_Example_GetResult_Customized()
    {
        var personFuzzr =
            from firstname in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
            from lastname in Fuzzr.OneOf("Lennon", "McCartney", "Harrison", "Star")
            from age in Fuzzr.Int(18, 80)
            select new Person { Name = $"{firstname} {lastname}", Age = age };
        return personFuzzr.Generate(42);
    }

    [Fact]
    [DocHeader("Composable Fuzzrs")]
    [DocContent(
@"Above example already shows how QuickFuzzr leverages the composability of `LINQ` to good effect,
but let's drive that point home a bit more.  
Consider this `Employee` class derived from `Person`:")]
    [DocExample(typeof(Employee))]
    [DocContent("Then, when defining a Fuzzr like so:\n\n")]
    [DocContent("**Social Security Number**")]
    [DocExample(typeof(YourFirstFuzzr), nameof(SsnFuzzr))]
    [DocContent("**Employee**")]
    [DocExample(typeof(YourFirstFuzzr), nameof(EmployeeFuzzr))]
    [DocOutput]
    [DocCodeFile("Employee.txt")]
    [DocContent(
@"In this example the lists used by `OneOf` are declared outside of the Fuzzr.
I just used `string[]`'s but the data could easily be loaded from a file for instance.")]
    public void Employee_Example_Customized()
    {
        var result = EmployeeFuzzr(SsnFuzzr()).Generate(43);
        Assert.Equal("John McCartney", result.Name);
        Assert.Equal(69, result.Age);
        Assert.Equal("john.mccartney@company.com", result.Email);
        Assert.Equal("761-65-2228", result.SocialSecurityNumber);
    }

    private static readonly string[] firstnames = ["John", "Paul", "George", "Ringo"];
    private static readonly string[] lastnames = ["Lennon", "McCartney", "Harrison", "Star"];
    private static readonly string[] emailProviders = ["company.com", "mailings.org", "freemail.net"];

    [CodeSnippet]
    [CodeRemove("return ssnFuzzr;")]
    public static FuzzrOf<string> SsnFuzzr()
    {
        var ssnFuzzr =
            from a in Fuzzr.Int(100, 999)
            from b in Fuzzr.Int(10, 99)
            from c in Fuzzr.Int(1000, 9999)
            select $"{a}-{b}-{c}";
        return ssnFuzzr;
    }

    [CodeSnippet]
    [CodeRemove("return employeeFuzzr;")]
    private static FuzzrOf<Employee> EmployeeFuzzr(FuzzrOf<string> ssnFuzzr)
    {
        var employeeFuzzr =
            from firstname in Fuzzr.OneOf(firstnames)
            from lastname in Fuzzr.OneOf(lastnames)
            from age in Fuzzr.Int(18, 80)
            from ssn in ssnFuzzr
            from email_provider in Fuzzr.OneOf(emailProviders)
            select
                new Employee
                {
                    Name = $"{firstname} {lastname}",
                    Age = age,
                    Email = $"{firstname}.{lastname}@{email_provider}".ToLower(),
                    SocialSecurityNumber = ssn
                };
        return employeeFuzzr;
    }

    [Fact]
    [DocHeader("Please, Sir, I Want Some More.")]
    [DocContent("For MORE!, ..., oh well then.  \nJust use:")]
    [DocCode("employeeFuzzr.Many(3).Generate()")]
    [DocOutput]
    [DocCodeFile("EmployeeMany.txt")]
    public void Employee_Example_Customized_Many()
    {
        var result = EmployeeFuzzr(SsnFuzzr()).Many(3).Generate(44).ToList();

        Assert.Equal("george.harrison@mailings.org", result[0].Email);
        Assert.Equal("953-16-1093", result[0].SocialSecurityNumber);
        Assert.Equal("George Harrison", result[0].Name);
        Assert.Equal(50, result[0].Age);

        Assert.Equal("george.mccartney@mailings.org", result[1].Email);
        Assert.Equal("736-82-8923", result[1].SocialSecurityNumber);
        Assert.Equal("George McCartney", result[1].Name);
        Assert.Equal(48, result[1].Age);

        Assert.Equal("ringo.harrison@mailings.org", result[2].Email);
        Assert.Equal("347-87-4164", result[2].SocialSecurityNumber);
        Assert.Equal("Ringo Harrison", result[2].Name);
        Assert.Equal(40, result[2].Age);
    }

    private static readonly string[] streets =
        ["High Street", "Church Road", "Station Road", "Victoria Street", "Green Lane", "Kings Road"];
    private static readonly string[] cities =
        ["London", "Manchester", "Birmingham", "Liverpool", "Leeds", "Bristol"];

    [Fact]
    [DocHeader("The Strand Union Workhouse")]
    [DocContent("Another *derivation*:")]
    [DocExample(typeof(HousedEmployee))]
    [DocContent(
@"Now we could use an `addressFuzzr` and add another line to the `select` clause of the `LINQ` expression,
but let's introduce the final piece of the QuickFuzzr puzzle: `Configr`,
and rewrite it like so:

")]
    [DocContent("**Address**")]
    [DocExample(typeof(YourFirstFuzzr), nameof(AddressConfigr))]
    [DocContent("**Info**  \n> This is a helper `record` in order to correlate name and email.")]
    [DocExample(typeof(YourFirstFuzzr), nameof(InfoFuzzr))]
    [DocContent("**Person/Employee**")]
    [DocExample(typeof(YourFirstFuzzr), nameof(PersonConfigr))]
    [DocContent("**All Together Now**")]
    [DocExample(typeof(YourFirstFuzzr), nameof(PeopleFuzzr))]
    [DocOutput]
    [DocCodeFile("HousedEmployee.txt")]
    public void HousedEmployee_Example()
    {
        var result = CombinedFuzzr().Generate(42);
        Assert.Equal("george.lennon@company.com", result.Email);
        Assert.Equal("336-74-5615", result.SocialSecurityNumber);
        Assert.Equal("George Lennon", result.Name);
        Assert.Equal(28, result.Age);
        Assert.Equal("Victoria Street", result.Address.Street);
        Assert.Equal("Manchester", result.Address.City);
    }

    public record Info(string Name, string Email);

    [CodeSnippet]
    [CodeRemove("return addressConfigr;")]
    private static FuzzrOf<Intent> AddressConfigr()
    {
        var addressConfigr =
            from street in Configr<Address>.Property(a => a.Street, Fuzzr.OneOf(streets))
            from city in Configr<Address>.Property(a => a.City, Fuzzr.OneOf(cities))
            select Intent.Fixed;
        return addressConfigr;
    }

    [CodeSnippet]
    [CodeRemove("return infoFuzzr;")]
    private static FuzzrOf<Info> InfoFuzzr()
    {
        var infoFuzzr =
            from firstname in Fuzzr.OneOf(firstnames)
            from lastname in Fuzzr.OneOf(lastnames)
            from email_provider in Fuzzr.OneOf(emailProviders)
            select new Info(
                $"{firstname} {lastname}",
                $"{firstname}.{lastname}@{email_provider}".ToLower());
        return infoFuzzr;
    }

    [CodeSnippet]
    [CodeRemove("return employeeConfigr;")]
    private static FuzzrOf<Intent> PersonConfigr(FuzzrOf<Info> infoFuzzr, FuzzrOf<string> ssnFuzzr)
    {
        var employeeConfigr =
            from _ in Configr<Person>.With(infoFuzzr, info =>
                from name in Configr<Person>.Property(a => a.Name, info.Name)
                from email in Configr<Employee>.Property(a => a.Email, info.Email)
                select Intent.Fixed)
            from age in Configr<Person>.Property(a => a.Age, Fuzzr.Int(18, 80))
            from ssn in Configr<Employee>.Property(a => a.SocialSecurityNumber, ssnFuzzr)
            select Intent.Fixed;
        return employeeConfigr;
    }

    [CodeSnippet]
    [CodeRemove("return peopleFuzzr;")]
    private static FuzzrOf<HousedEmployee> PeopleFuzzr(FuzzrOf<Intent> addressConfigr, FuzzrOf<Intent> employeeConfigr)
    {
        var peopleFuzzr =
            from addressCfg in addressConfigr
            from employeeCfg in employeeConfigr
            from housedEmployee in Fuzzr.One<HousedEmployee>()
            select housedEmployee;
        return peopleFuzzr;
    }

    private static FuzzrOf<HousedEmployee> CombinedFuzzr()
    {
        return PeopleFuzzr(AddressConfigr(), PersonConfigr(InfoFuzzr(), SsnFuzzr()));
    }

    [Fact]
    [DocContent(
@"Ok, calling this chapter *""Your First Fuzzr""*, might have been a bit optimistic.
Because that is some dense `LINQ`-ing right there.  
But here are some counter arguments.  

**1. You can always just**: Use `Fuzzr.One<HousedEmployee>()`, resulting in, for instance:")]
    [DocCode(
@"{
    Address: {
        Street: ""u"",
        City: ""ykgx""
    },
    Email: ""dratnq"",
    SocialSecurityNumber: ""ggygun"",
    Name: ""v"",
    Age: 30
}
", "text")]
    public void BackToSquareOne()
    {
        var result = Fuzzr.One<HousedEmployee>().Generate(58);
        Assert.Equal("dratnq", result.Email);
        Assert.Equal("ggygun", result.SocialSecurityNumber);
        Assert.Equal("u", result.Name);
        Assert.Equal(30, result.Age);
        Assert.Equal("v", result.Address.Street);
        Assert.Equal("ykgx", result.Address.City);
    }

    [Fact]
    [DocContent(
@"**2. Reusability**: Once you have defined a `LINQ` chain like the one above, you can do more than one thing with it.

Generate just addresses (or `Person`, `Employee`, etc.):
")]
    [DocExample(typeof(YourFirstFuzzr), nameof(Addresses_Fuzzr))]
    [DocOutput] // QuickPulse.Explains bug <= can't use records for the fragments
    [DocCodeFile("Addresses.txt")]
    public void Addresses()
    {
        var result = Addresses_Fuzzr(CombinedFuzzr()).ToList();
        Assert.Equal("High Street", result[0].Street);
        Assert.Equal("Bristol", result[0].City);
        Assert.Equal("Victoria Street", result[1].Street);
        Assert.Equal("London", result[1].City);
        Assert.Equal("Kings Road", result[2].Street);
        Assert.Equal("Liverpool", result[2].City);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("13")]
    private static IEnumerable<Address> Addresses_Fuzzr(FuzzrOf<HousedEmployee> peopleFuzzr)
    {
        var fuzzr =
            from cfg in peopleFuzzr
            from address in Fuzzr.One<Address>()
            select address;
        return fuzzr.Many(3).Generate(13);
    }

    [Fact]
    [DocContent("**3. Composability**: Or configure and reconfigure on the fly:")]
    [DocExample(typeof(YourFirstFuzzr), nameof(Wild_Fuzzr))]
    [DocOutput] // QuickPulse.Explains bug <= can't use records for the fragments
    [DocCodeFile("Mixed.txt")]
    [DocContent(
@"As you can see this reuses the previous Fuzzr, generates a *normal* `HousedEmployee`,
another one that is guaranteed to live in London, and finally an underaged one.")]
    public void Wild()
    {
        var result = Wild_Fuzzr(CombinedFuzzr());

        var normal = result.Item1;
        Assert.Equal("paul.mccartney@mailings.org", normal.Email);
        Assert.Equal("621-54-5020", normal.SocialSecurityNumber);
        Assert.Equal("Paul McCartney", normal.Name);
        Assert.Equal(27, normal.Age);
        Assert.Equal("Station Road", normal.Address.Street);
        Assert.Equal("Bristol", normal.Address.City);

        var londoner = result.Item2;
        Assert.Equal("george.harrison@freemail.net", londoner.Email);
        Assert.Equal("535-80-4278", londoner.SocialSecurityNumber);
        Assert.Equal("George Harrison", londoner.Name);
        Assert.Equal(34, londoner.Age);
        Assert.Equal("Victoria Street", londoner.Address.Street);
        Assert.Equal("London", londoner.Address.City);

        var underaged = result.Item3;
        Assert.Equal("paul.star@company.com", underaged.Email);
        Assert.Equal("428-67-7239", underaged.SocialSecurityNumber);
        Assert.Equal("Paul Star", underaged.Name);
        Assert.Equal(11, underaged.Age);
        Assert.Equal("Kings Road", underaged.Address.Street);
        Assert.Equal("Manchester", underaged.Address.City);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("68")]
    private static (HousedEmployee, HousedEmployee, HousedEmployee) Wild_Fuzzr(FuzzrOf<HousedEmployee> peopleFuzzr)
    {
        var fuzzr =
            from cfg in peopleFuzzr
                // Generate a HousedEmployee according to current configuration
            from normal in Fuzzr.One<HousedEmployee>()
                // Override city for this HousedEmployee
            from london in Configr<Address>.Property(a => a.City, "London")
            from londoner in Fuzzr.One<HousedEmployee>()
                // Restore city config and override Age
            from city in Configr<Address>.Property(a => a.City, Fuzzr.OneOf(cities))
            from age in Configr<Person>.Property(a => a.Age, Fuzzr.Int(8, 17))
            from underaged in Fuzzr.One<HousedEmployee>()
                // Return a Tuple (or, ..., a Thruple ?) of HousedEmployees
            select (normal, londoner, underaged);
        return fuzzr.Generate(68);
    }
}