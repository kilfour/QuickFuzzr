using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests;

[DocFile]
[DocFileHeader("QuickFuzzr")]
[DocContent(@"> A type-walking cheetah with a hand full of random.")]
public class CreateDoc
{
    [Fact]
    [DocHeader("Installation")]
    [DocContent(@"QuickFuzzrQuickFuzzr is available on NuGet:
```bash
Install-Package QuickFuzzr
```
Or via the .NET CLI:
```bash
dotnet add package QuickFuzzr
```")]
    public void Now() { Explain.This<CreateDoc>("README.md"); }
}