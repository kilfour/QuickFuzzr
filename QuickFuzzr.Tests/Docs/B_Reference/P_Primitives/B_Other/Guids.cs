using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.B_Other;


[DocContent("Use `Fuzzr.Guid()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces non-empty random `Guid` values.")]
public class Guids : Primitive<Guid>
{
	protected override FuzzrOf<Guid> CreateFuzzr() => Fuzzr.Guid();

	[Fact]
	[DocContent("- The default Fuzzr never generates Guid.Empty.")]
	public void NeverGuidEmpty()
	{
		var fuzzr = Fuzzr.Guid();
		for (int i = 0; i < 10; i++)
		{
			var val = fuzzr.Generate();
			Assert.NotEqual(Guid.Empty, val);
		}
	}

	[Fact]
	[DocContent("- `Fuzzr.Guid()` is deterministic when seeded.")]
	public void DeterministicWithSeed()
	{
		Assert.Equal("96ba173e-04ae-3bcd-9986-9e56f0adbf3a", Fuzzr.Guid().Generate(42).ToString().ToLower());
	}
}