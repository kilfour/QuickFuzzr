using HorsesForCourses.Domain.Coaches;
using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.TimeSlots;
using QuickPulse.Instruments;
using QuickPulse.Show;
using HorsesForCourses.Domain.Skills;
using System.Collections.ObjectModel;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Q_ShowCase;

[DocFile]
public class TheFinalShowcase
{

    [Fact(Skip = "explicit")]
    [DocContent(
@"So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc., 
how *exactly* do we handle that ?  

This example uses the [HorsesForCourses](https://github.com/kilfour/HorsesForCourses) example domain.  

**Fuzzrs:**")]
    [DocCodeFile("FuzzThe.cs", "csharp", 12)]
    [DocContent("**Output:**")]
    [DocCodeFile("Result.txt", "text")]
    public void BuildDomain()
    {
        var logFile = "horses-for-courses.log";
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));
        var result = FuzzThe.Domain.Generate(42);
        Please.AllowMe()
            .ToSubstituteWithPropertyNamed<string>("Value")
            .ToSubstituteWithPropertyNamed<int>("Value")
            .ToSelfReference<Coach>(a => $"<cycle => Coach.Id: {a.Id.Value}>")
            .ToSelfReference<Course>(a => $"<cycle => Course.Id: {a.Id.Value}>")
            .ToInline<ReadOnlyCollection<Skill>>()
            .ToInline<TimeSlot>()
            .ToUseNonLinearTime()
            .IntroduceThis(result)
            .PulseToLog(logFile);
    }
}