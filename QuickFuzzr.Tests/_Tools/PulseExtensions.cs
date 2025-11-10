using System.Runtime.CompilerServices;
using QuickPulse;
using QuickPulse.Arteries;
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
           .Pulse(Introduce.This(item!));
        return item;
    }
}