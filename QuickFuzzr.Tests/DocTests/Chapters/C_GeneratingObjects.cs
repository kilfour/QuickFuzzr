using QuickFuzzr.Tests.DocTests.Chapters.Objects;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters;

[DocFile]
[DocInclude(typeof(ASimpleObject))]
[DocInclude(typeof(IgnoringProperties))]
[DocInclude(typeof(CustomizingProperties))]
[DocInclude(typeof(CustomizingConstructors))]
[DocInclude(typeof(ManyObjects))]
[DocInclude(typeof(Inheritance))]
[DocInclude(typeof(ToArray))]
[DocInclude(typeof(ToList))]
[DocInclude(typeof(ReplacingPrimitiveGenerators))]
public class C_GeneratingObjects { }