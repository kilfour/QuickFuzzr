using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools.Models;

[CodeExample]
public class PublicAgenda
{
    public List<Appointment> Appointments { get; set; } = [];
}

[CodeExample]
public class Agenda
{
    private readonly List<Appointment> appointments = [];
    public IReadOnlyList<Appointment> Appointments => appointments;
    public void Add(Appointment appointment) => appointments.Add(appointment);
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
