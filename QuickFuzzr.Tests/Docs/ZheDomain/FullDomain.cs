using HorsesForCourses.Domain.Coaches;
using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.TimeSlots;
using QuickPulse.Instruments;
using QuickPulse.Show;
using HorsesForCourses.Domain.Skills;
using System.Collections.ObjectModel;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.ZheDomain;

//[DocFile]
public class FullDomain
{

    [Fact(Skip = "explicit")]
    [DocContent(
@"So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc., 
how *exactly* do we handle that ?  

**Fuzzrs:**")]
    [DocCodeFile("FuzzThe.cs", "csharp", 12)]
    [DocContent("**Result:**")]
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