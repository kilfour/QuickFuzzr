using BenchmarkDotNet.Attributes;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.Tests.Docs.C_Cookbook.A_ADeepDarkForest;
using QuickFuzzr.Tests.Docs.C_Cookbook.B_SometimesTheCheetahNeedsToRun;

namespace QuickFuzzr.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class Trees
{
    [Benchmark]
    public List<Tree> ConfigFuzzr()
        => [.. SometimesTheCheetahNeedsToRun.ConfigrFuzzr(ADeepDarkForest.TreesTest_Fuzzr())];

    [Benchmark]
    public List<Tree> PreloadedConfigFuzzr()
        => [.. SometimesTheCheetahNeedsToRun.PreloadedConfigrFuzzr(SometimesTheCheetahNeedsToRun.TreesTest_Configr())];

}
