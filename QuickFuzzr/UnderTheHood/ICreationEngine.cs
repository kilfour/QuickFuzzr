namespace QuickFuzzr.UnderTheHood;

public interface ICreationEngine
{
    object Create(State state, Type type);
    object Create(State state, Type type, Func<Type?, object> ctor);
}
