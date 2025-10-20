using QuickFuzzr.UnderTheHood;
using QuickPulse.Instruments;
using QuickPulse.Show;

namespace QuickFuzzr.Tests;

public class ConfigurationSpike
{
    private static Generator<int> Counter()
    {
        var counter = 1;
        return state => Chain.It(() => counter++, new Result<int>(counter, state));
    }

    [Fact]
    public void FirstShot()
    {
        var cfg =
            from _1 in Configr<Order>.Property(a => a.OrderId, Counter())
            from _2 in Configr.Property(a => a.Name == "Quantity", Fuzzr.Int(100, 200))
            from _3 in Configr<Order>.Property(a => a.Item, Fuzzr.OneOf("toy", "phone", "apple"))
            select Unit.Instance;
        var ordersGen =
            from _ in cfg
            from order1 in Fuzzr.One<Order>()
            from __ in Configr<Order>.Property(a => a.Item, "OVERRIDE")
            from order2 in Fuzzr.One<Order>()
            select new Order[] { order1, order2 };
        ordersGen.Generate().PulseToLog("orders.log");
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string Item { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
    }
}