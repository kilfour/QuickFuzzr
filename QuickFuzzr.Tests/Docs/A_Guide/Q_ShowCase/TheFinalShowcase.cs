using HorsesForCourses.Domain.Coaches;
using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.TimeSlots;
using QuickPulse.Instruments;
using QuickPulse.Show;
using HorsesForCourses.Domain.Skills;
using System.Collections.ObjectModel;
using QuickPulse.Explains;
using QuickFuzzr.Tests._Tools.DataLists;
using HorsesForCourses.Abstractions;
using WibblyWobbly;
using HorsesForCourses.Domain.Accounts;
using System.Security.Claims;
using QuickFuzzr.Tests._Tools;

namespace QuickFuzzr.Tests.Docs.A_Guide.Q_ShowCase;

[DocFile]
public class TheFinalShowcase
{
    private readonly bool WriteToFile = false;

    [DocContent(
@"So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc.,
how *exactly* do we handle that in practice?

This example uses the [HorsesForCourses](https://github.com/kilfour/HorsesForCourses) example domain.  
*Note:* `Admin` is a system user authorized for mutations, defined elsewhere in the domain model.")]
    [DocHeader("Building Blocks", 1)]
    [DocContent(
@"**Generic Identity**
- Generate a stable, per-type increasing id.")]
    [DocExample(typeof(TheFinalShowcase), nameof(GenericId))]
    [DocContent(
@"**Name and Email**
- Build a consistent Info(name, email) pair.
- Depends on external *DataLists*.")]
    [DocExample(typeof(TheFinalShowcase), nameof(InfoFuzzr))]
    [DocContent(
@"**Coach**
- Generates a complete `Coach` aggregate, linking skills and personal info.
- Requires the GenericId and InfoFuzzr from earlier and a `string[]` of skills.")]
    [DocExample(typeof(TheFinalShowcase), nameof(CoachFuzzr))]
    [DocContent(
@"**Period**
- Generates a valid course period within 2025.")]
    [DocExample(typeof(TheFinalShowcase), nameof(PeriodFuzzr))]
    [DocContent(
@"**Timeslot**
- Produce a single (day, start, end) slot with sane hours.")]
    [DocExample(typeof(TheFinalShowcase), nameof(TimeslotFuzzrFor))]
    [DocContent(
@"**Course**
- Construct a Course and bring it to a confirmed, assignable state.
- Requires the PeriodFuzzr and TimeslotFuzzrFor, the `string[]` of skills and a pool of coaches so one can potentially be assigned.")]
    [DocExample(typeof(TheFinalShowcase), nameof(CourseFuzzr))]
    [DocContent("**All Together Now**")]
    [DocExample(typeof(TheFinalShowcase), nameof(Domain))]
    [DocOutput]
    [DocCodeFile("Result.txt", "text")]
    [DocHeader("Looking Back", 1)]
    [DocContent(
@"This showcase demonstrates how QuickFuzzr can:
- Generate fully interconnected domain aggregates with bidirectional relationships
- Respect domain rules and business logic
- Handle identity generation and uniqueness constraints  
- Compose complex objects from simple building blocks
- Work with real-world domain patterns (entities, value objects, aggregates)")]
    public void BuildDomain()
    {
        var logFile = "horses-for-courses.log";
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));
        var result = Domain(CoachFuzzr()).Generate(42);
        if (WriteToFile)
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

    private static readonly Actor Admin = Actor.From([new(ClaimTypes.Role, ApplicationUser.AdminRole)]);

    private static readonly string[] SkillPool =
    {
        "TDD", "Refactoring", "C#", "ASP.NET", "EF Core", "SQL",
        "DomainDrivenDesign", "UnitTesting", "Git", "CI/CD",
        "JavaScript", "React", "Elm", "Architecture"
    };

    private static readonly string[] CourseTitles =
    {
        "Intro to C#", "Advanced C# Patterns", "WebAPI & REST",
        "EF Core In Depth", "Domain Modelling", "Testing Workshop",
        "Frontend for Backenders", "CI/CD Fundamentals"
    };

    [CodeExample]
    private static FuzzrOf<int> GenericId<T>()
        where T : DomainEntity<T>
    {
        return
            from id in Fuzzr.Counter($"{typeof(T).Name.ToLower()}-id")
            from _ in Configr<T>.Property(a => a.Id, Id<T>.From(id))
            select id;
    }

    private record Info(string Name, string Email);

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Info> InfoFuzzr()
    {
        return
        // Name
        from isMale in Fuzzr.Bool()
        let firstNames = isMale ? DataLists.MaleFirstNames : DataLists.FemaleFirstNames
        from firstName in Fuzzr.OneOf(firstNames)
        from lastName in Fuzzr.OneOf(DataLists.LastNames)
        let name = $"{firstName} {lastName}"
        // Email
        from emailProvider in Fuzzr.OneOf(DataLists.EmailProviders)
        from domain in Fuzzr.OneOf(DataLists.TopLevelDomains)
        let email = $"{firstName}.{lastName}@{emailProvider}.{domain}".ToLower()
        select new Info(name, email);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Coach> CoachFuzzr()
    {
        return
        from coachId in GenericId<Coach>()
        from info in InfoFuzzr()
        from skills in Fuzzr.OneOf(SkillPool).Unique(coachId).Many(3, 5)
        from coach in Fuzzr.One(() => Coach.Create(Admin, info.Name, info.Email))
            .Apply(a => a.UpdateSkills(Admin, skills))
        select coach;
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(DateOnly Start, DateOnly End)> PeriodFuzzr()
    {
        return
        from start in Fuzzr.DateOnly(1.January(2025), 31.December(2025))
        from length in Fuzzr.Int(0, 120)
        from end in Fuzzr.DateOnly(start, start.AddDays(length))
        select (start, end);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(CourseDay Day, int Start, int End)> TimeslotFuzzrFor(int key)
    {
        return
        from start in Fuzzr.Int(9, 17)
        from end in Fuzzr.Int(start + 1, 18)
        from day in Fuzzr.Enum<CourseDay>().Unique($"day-{key}")
        select (day, start, end);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Course> CourseFuzzr(IEnumerable<Coach> coaches)
    {
        return
        from courseId in GenericId<Course>()
            // Basic Info
        from courseTitle in Fuzzr.OneOf(CourseTitles)
        from period in PeriodFuzzr()
            // Course Construction
        from course in Fuzzr.One(() => Course.Create(Admin, courseTitle, period.Start, period.End))
            // Values for Methods 
        from requiredSkills in Fuzzr.OneOf(SkillPool).Many(1)
        from timeslots in TimeslotFuzzrFor(courseId).Many(1, 3)
            // Calling Entity Methods
        let _1 = course.UpdateRequiredSkills(Admin, requiredSkills)
        let _2 = course.UpdateTimeSlots(Admin, [.. timeslots], a => a)
        let _3 = course.Confirm(Admin)
        // look for coaches that can be assigned to the course
        //   - WithDefault results in <null> if the collection is empty
        let eligibleCoaches = coaches.Where(a => a.IsSuitableFor(course) && a.IsAvailableFor(course))
        from coachToAssign in Fuzzr.OneOf(eligibleCoaches).WithDefault()
            // Assign a Coach if possible
        select coachToAssign == null ? course : course.AssignCoach(Admin, coachToAssign);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Course, IEnumerable<Coach>)> Domain(FuzzrOf<Coach> coachFuzzr)
    {
        return
        from coaches in coachFuzzr.Many(3)
        from course in CourseFuzzr(coaches)
        select (course, coaches);
    }

    public static (IEnumerable<Course>, IEnumerable<Coach>) Lots()
    {
        return
        (from coaches in CoachFuzzr().Many(250)
         from courses in CourseFuzzr(coaches).Many(50)
         select (courses, coaches)).Generate(42);
    }
}