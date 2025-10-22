using QuickPulse.Show;

namespace QuickFuzzr.Tests;

public class CompositionSpike
{

    [Fact]
    public void FirstShot()
    {
        var digit = Fuzzr.Char('0', '9');

        var ssn1 =
            from a in Fuzzr.String(digit, 3)
            from b in Fuzzr.String(digit, 2)
            from c in Fuzzr.String(digit, 4)
            select $"{a}-{b}-{c}";

        var ssn2 =
            from a in Fuzzr.Int(100, 999)
            from b in Fuzzr.Int(10, 99)
            from c in Fuzzr.Int(1000, 9999)
            select $"{a}-{b}-{c}";

        // ssn1.Generate().PulseToLog("result.log");
        // ssn2.Generate().PulseToLog("result.log");
    }
}
