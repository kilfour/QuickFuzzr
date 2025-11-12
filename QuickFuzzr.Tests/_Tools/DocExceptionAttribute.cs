using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools;

public class DocExceptionAttribute(string exception, string when)
    : DocContentAttribute($"- `{exception}`: {when}");