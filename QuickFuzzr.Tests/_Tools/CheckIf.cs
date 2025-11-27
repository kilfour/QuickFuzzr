using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickPulse;

namespace QuickFuzzr.Tests._Tools;

public static class CheckIf
{
    public static (string, Func<T, bool>) Is<T>(T expected) =>
        (expected?.ToString() ?? "null", x => EqualityComparer<T>.Default.Equals(x, expected));

    [StackTraceHidden]
    public static void TheseValuesAreGenerated<T>(FuzzrOf<T> fuzzr, params T[] needsToBeSeen)
    {
        GeneratedValuesShouldEventuallySatisfyAll(fuzzr, [.. needsToBeSeen.Select(Is)]);
    }

    [StackTraceHidden]
    public static void GeneratesNullAndNotNull<T>(FuzzrOf<T> fuzzr)
        => GeneratedValuesShouldEventuallySatisfyAll(fuzzr,
            ("is null", a => a is null),
            ("is not null", a => a is not null));

    [StackTraceHidden]
    public static void GeneratedValuesShouldEventuallySatisfyAll<T>(
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
            => GeneratedValuesShouldEventuallySatisfyAll(100, fuzzr, labeledChecks);

    [StackTraceHidden]
    public static void GeneratedValuesShouldEventuallySatisfyAll<T>(
        int numberOfExecutions,
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        var signal = Signal.From<T>(a => Pulse.Trace(a!));
        var check =
            from inspector in Trackr.Stashed(
                () => signal.SetAndReturnArtery(new DistinctValueInspector<T>()))
            from input in Checkr.Input("Fuzzr", fuzzr)
            from inspect in Checkr.Act("Inspect", () => signal.Pulse(input))
            from _e in Trackr.ProvenWhen("early exit",
                () => inspector.SeenSatisfyEach([.. labeledChecks.Select(a => a.Item2)]))
            from _s in Trackr.Assay("Assayer",
                [.. labeledChecks.Select(a => (a.Item1, (Func<bool>)(() => inspector.HasValueThatSatisfies(a.Item2))))])
            select Case.Closed;
        check.Run(numberOfExecutions.ExecutionsPerRun());
    }

    [StackTraceHidden]
    public static void GeneratedValuesShouldAllSatisfy<T>(
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        GeneratedValuesShouldAllSatisfy(20, fuzzr, labeledChecks);
    }

    [StackTraceHidden]
    public static void GeneratedValuesShouldAllSatisfy<T>(
        int numberOfExecutions,
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        var check =
            from input in Checkr.Input("Fuzzr", fuzzr)
            from t in Checkr.Trace("input", () => input.ToString()!)
            from _ in CombineSpecs(input, labeledChecks) // Move this to QuickCheckr maybe
            select Case.Closed;
        check.Run(numberOfExecutions.ExecutionsPerRun());
    }

    private static CheckrOf<Case> CombineSpecs<T>(T input, IEnumerable<(string, Func<T, bool>)> checks)
    {
        return checks
            .Select(c => Checkr.Spec(c.Item1, () => c.Item2(input)))
            .Aggregate(Acc, (acc, next) => from _ in acc from __ in next select Case.Closed);
    }

    private static readonly CheckrOf<Case> Acc =
        s => CheckrResult.CaseOnly(s);
}