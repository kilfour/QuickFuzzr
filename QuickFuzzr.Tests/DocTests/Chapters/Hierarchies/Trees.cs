
using QuickFuzzr;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Hierarchies;

public class Trees
{
	[Fact]
	[DocContent(@"Depth control together with the `.GenerateAsOneOf(...)` combinator mentioned above and the previously unmentioned `TreeLeaf<T>()` one allows you to build tree type hierarchies.  
Given the canonical abstract Tree, concrete Branch and Leaf example model, we can generate this like so:
```csharp
var generator =
	from _d in Fuzzr.For<Tree>().Depth(1, 3)
	from _i in Fuzzr.For<Tree>().GenerateAsOneOf(typeof(Branch), typeof(Leaf))
	from _l in Fuzzr.For<Tree>().AsLeaf<Leaf>()
	from tree in Fuzzr.One<Tree>()
	select tree;
```
Our leaf has an int value property, so the following:
```csharp
Console.WriteLine(generator.Generate().ToString());
```	
Would output something like:
```
Node(Leaf(31), Node(Leaf(71), Leaf(10)))
```
")]
	public void TreesTest()
	{
		var generator =
			from _1 in Configr<Tree>.Depth(1, 3)
			from _2 in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
			from _3 in Configr<Tree>.EndOn<Leaf>()
			from tree in Fuzzr.One<Tree>()
			select tree.ToLabel();

		var validLabels = new[] { "E", "LE", "RE", "LLE", "LRE", "RLE", "RRE" };

		_Tools.CheckIf.GeneratedValuesShouldEventuallySatisfyAll(200,
			generator,
			("has E", s => s.Split("|").Contains("E")),
			("has LE", s => s.Split("|").Contains("LE")),
			("has RE", s => s.Split("|").Contains("RE")),
			("has LLE", s => s.Split("|").Contains("LLE")),
			("has LRE", s => s.Split("|").Contains("LRE")),
			("has RLE", s => s.Split("|").Contains("RLE")),
			("has RRE", s => s.Split("|").Contains("RRE")),
			("valid", s => s.Split("|").All(validLabels.Contains))
		);
	}

	[Fact]
	[DocContent("**Note :** The `TreeLeaf<T>()` combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Configr<Tree>.Depth(1, 1);
		Assert.Equal(Intent.Fixed, generator.Generate());
	}

	private abstract class Tree
	{
		public abstract string ToLabel();
	}

	private class Leaf : Tree
	{
		public int Value { get; set; }
		public override string ToString() => "Leaf";

		public override string ToLabel() => "E";
	}

	private class Branch : Tree
	{
		public Tree? Left { get; set; }
		public Tree? Right { get; set; }

		public override string ToString() => $"Node";
		public override string ToLabel()
		{
			return string.Join("|",
				Prefix("L", Left!.ToLabel()),
				Prefix("R", Right!.ToLabel()));
		}

		private string Prefix(string prefix, string labels)
		{
			return string.Join("|",
				labels.Split('|').Select(label => prefix + label));
		}
	}
}