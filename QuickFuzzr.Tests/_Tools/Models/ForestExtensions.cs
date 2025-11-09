namespace QuickFuzzr.Tests._Tools.Models;

public static class ForestExtensions
{
    public static string ToTreeLabel(this Tree tree)
    {
        if (tree is Branch)
        {
            return $"Branch";
        }
        return $"Leaf";
    }

    public static string ToLabel(this Tree tree)
    {
        if (tree is Branch branch)
        {
            return string.Join("|",
                Prefix("L", branch.Left!.ToLabel()),
                Prefix("R", branch.Right!.ToLabel()));
        }
        return "E";
    }

    private static string Prefix(string prefix, string labels)
        => string.Join("|", labels.Split('|').Select(label => prefix + label));
}