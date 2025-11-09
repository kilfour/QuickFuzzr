namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Represents the base class for all exceptions thrown by QuickFuzzr.
/// Provides a common ancestor for more specific exception types related
/// to generator configuration, construction, and value production.
/// </summary>
public class QuickFuzzrException : Exception
{
    public QuickFuzzrException() : base() { }
    public QuickFuzzrException(string message) : base(message) { }
    public QuickFuzzrException(string message, Exception innerException) : base(message, innerException) { }
}
