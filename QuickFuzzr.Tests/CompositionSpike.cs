using QuickPulse.Show;

namespace QuickFuzzr.Tests;

public class CompositionSpike
{

    [Fact]
    public void FirstShot()
    {
        var digit = Fuzzr.Char('0', '9');
        Func<IEnumerable<char>, string> charsToString =
            a => new string([.. a]);
        var ssn =
            from d1 in digit.Many(3)
            from d2 in digit.Many(2)
            from d3 in digit.Many(4)
            select d1 + "-" + d2 + "-" + d3;
        var ssn2 =
            from a in Fuzzr.Int(100, 999)
            from b in Fuzzr.Int(10, 99)
            from c in Fuzzr.String(1000, 9999)
            select $"{a}-{b}-{c}";
        //select $"{d1}-{d2}-{d3}";
        //ssn2.Generate().PulseToLog("result.log");
    }
}

// public static class Ext
// {
//     public static string CharsAsString
// } 