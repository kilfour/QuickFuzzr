using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools;

public class DocSignatureAttribute(string signature) : DocContentAttribute($"\n**Signature:**  \n```csharp\n{signature}\n```\n");