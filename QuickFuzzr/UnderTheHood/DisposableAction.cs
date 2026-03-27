namespace QuickFuzzr.UnderTheHood;

public class DisposableAction(Action onDispose) : IDisposable
{
    private readonly Action onDispose = onDispose;
    public void Dispose() => onDispose();
}