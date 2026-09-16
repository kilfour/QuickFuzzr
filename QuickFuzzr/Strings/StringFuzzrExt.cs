namespace QuickFuzzr.Strings;

public static class StringFuzzrExt
{
    public static FuzzrOf<string> InsertOneOf(this FuzzrOf<string> fuzzr, params char[] chars) =>
        from text in fuzzr
        from index in Fuzzr.Int(0, text.Length + 1)
        from ch in Fuzzr.OneOf(chars)
        select text.Insert(index, ch.ToString());

    public static Replacer ReplaceOneOf(this FuzzrOf<string> fuzzr, params char[] candidates) =>
        new(fuzzr, candidates);

    public class Replacer(FuzzrOf<string> fuzzr, char[] candidates)
    {
        public FuzzrOf<string> WithOneOf(char[] replacements) =>
            from text in fuzzr
            let indexes =
                text.Select((a, i) => (Char: a, Index: i))
                    .Where(a => candidates.Any(b => a.Char == b))
                    .Select(a => a.Index)
            from index in Fuzzr.OneOf(indexes)
            from ch in Fuzzr.OneOf(replacements)
            select text[..index] + ch + text[(index + 1)..];
    }

    public static FuzzrOf<string> ReplaceOneWithOneOf(this FuzzrOf<string> fuzzr, char[] chars) =>
        from text in fuzzr
        from index in Fuzzr.Int(0, text.Length)
        from ch in Fuzzr.OneOf(chars)
        select text[..index] + ch + text[(index + 1)..];
}
