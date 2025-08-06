using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests;

[DocFile]
[DocContent(@"> A type-walking cheetah with a hand full of random.")]
public class QuickFuzzrTests
{
    [DocHeader("Installation")]
    [DocContent(@"QuickFuzzrQuickFuzzr is available on NuGet:
```bash
Install-Package QuickFuzzr
```
Or via the .NET CLI:
```bash
dotnet add package QuickFuzzr
```")]
    public void PlaceHolder() { }
}