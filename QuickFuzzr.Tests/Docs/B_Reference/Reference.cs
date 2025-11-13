using QuickFuzzr.Tests.Docs.B_Reference.P_Primitives;
using QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing;
using QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods;
using QuickFuzzr.Tests.Docs.B_Reference.D_Configuration;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference;

[DocFile]
[DocLink(typeof(PrimitiveFuzzrs), "PrimitiveFuzzrs")]
[DocLink(typeof(Fuzzing), "Fuzzing")]
[DocLink(typeof(FuzzrExtensionMethods), "FuzzrExtensionMethods")]
[DocLink(typeof(Configuring), "Configuring")]
[DocContent(
@"This reference provides a **complete, factual overview** of QuickFuzzr's public API.
It lists all available fuzzrs, configuration points, and extension methods, organized by category.  
Each entry includes a concise description of its purpose and behavior,
serving as a quick lookup for day-to-day use or library integration.

If you're looking for examples or background explanations, see the guide or cookbook.

All examples and summaries are real, verified through executable tests, ensuring what you see here is exactly what QuickFuzzr does.

QuickFuzzr exposes three kinds of building blocks: `FuzzrOf<T>` for value production, `Configr` for generation behavior, and extension methods for modifying fuzzrs. 

Everything in this reference fits into one of these three roles.
")]
public class Reference
{
  [DocHeader("Contents")]
  [DocContent(
@"
- [Fuzzing][Fuzzing]
- [Configuration][Configuring]
- [Fuzzr Extension Methods][FuzzrExtensionMethods]
- [Primitive Fuzzrs][PrimitiveFuzzrs]
")]
  public static void Contents() { }
}