namespace QuickFuzzr.Tests._Tools.Models;

public class MultiCtorContainer
{
    public int? AnInt1 { get; private set; }
    public int? AnInt2 { get; private set; }
    public int? AnInt3 { get; private set; }
    public int? AnInt4 { get; private set; }
    public string? AString { get; private set; }

    public MultiCtorContainer() { }

    public MultiCtorContainer(int anInt1)
    {
        AnInt1 = anInt1;
    }

    public MultiCtorContainer(int anInt1, int anInt2)
    {
        AnInt1 = anInt1;
        AnInt2 = anInt2;
    }

    public MultiCtorContainer(int anInt1, int anInt2, int anInt3)
    {
        AnInt1 = anInt1;
        AnInt2 = anInt2;
        AnInt3 = anInt3;
    }

    public MultiCtorContainer(int anInt1, int anInt2, int anInt3, int anInt4)
    {
        AnInt1 = anInt1;
        AnInt2 = anInt2;
        AnInt3 = anInt3;
        AnInt4 = anInt4;
    }

    public MultiCtorContainer(int anInt1, int anInt2, int anInt3, int anInt4, string aString)
    {
        AnInt1 = anInt1;
        AnInt2 = anInt2;
        AnInt3 = anInt3;
        AnInt4 = anInt4;
        AString = aString;
    }
}