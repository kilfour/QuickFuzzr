# The Final Showcase
So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc.,
how *exactly* do we handle that in practice?

This example uses the [HorsesForCourses](https://github.com/kilfour/HorsesForCourses) example domain.  
*Note:* `Admin` is a system user authorized for mutations, defined elsewhere in the domain model.  
### Building Blocks
**Generic Identity**
- Generate a stable, per-type increasing id.  
```csharp
private static FuzzrOf<int> GenericId<T>()
    where T : DomainEntity<T>
{
    return
    from id in Fuzzr.Counter($"{typeof(T).Name.ToLower()}-id")
    from _ in Configr<T>.Property(a => a.Id, Id<T>.From(id))
    select id;
}
```
**Name and Email**
- Build a consistent Info(name, email) pair.
- Depends on external *DataLists*.  
```csharp
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
```
**Coach**
- Generates a complete `Coach` aggregate, linking skills and personal info.
- Requires the GenericId and InfoFuzzr from earlier and a `string[]` of skills.  
```csharp
from coachId in GenericId<Coach>()
from info in InfoFuzzr()
from skills in Fuzzr.OneOf(SkillPool).Unique(coachId).Many(3, 5)
from coach in Fuzzr.One(() => Coach.Create(Admin, info.Name, info.Email))
    .Apply(a => a.UpdateSkills(Admin, skills))
select coach;
```
**Period**
- Generates a valid course period within 2025.  
```csharp
from start in Fuzzr.DateOnly(1.January(2025), 31.December(2025))
from length in Fuzzr.Int(0, 120)
from end in Fuzzr.DateOnly(start, start.AddDays(length))
select (start, end);
```
**Timeslot**
- Produce a single (day, start, end) slot with sane hours.  
```csharp
from start in Fuzzr.Int(9, 17)
from end in Fuzzr.Int(start + 1, 18)
from day in Fuzzr.Enum<CourseDay>().Unique($"day-{key}")
select (day, start, end);
```
**Course**
- Construct a Course and bring it to a confirmed, assignable state.
- Requires the PeriodFuzzr and TimeslotFuzzrFor, the `string[]` of skills and a pool of coaches so one can potentially be assigned.  
```csharp
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
from coachToAssign in Fuzzr.OneOfOrDefault(
    coaches.Where(a => a.IsSuitableFor(course) && a.IsAvailableFor(course)))
    // Assign a Coach if possible
select coachToAssign == null ? course : course.AssignCoach(Admin, coachToAssign);
```
**All Together Now**  
```csharp
from coaches in coachFuzzr.Many(3)
from course in CourseFuzzr(coaches)
select (course, coaches);
```
**Output:**  
```text
(
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
    },
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
### Looking Back
This showcase demonstrates how QuickFuzzr can:
- Generate fully interconnected domain aggregates with bidirectional relationships
- Respect domain rules and business logic
- Handle identity generation and uniqueness constraints  
- Compose complex objects from simple building blocks
- Work with real-world domain patterns (entities, value objects, aggregates)  
