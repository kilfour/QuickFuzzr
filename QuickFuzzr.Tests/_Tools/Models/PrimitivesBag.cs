namespace QuickFuzzr.Tests._Tools.Models;

public class PrimitivesBag<T> where T : struct
{
    public T Value { get; set; } = default!;
    public T? NullableValue { get; set; } = default!;
}

public class StringBag
{
    public string Value { get; set; } = string.Empty;
    public string? NullableValue { get; set; }
}

public class Thing
{
    public int Id { get; set; }
    public int Prop { get; set; }
}

public class DerivedThing : Thing { public int PropOnDerived { get; set; } }