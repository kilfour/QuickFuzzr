namespace QuickFuzzr.Strings;

public static class CharSet
{
    /// <summary>ASCII digits 0 through 9.</summary>
    public static readonly char[] Digits = "0123456789".ToCharArray();

    /// <summary>ASCII lowercase letters a through z.</summary>
    public static readonly char[] Lowercase = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    /// <summary>ASCII uppercase letters A through Z.</summary>
    public static readonly char[] Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>ASCII lowercase and uppercase letters.</summary>
    public static readonly char[] Letters = [.. Lowercase, .. Uppercase];

    /// <summary>ASCII letters and digits.</summary>
    public static readonly char[] AlphaNumeric = [.. Letters, .. Digits];

    /// <summary>Hexadecimal digits, including both letter cases.</summary>
    public static readonly char[] HexDigits = "0123456789abcdefABCDEF".ToCharArray();

    /// <summary>Common straight and curly quotation marks.</summary>
    public static readonly char[] Quotes = ['\'', '"', '\u2018', '\u2019', '\u201C', '\u201D'];

    /// <summary>Individual line-break characters, including Unicode line and paragraph separators.</summary>
    public static readonly char[] LineBreaks = ['\r', '\n', '\u0085', '\u2028', '\u2029'];

    /// <summary>ASCII control characters U+0000 through U+001F, plus DEL.</summary>
    public static readonly char[] Control = [.. Enumerable.Range(0, 32).Select(value => (char)value), '\u007F'];

    /// <summary>A selection of common whitespace characters, including non-breaking space.</summary>
    public static readonly char[] Whitespace = [' ', '\t', '\r', '\n', '\u00A0'];
}
