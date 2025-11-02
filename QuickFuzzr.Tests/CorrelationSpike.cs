using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Instruments;
using QuickPulse.Show;

namespace QuickFuzzr.Tests;

public class CorrelationSpike
{
    private static readonly string[] firstnames = ["John", "Paul", "George", "Ringo"];
    private static readonly string[] lastnames = ["Lennon", "McCartney", "Harrison", "Star"];
    private static readonly string[] emailProviders = ["company.com", "mailings.org", "freemail.net"];

    public record Info(string Name, string Email);

    [Fact]
    public void LetsSee()
    {
        var infoFuzzr =
            from firstname in Fuzzr.OneOf(firstnames)
            from lastname in Fuzzr.OneOf(lastnames)
            from email_provider in Fuzzr.OneOf(emailProviders)
            select new Info(
                $"{firstname} {lastname}",
                $"{firstname}.{lastname}@{email_provider}");


        var employeeConfigr =
            from name in Configr<Person>.With(infoFuzzr, info =>
                from name in Configr<Person>.Property(a => a.Name, info.Name)
                from email in Configr<Employee>.Property(a => a.Email, info.Email)
                select Intent.Fixed)
            select Intent.Fixed;

        var fuzzr =
            from cfg in employeeConfigr
            from person in Fuzzr.One<Person>().Many(2)
            from employee in Fuzzr.One<Employee>()
            select (person, employee);
        var logFile = "QuickFuzzr.Tests\\CorrelationSpike.log";
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));
        fuzzr.Generate().PulseToLog(logFile);
    }
}