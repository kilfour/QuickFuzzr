namespace QuickFuzzr.Strings;

public static class StringFuzzrExt
{
    /// <summary>
    /// Inserts one randomly chosen character at a random position in each generated string, including either end.
    /// An empty source string becomes a single-character string.
    /// </summary>
    public static FuzzrOf<string> InsertOneOf(this FuzzrOf<string> fuzzr, params char[] chars)
    {
        ArgumentNullException.ThrowIfNull(fuzzr);
        CheckCharacters(chars, nameof(chars));
        return from text in fuzzr
        from index in Fuzzr.Int(0, text.Length + 1)
        from ch in Fuzzr.OneOf(chars)
        select text.Insert(index, ch.ToString());
    }

    /// <summary>
    /// Specifies the candidate characters for a replacement completed by calling WithOneOf.
    /// Strings containing no candidate characters are preserved.
    /// </summary>
    public static Replacer ReplaceOneOf(this FuzzrOf<string> fuzzr, params char[] candidates) =>
        new(fuzzr, candidates);

    public class Replacer
    {
        private readonly FuzzrOf<string> fuzzr;
        private readonly char[] candidates;

        /// <summary>Creates a replacement configuration for a source generator and a nonempty set of candidate characters.</summary>
        public Replacer(FuzzrOf<string> fuzzr, char[] candidates)
        {
            ArgumentNullException.ThrowIfNull(fuzzr);
            CheckCharacters(candidates, nameof(candidates));
            this.fuzzr = fuzzr;
            this.candidates = candidates;
        }

        /// <summary>Replaces one matching character, or returns the original string when none match.</summary>
        public FuzzrOf<string> WithOneOf(char[] replacements)
        {
            CheckCharacters(replacements, nameof(replacements));
            return from text in fuzzr
            let indexes =
                text.Select((a, i) => (Char: a, Index: i))
                    .Where(a => candidates.Any(b => a.Char == b))
                    .Select(a => a.Index).ToArray()
            from result in indexes.Length == 0
                ? Fuzzr.Constant(text)
                : ReplaceAt(text, Fuzzr.OneOf(indexes), replacements)
            select result;
        }
    }

    /// <summary>Replaces one character, or returns the original string when it is empty.</summary>
    public static FuzzrOf<string> ReplaceOneWithOneOf(this FuzzrOf<string> fuzzr, char[] chars)
    {
        ArgumentNullException.ThrowIfNull(fuzzr);
        CheckCharacters(chars, nameof(chars));
        return from text in fuzzr
        from result in text.Length == 0
            ? Fuzzr.Constant(text)
            : ReplaceAt(text, Fuzzr.Int(0, text.Length), chars)
        select result;
    }

    /// <summary>Replaces the character at a generated index with a randomly chosen character.</summary>
    private static FuzzrOf<string> ReplaceAt(string text, FuzzrOf<int> indexes, char[] chars) =>
        from index in indexes
        from ch in Fuzzr.OneOf(chars)
        select text[..index] + ch + text[(index + 1)..];

    /// <summary>Rejects null or empty character choices using the caller's parameter name.</summary>
    private static void CheckCharacters(char[] chars, string paramName)
    {
        ArgumentNullException.ThrowIfNull(chars, paramName);
        if (chars.Length == 0)
            throw new ArgumentException("Supply at least one character.", paramName);
    }
}
