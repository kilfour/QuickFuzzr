using QuickAcid;
using QuickAcid.Bolts;
using QuickFuzzr;
using QuickPulse;
using StringExtensionCombinators;

namespace QuickFuzzr.Tests._Tools;

public static class CheckIf
{
    public static (string, Func<T, bool>) Is<T>(T expected) =>
        (expected?.ToString() ?? "null", x => EqualityComparer<T>.Default.Equals(x, expected));

    public static void TheseValuesAreGenerated<T>(FuzzrOf<T> fuzzr, params T[] needsToBeSeen)
    {
        GeneratedValuesShouldEventuallySatisfyAll(fuzzr, [.. needsToBeSeen.Select(Is)]);
    }

    public static void GeneratesNullAndNotNull<T>(FuzzrOf<T> fuzzr)
        => GeneratedValuesShouldEventuallySatisfyAll(fuzzr,
            ("is null", a => a is null),
            ("is not null", a => a is not null));

    public static void GeneratedValuesShouldEventuallySatisfyAll<T>(
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
            => GeneratedValuesShouldEventuallySatisfyAll(100, fuzzr, labeledChecks);

    public static void GeneratedValuesShouldEventuallySatisfyAll<T>(
        int numberOfExecutions,
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        var signal = Signal.From<T>(a => Pulse.Trace(a!));
        var run =
            from inspector in "inspector".Stashed(
                () => signal.SetAndReturnArtery(new DistinctValueInspector<T>()))
            from input in "Fuzzr".Input(fuzzr)
            from inspect in "Inspect".Act(() => signal.Pulse(input))
            from _e in "early exit".TestifyProvenWhen(
                () => inspector.SeenSatisfyEach([.. labeledChecks.Select(a => a.Item2)]))
            from _s in "Assayer".Assay(
                [.. labeledChecks.Select(a => (a.Item1, (Func<bool>)(() => inspector.HasValueThatSatisfies(a.Item2))))])
            select Acid.Test;
        QState.Run(run).WithOneRun().And(numberOfExecutions.ExecutionsPerRun());
    }

    public static void GeneratedValuesShouldAllSatisfy<T>(
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        GeneratedValuesShouldAllSatisfy(20, fuzzr, labeledChecks);
    }

    public static void GeneratedValuesShouldAllSatisfy<T>(
        int numberOfExecutions,
        FuzzrOf<T> fuzzr,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        var run =
            from input in "Fuzzr".Input(fuzzr)
            from _ in CombineSpecs(input, labeledChecks) // Move this to QuickAcid
            select Acid.Test;
        QState.Run(run).WithOneRun().And(numberOfExecutions.ExecutionsPerRun());
    }

    private static QAcidScript<Acid> CombineSpecs<T>(T input, IEnumerable<(string, Func<T, bool>)> checks)
    {
        return checks
            .Select(c => c.Item1.Spec(() => c.Item2(input)))
            .Aggregate(Acc, (acc, next) => from _ in acc from __ in next select Acid.Test);
    }

    private static readonly QAcidScript<Acid> Acc =
        s => Vessel.AcidOnly(s);
}