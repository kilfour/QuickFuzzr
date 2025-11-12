using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.Counter(object key)")]
[DocColumn(Fuzzing.Columns.Description, "Generates a sequential integer per key, starting at 1.")]
public class FuzzrCounter
{
	[Fact]
	[DocContent(
@"This generator returns an `int` starting at 1, and incrementing by 1 on each call.  
Useful for generating unique sequential IDs or counters.  
")]
	public void Counter_Generates_One()
		=> Assert.Equal(1, Fuzzr.Counter("a").Generate());

	[CodeSnippet]
	[CodeRemove("return ")]
	private static IEnumerable<int> Usage_Example()
	{
		return Fuzzr.Counter("the-key").Many(5).Generate();
		// Returns => [1, 2, 3, 4, 5]
	}

	[Fact]
	[DocUsage]
	[DocExample(typeof(FuzzrCounter), nameof(Usage_Example))]
	public void Counter_Many_Produces_Expected_Run()
		=> Assert.Equal([1, 2, 3, 4, 5], Usage_Example());

	[Fact]
	[DocContent("- Each `key` maintains its own independent counter sequence.")]
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
	[DocContent("- Counter state resets between separate `Generate()` calls.")]
	public void Counter_Resets_Between_Runs()
	{
		var gen = Fuzzr.Counter("a");
		Assert.Equal([1, 2, 3], gen.Many(3).Generate());
		Assert.Equal([1, 2, 3], gen.Many(3).Generate());
	}

	[Fact]
	[DocContent("- Works seamlessly in LINQ chains and with .Apply(...) to offset or transform the sequence.")]
	public void Counter_Apply_Preserves_Sequence()
		=> Assert.Equal([12, 13, 14], Fuzzr.Counter("a").Apply(x => x + 11).Many(3).Generate());

	[Fact]
	[DocExceptions]
	[DocException("ArgumentNullException", "When the provided key is null.")]
	public void Counter_Null_Key() // Check: change message
	{
		var ex = Assert.Throws<ArgumentNullException>(() => Fuzzr.Counter(null!).Generate());
		Assert.Equal("Value cannot be null. (Parameter 'key')", ex.Message);
	}
}