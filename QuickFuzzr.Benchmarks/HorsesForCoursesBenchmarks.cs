using BenchmarkDotNet.Attributes;
using QuickFuzzr.Tests.Docs.A_Guide.Q_ShowCase;

namespace QuickFuzzr.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class HorsesForCoursesBenchmarks
{
    [Benchmark]
    public void FromTheGuide()
        => TheFinalShowcase.Lots();
}
