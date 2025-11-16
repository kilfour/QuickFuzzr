
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Show;

namespace QuickFuzzr.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class FlatBenchmarks
{
    public class Model
    {
        public string? Name { get; set; }
        public int NaturalNumber { get; set; }
        public decimal Money { get; set; }
        public DateTime Date { get; set; }
        public bool Boolean { get; set; }
    }

    private const int Count = 10_000;

    private FuzzrOf<Model> autoFuzzr = default!;
    private FuzzrOf<Model> configFuzzr = default!;
    private FuzzrOf<Intent> preloadConfig = default!;
    private FuzzrOf<Model> factoryFuzzr = default!;

    [GlobalSetup]
    public void Setup()
    {
        autoFuzzr = Fuzzr.One<Model>();

        configFuzzr =
            from _ in Configr.IgnoreAll()
            from name in Configr<Model>.Property(a => a.Name, Fuzzr.String())
            from nat in Configr<Model>.Property(a => a.NaturalNumber, Fuzzr.Int())
            from money in Configr<Model>.Property(a => a.Money, Fuzzr.Decimal())
            from date in Configr<Model>.Property(a => a.Date, Fuzzr.DateTime())
            from flag in Configr<Model>.Property(a => a.Boolean, Fuzzr.Bool())
            from model in Fuzzr.One<Model>()
            select model;

        preloadConfig =
            from _ in Configr.IgnoreAll()
            from name in Configr<Model>.Property(a => a.Name, Fuzzr.String())
            from nat in Configr<Model>.Property(a => a.NaturalNumber, Fuzzr.Int())
            from money in Configr<Model>.Property(a => a.Money, Fuzzr.Decimal())
            from date in Configr<Model>.Property(a => a.Date, Fuzzr.DateTime())
            from flag in Configr<Model>.Property(a => a.Boolean, Fuzzr.Bool())
            select Intent.Fixed;

        factoryFuzzr =
            from _ in Configr.IgnoreAll()
            from name in Fuzzr.String()
            from nat in Fuzzr.Int()
            from money in Fuzzr.Decimal()
            from date in Fuzzr.DateTime()
            from flag in Fuzzr.Bool()
            from model in Fuzzr.One<Model>()
            select new Model
            {
                Name = name,
                NaturalNumber = nat,
                Money = money,
                Date = date,
                Boolean = flag
            };
    }

    [Benchmark]
    public List<Model> AutoFuzzr()
        => [.. autoFuzzr.Many(Count).Generate()];

    [Benchmark]
    public List<Model> ConfigFuzzr()
        => [.. configFuzzr.Many(Count).Generate()];

    [Benchmark]
    public List<Model> PreloadedConfigFuzzr()
        => [.. (from cfg in preloadConfig from models in Fuzzr.One<Model>().Many(Count) select models).Generate()];

    [Benchmark]
    public List<Model> FactoryFuzzr()
       => [.. factoryFuzzr.Many(Count).Generate()];
}
