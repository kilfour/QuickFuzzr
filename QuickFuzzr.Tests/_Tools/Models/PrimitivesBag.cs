namespace QuickFuzzr.Tests._Tools.Models;

public class PrimitivesBag<T> where T : struct
{
    public T Value { get; set; } = default!;
    public T? NullableValue { get; set; } = default!;
}