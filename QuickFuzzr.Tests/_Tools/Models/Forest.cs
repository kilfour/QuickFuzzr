namespace QuickFuzzr.Tests._Tools.Models;

public abstract class Tree { }
public class Leaf : Tree { }
public class Branch : Tree
{
    public Tree? Left { get; set; }
    public Tree? Right { get; set; }
}
