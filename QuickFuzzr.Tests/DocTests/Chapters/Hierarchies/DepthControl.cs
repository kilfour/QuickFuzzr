using QuickPulse.Explains;


namespace QuickFuzzr.Tests.DocTests.Chapters.Hierarchies;

public class Recurse
{
	public Recurse? Child { get; set; }
	public NoRecurse? OtherChild { get; set; }
	public override string ToString()
	{
		var childString =
			Child == null ? "<null>" : Child.ToString();
		var otherChildString =
			OtherChild == null ? "<null>" : "{ NoRecurse }";
		return $"{{ Recurse: Child = {childString}, OtherChild = {otherChildString} }}";
	}
}

public class NoRecurse { }

public class DepthControl
{
	[Fact]
	public void DefaultDepth()
	{
		var generator = Fuzzr.One<Recurse>();

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.Null(value.Child);
		Assert.NotNull(value.OtherChild);
	}

	[Fact]
	public void WithDepth2()
	{
		var generator =
			from _ in Configr<Recurse>.Depth(2, 2)
			from recurse in Fuzzr.One<Recurse>()
			select recurse;

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.NotNull(value.OtherChild);
		Assert.NotNull(value.Child);
		Assert.NotNull(value.Child.OtherChild);
		Assert.Null(value.Child.Child);
	}

	[Fact]
	public void WithDepth3()
	{

		var generator =
			from _ in Configr<Recurse>.Depth(3, 3)
			from thing in Fuzzr.One<Recurse>()
			select thing;

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.NotNull(value.OtherChild);
		Assert.NotNull(value.Child);
		Assert.NotNull(value.Child.OtherChild);
		Assert.NotNull(value.Child.Child);
		Assert.NotNull(value.Child.Child.OtherChild);
		Assert.Null(value.Child.Child.Child);
	}

	[Fact]
	[DocContent(
@"Using for instance `.Depth(1, 3)` allows the generator to randomly choose a depth between 1 and 3 (inclusive) for that type.
This means some instances will be shallow, while others may be more deeply nested, introducing variability within the defined bounds.")]
	public void WithDepth_1_3()
	{
		var generator =
			from _ in Configr<Recurse>.Depth(1, 3)
			from value in Fuzzr.One<Recurse>()
			select value;

		_Tools.CheckIf.GeneratedValuesShouldEventuallySatisfyAll(
			generator.Select(GetDepthString),
			("has depth 1", d => d == "1"),
			("has depth 2", d => d == "2"),
			("has depth 3", d => d == "3"),
			("no depth 4", d => d != "4")
		);
	}

	public string GetDepthString(Recurse thing)
	{
		if (thing.Child == null) return "1";
		if (thing.Child.Child == null) return "2";
		if (thing.Child.Child.Child == null) return "3";
		return "4";
	}

	public class SomeThingToGenerate
	{
		public SomeComponent? MyComponent { get; set; }
		public SomeChildToGenerate? MyChild { get; set; }
		public int Fire { get; set; }
	}

	public class SomeChildToGenerate
	{
		public SomeComponent? MyComponent { get; set; }
		public int WalkWithMe { get; set; }
	}

	public class SomeComponent
	{
		public int TheAnswer { get; set; }
	}

	public class SomeThingRecursive
	{
		public SomeThingRecursive? Curse { get; set; }
	}
}