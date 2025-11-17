using BenchmarkDotNet.Attributes;
using QuickFuzzr.Tests._Tools.Models;

namespace QuickFuzzr.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class FlatBenchmarks
{
    private const int Count = 10_000;

    private FuzzrOf<Pseudopolis> autoFuzzr = default!;
    private FuzzrOf<Pseudopolis> configFuzzr = default!;
    private FuzzrOf<Intent> preloadConfig = default!;
    private FuzzrOf<Pseudopolis> factoryFuzzr = default!;

    [GlobalSetup]
    public void Setup()
    {
        autoFuzzr = Fuzzr.One<Pseudopolis>();

        configFuzzr =
            from _ in Configr.IgnoreAll()
            from name in Configr<Pseudopolis>.Property(a => a.Name, Fuzzr.String())
            from nat in Configr<Pseudopolis>.Property(a => a.NaturalNumber, Fuzzr.Int())
            from money in Configr<Pseudopolis>.Property(a => a.Money, Fuzzr.Decimal())
            from date in Configr<Pseudopolis>.Property(a => a.Date, Fuzzr.DateTime())
            from flag in Configr<Pseudopolis>.Property(a => a.Boolean, Fuzzr.Bool())
            from model in Fuzzr.One<Pseudopolis>()
            select model;

        preloadConfig =
            from _ in Configr.IgnoreAll()
            from name in Configr<Pseudopolis>.Property(a => a.Name, Fuzzr.String())
            from nat in Configr<Pseudopolis>.Property(a => a.NaturalNumber, Fuzzr.Int())
            from money in Configr<Pseudopolis>.Property(a => a.Money, Fuzzr.Decimal())
            from date in Configr<Pseudopolis>.Property(a => a.Date, Fuzzr.DateTime())
            from flag in Configr<Pseudopolis>.Property(a => a.Boolean, Fuzzr.Bool())
            select Intent.Fixed;

        factoryFuzzr =
            from _ in Configr.IgnoreAll()
            from name in Fuzzr.String()
            from nat in Fuzzr.Int()
            from money in Fuzzr.Decimal()
            from date in Fuzzr.DateTime()
            from flag in Fuzzr.Bool()
            from model in Fuzzr.One<Pseudopolis>()
            select new Pseudopolis
            {
                Name = name,
                NaturalNumber = nat,
                Money = money,
                Date = date,
                Boolean = flag
            };
    }

    [Benchmark]
    public List<Pseudopolis> AutoFuzzr()
        => [.. autoFuzzr.Many(Count).Generate()];

    [Benchmark]
    public List<Pseudopolis> ConfigFuzzr()
        => [.. configFuzzr.Many(Count).Generate()];

    [Benchmark]
    public List<Pseudopolis> PreloadedConfigFuzzr()
        => [.. (from cfg in preloadConfig from models in Fuzzr.One<Pseudopolis>().Many(Count) select models).Generate()];

    [Benchmark]
    public List<Pseudopolis> FactoryFuzzr()
       => [.. factoryFuzzr.Many(Count).Generate()];
}
