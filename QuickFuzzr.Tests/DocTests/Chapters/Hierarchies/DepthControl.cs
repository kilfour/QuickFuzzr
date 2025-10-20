using QuickFuzzr.UnderTheHood;
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

[DocContent(@"As mentioned in the *A simple object section*: “The object properties will also be automatically filled in.”
However, this automatic population only applies to the first level of object properties.
Deeper properties will remain null unless configured otherwise.  
So if we have the following class :
```csharp
public class NoRecurse { }
public class Recurse
{
	public Recurse Child { get; set; }
	public NoRecurse OtherChild { get; set; }
	public override string ToString()
	{
		var childString =
			Child == null ? ""<null>"" : Child.ToString();
		var otherChildString =
			OtherChild == null ? ""<null>"" : ""{ NoRecurse }"";
		return $""{{ Recurse: Child = {childString}, OtherChild = {otherChildString} }}"";
	}
}
```")]
public class DepthControl
{
	[Fact]
	[DocContent(
@"If we then do :
```csharp
Console.WriteLine(Fuzz.One<Recurse>().Generate().ToString());
```
It outputs : 
```
{ Recurse: Child = <null>, OtherChild = { NoRecurse } }
```
While this may seem counter-intuitive, it is an intentional default to prevent infinite recursion or overly deep object trees.
Internally, a `DepthConstraint(int Min, int Max)` is registered per type.
The default values are `new(1, 1)`.  
Revisiting our example we can see that both types have indeed been generated with these default values.")]
	public void DefaultDepth()
	{
		var generator = Fuzzr.One<Recurse>();

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.Null(value.Child);
		Assert.NotNull(value.OtherChild);
	}

	[Fact]
	[DocContent(
@"You can control generation depth per type using the `.Depth(min, max)` combinator.  
For instance:
```csharp
var generator =
	from _ in Fuzz.For<Recurse>().Depth(2, 2)
	from recurse in Fuzz.One<Recurse>()
	select recurse;
Console.WriteLine(generator.Generate().ToString());
```
Outputs:
```
{ Recurse: Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } }
, OtherChild = { NoRecurse } 
}
```
 ")]
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
	[DocContent(
@"Recap:
```
Depth(1, 1)
{ Recurse: Child = <null>, OtherChild = { NoRecurse } }

Depth(2, 2)
{ Recurse: 
	Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } },
  	OtherChild = { NoRecurse } 
}

Depth(3, 3)
{ Recurse: 
	Child = { Recurse: 
		Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } },
        OtherChild = { NoRecurse } },
  	OtherChild = { NoRecurse } 
}
```
 ")]
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

	[Fact]
	[DocContent("**Note :** The `Depth(...)` combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Configr<SomeComponent>.Depth(1, 1);
		Assert.Equal(Unit.Instance, generator.Generate());
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