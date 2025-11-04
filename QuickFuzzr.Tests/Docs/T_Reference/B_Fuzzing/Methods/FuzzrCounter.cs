using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.T_Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.Counter(object key)")]
public class FuzzrCounter
{
	[Fact]
	[DocContent(
@"This generator returns an `int` starting at 1, and incrementing by 1 on each subsequent call.")]
	public void Counter_Generates_One()
		=> Assert.Equal(1, Fuzzr.Counter("a").Generate());

	[Fact]
	public void Counter_Many_Produces_Expected_Run()
		=> Assert.Equal([1, 2, 3, 4, 5], Fuzzr.Counter("a").Many(5).Generate());

	[Fact]
	public void Counter_Apply_Preserves_Sequence()
		=> Assert.Equal([12, 13, 14], Fuzzr.Counter("a").Apply(x => x + 11).Many(3).Generate());

	[Fact]
	public void Counter_Instances_Are_Independent()
	{
		var gen =
			from a in Fuzzr.Counter("a")
			from b in Fuzzr.Counter("b").Apply(x => x + 11)
			select (a, b);
		var result = gen.Many(2).Generate().ToArray();
		Assert.Equal(1, result[0].a);
		Assert.Equal(2, result[1].a);
		Assert.Equal(12, result[0].b);
		Assert.Equal(13, result[1].b);
	}

	[Fact]
	public void Counter_Resets_Between_Runs()
	{
		var gen = Fuzzr.Counter("a");
		Assert.Equal([1, 2, 3], gen.Many(3).Generate());
		Assert.Equal([1, 2, 3], gen.Many(3).Generate());
	}
}