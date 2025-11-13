using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools;

public class DocFileCodeHeaderAttribute(string code)
    : DocFileHeaderAttribute(code.Replace("<", "&lt;").Replace(">", "&gt;"));
