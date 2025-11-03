namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

public class QuickFuzzrException : Exception
{
    public QuickFuzzrException() : base() { }
    public QuickFuzzrException(string message) : base(message) { }
}
