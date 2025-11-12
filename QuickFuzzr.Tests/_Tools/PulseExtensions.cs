using System.Runtime.CompilerServices;
using QuickPulse;
using QuickPulse.Arteries;
using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests._Tools;

public static class PulseExtensions
{
    public static T PulseToQuickLog<T>(
        this T item,
        [CallerMemberName] string testName = "",
        [CallerFilePath] string callerPath = "")
    {
        var dir = Path.GetDirectoryName(callerPath)!;
        var fullPath = Path.Combine(dir, $"{testName}.log");
        Signal.From<string>(a => Pulse.Trace(a))
           .SetArtery(FileLog.Write(fullPath))
           .Pulse(Please.AllowMe().ToAddSomeClass().IntroduceThis(item!, false));
        return item;
    }
}

public abstract class ExplainMe<T>
{
    [Fact]
    public void ExplainOnlyThis()
    {
        Explain.OnlyThis<T>("temp.md");
    }
}