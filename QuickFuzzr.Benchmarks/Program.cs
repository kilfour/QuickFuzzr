using BenchmarkDotNet.Running;
using QuickFuzzr.Benchmarks;

//new FlatBenchmarks().Log();

BenchmarkRunner.Run(
[
    typeof(FlatBenchmarks),
    typeof(HorsesForCoursesBenchmarks),
    typeof(PseudopolisBenchmarks),
    typeof(Trees),
]);
