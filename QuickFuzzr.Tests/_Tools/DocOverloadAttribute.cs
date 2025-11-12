using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools;

public class DocOverloadAttribute(string signature) : DocContentAttribute($"- `{signature}`");