namespace QuickFuzzr;

public class HeyITriedFiftyTimesButCouldNotGetADifferentValue
	: Exception
{
	public HeyITriedFiftyTimesButCouldNotGetADifferentValue() { }

	public HeyITriedFiftyTimesButCouldNotGetADifferentValue(string message)
		: base(message) { }
}