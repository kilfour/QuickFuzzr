namespace QuickFuzzr.UnderTheHood;

public static class TypeExtensions
{
    public static bool IsGenericTypeOf(this Type type, Type openGeneric)
        => type.IsGenericType && type.GetGenericTypeDefinition() == openGeneric;
}