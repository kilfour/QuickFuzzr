
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.Tests.Docs.C_Cookbook.B_SometimesTheCheetahNeedsToRun;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Show;

namespace QuickFuzzr.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class PseudopolisBenchmarks
{
    [Benchmark]
    public List<Pseudopolis> AutoFuzzr()
        => [.. SometimesTheCheetahNeedsToRun.PseudopolisAutoFuzzr()];

    [Benchmark]
    public List<Pseudopolis> ConfigFuzzr()
        => [.. SometimesTheCheetahNeedsToRun.PseudopolisFuzzr()];

    [Benchmark]
    public List<Pseudopolis> PreloadedConfigFuzzr()
        => [.. SometimesTheCheetahNeedsToRun.PseudopolisConfigr()];
}
