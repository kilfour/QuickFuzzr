# The Final Showcase
So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc., 
how *exactly* do we handle that ?  

This example uses the [HorsesForCourses](https://github.com/kilfour/HorsesForCourses) example domain.  

**Fuzzrs:**  
```csharp
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
        from coaches in CoachFuzzr.Many(3)
        from courses in CourseFuzzr(coaches).Many(1)
        select (courses, coaches);
}
```
**Output:**  
```text
(
    [
        {
            Name: "Advanced C# Patterns",
            Period: {
                Start: 26.November(2025),
                End: 7.January(2026)
            },
            TimeSlots: [
                { Day: Friday, Start: 11, End: 15 }
            ],
            RequiredSkills: [
                "C#"
            ],
            IsConfirmed: true,
            AssignedCoach: {
                Name: "Earl Owens",
                Email: "earl.owens@hotmail.org",
                Skills: [
                    "JavaScript",
                    "UnitTesting",
                    "C#"
                ],
                AssignedCourses: [
                    <cycle => Course.Id: 1>
                ],
                Id: 1,
                IsTransient: false
            },
            Id: 1,
            IsTransient: false
        }
    ],
    [
        {
            Name: "Earl Owens",
            Email: "earl.owens@hotmail.org",
            Skills: [
                "JavaScript",
                "UnitTesting",
                "C#"
            ],
            AssignedCourses: [
                {
                    Name: "Advanced C# Patterns",
                    Period: {
                        Start: 26.November(2025),
                        End: 7.January(2026)
                    },
                    TimeSlots: [
                        { Day: Friday, Start: 11, End: 15 }
                    ],
                    RequiredSkills: [
                        "C#"
                    ],
                    IsConfirmed: true,
                    AssignedCoach: <cycle => Coach.Id: 1>,
                    Id: 1,
                    IsTransient: false
                }
            ],
            Id: 1,
            IsTransient: false
        },
        {
            Name: "Leon Mccoy",
            Email: "leon.mccoy@hotmail.io",
            Skills: [
                "ASP.NET",
                "UnitTesting",
                "TDD"
            ],
            AssignedCourses: [ ],
            Id: 2,
            IsTransient: false
        },
        {
            Name: "Emily Figueroa",
            Email: "emily.figueroa@outlook.com",
            Skills: [
                "React",
                "UnitTesting",
                "TDD",
                "CI/CD"
            ],
            AssignedCourses: [ ],
            Id: 3,
            IsTransient: false
        }
    ]
)
```
