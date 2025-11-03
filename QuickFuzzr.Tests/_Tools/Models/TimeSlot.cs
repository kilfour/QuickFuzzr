using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools.Models;

[CodeExample]
public class Agenda
{
    public List<Appointment> Appointments { get; set; } = [];
}


[CodeExample]
public class Appointment
{
    public TimeSlot TimeSlot { get; set; } = default!;
}

[CodeExample]
public class TimeSlot
{
    public DayOfWeek Day { get; set; }
    public int Time { get; set; }
}
