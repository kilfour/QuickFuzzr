using System.Security.Claims;
using HorsesForCourses.Abstractions;
using HorsesForCourses.Domain.Accounts;
using HorsesForCourses.Domain.Coaches;
using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.TimeSlots;
using WibblyWobbly;
using QuickFuzzr.Tests._Tools.DataLists;
using QuickFuzzr;

namespace QuickFuzzr.Tests.Docs.Q_ShowCase;

public class FuzzThe
{
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

    private static FuzzrOf<int> GenericId<T>()
        where T : DomainEntity<T> =>
        from id in Fuzzr.Counter($"{typeof(T).Name.ToLower()}-id")
        from _ in Configr<T>.Property(a => a.Id, Id<T>.From(id))
        select id;

    private record Info(string Name, string Email);

    private static readonly FuzzrOf<Info> InfoFuzzr =
        from isMale in Fuzzr.Bool()
        let firstNames = isMale ? DataLists.MaleFirstNames : DataLists.FemaleFirstNames
        from firstName in Fuzzr.OneOf(DataLists.FirstNames)
        from lastName in Fuzzr.OneOf(DataLists.LastNames)
        from emailProvider in Fuzzr.OneOf(DataLists.EmailProviders)
        from domain in Fuzzr.OneOf(DataLists.TopLevelDomains)
        select new Info($"{firstName} {lastName}", $"{firstName}.{lastName}@{emailProvider}.{domain}".ToLower());

    private static readonly FuzzrOf<Coach> CoachFuzzr =
        from coachId in GenericId<Coach>()
        from info in InfoFuzzr
        from skills in Fuzzr.OneOf(SkillPool).Unique(coachId).Many(3, 5)
        from coach in Fuzzr.One(() => Coach.Create(Admin, info.Name, info.Email))
            .Apply(a => a.UpdateSkills(Admin, skills))
        select coach;

    private static readonly FuzzrOf<(DateOnly Start, DateOnly End)> PeriodFuzzr =
        from start in Fuzzr.DateOnly(1.January(2025), 31.December(2025))
        from length in Fuzzr.Int(0, 120)
        from end in Fuzzr.DateOnly(start, start.AddDays(length))
        select (start, end);

    private static FuzzrOf<Course> CourseFuzzr(IEnumerable<Coach> coaches) =>
        from courseId in GenericId<Course>()
        from courseTitle in Fuzzr.OneOf(CourseTitles)
        from period in PeriodFuzzr
        from course in Fuzzr.One(() => Course.Create(Admin, courseTitle, period.Start, period.End))
        from requiredSkills in Fuzzr.OneOf(SkillPool).Many(1)
        let _1 = course.UpdateRequiredSkills(Admin, requiredSkills)
        from timeslots in TimeslotFuzzrFor(courseId).Many(1, 3)
        let _2 = course.UpdateTimeSlots(Admin, [.. timeslots], a => a)
        let _3 = course.Confirm(Admin)
        from coachToAssign in Fuzzr.OneOfOrDefault(
            coaches.Where(a => a.IsSuitableFor(course) && a.IsAvailableFor(course)))
        select coachToAssign == null ? course : course.AssignCoach(Admin, coachToAssign);

    private static FuzzrOf<(CourseDay Day, int Start, int End)> TimeslotFuzzrFor(int key) =>
        from start in Fuzzr.Int(9, 17)
        from end in Fuzzr.Int(start + 1, 18)
        from day in Fuzzr.Enum<CourseDay>().Unique($"day-{key}")
        select (day, start, end);

    public static readonly FuzzrOf<(IEnumerable<Course> courses, IEnumerable<Coach> coaches)> Domain =
            from _ in Configr.IgnoreAll()
            from coaches in CoachFuzzr.Many(3)
            from courses in CourseFuzzr(coaches).Many(1)
            select (courses, coaches);
}