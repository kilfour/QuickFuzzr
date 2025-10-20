using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent(@"Use the `Fuzzr.For<T>().Construct<TArg>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```csharp
Fuzzr.For<SomeThing>().Construct(Fuzzr.Constant(42))
```")]
public class CustomizingConstructors
{
	[Fact]
	[DocContent("Subsequent calls to `Fuzzr.One<T>()` will then use the registered constructor.")]
	public void Works()
	{
		var generator =
			from i in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from c in Configr<SomeThing>.Construct(Fuzzr.Constant(42))
			from r in Fuzzr.One<SomeThing>()
			select r;
		Assert.Equal(42, generator.Generate().AnInt1);
	}

	[Fact]
	[DocContent(@"Various overloads exist : 
 -  `Fuzzr.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2)`")]
	public void WorksTwoArgs()
	{
		var generator =
			from i1 in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from i2 in Configr<SomeThing>.Ignore(a => a.AnInt2)
			from c in Configr<SomeThing>.Construct(Fuzzr.Constant(42), Fuzzr.Constant(43))
			from r in Fuzzr.One<SomeThing>()
			select r;
		var result = generator.Generate();
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
	}

	[Fact]
	[DocContent(" -  `Fuzzr.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3)`")]
	public void WorksThreeArgs()
	{
		var generator =
			from i1 in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from i2 in Configr<SomeThing>.Ignore(a => a.AnInt2)
			from i3 in Configr<SomeThing>.Ignore(a => a.AnInt3)
			from c in Configr<SomeThing>.Construct(Fuzzr.Constant(42), Fuzzr.Constant(43), Fuzzr.Constant(44))
			from r in Fuzzr.One<SomeThing>()
			select r;
		var result = generator.Generate();
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
	}

	[Fact]
	[DocContent(" -  `Fuzzr.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4)`")]
	public void WorksFourArgs()
	{
		var generator =
			from i1 in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from i2 in Configr<SomeThing>.Ignore(a => a.AnInt2)
			from i3 in Configr<SomeThing>.Ignore(a => a.AnInt3)
			from i4 in Configr<SomeThing>.Ignore(a => a.AnInt4)
			from c in Configr<SomeThing>.Construct(Fuzzr.Constant(42), Fuzzr.Constant(43), Fuzzr.Constant(44), Fuzzr.Constant(45))
			from r in Fuzzr.One<SomeThing>()
			select r;
		var result = generator.Generate();
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
		Assert.Equal(45, result.AnInt4);
	}

	[Fact]
	[DocContent(@" -  `Fuzzr.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4, Generator<T5> g5)`  

After that, ... you're on your own.")]
	public void WorksFiveArgs()
	{
		var generator =
			from i1 in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from i2 in Configr<SomeThing>.Ignore(a => a.AnInt2)
			from i3 in Configr<SomeThing>.Ignore(a => a.AnInt3)
			from i4 in Configr<SomeThing>.Ignore(a => a.AnInt4)
			from i5 in Configr<SomeThing>.Ignore(a => a.AString)
			from c in Configr<SomeThing>.Construct(Fuzzr.Constant(42), Fuzzr.Constant(43), Fuzzr.Constant(44), Fuzzr.Constant(45), Fuzzr.Constant("answer"))
			from r in Fuzzr.One<SomeThing>()
			select r;
		var result = generator.Generate();
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
		Assert.Equal(45, result.AnInt4);
		Assert.Equal("answer", result.AString);
	}

	[Fact]
	[DocContent(@"Or use the factory method overload:  
`Fuzzr.For<T>().Construct<T>(Func<T> ctor)`")]
	public void FactoryCtor()
	{
		var generator =
			from i1 in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from i2 in Configr<SomeThing>.Ignore(a => a.AnInt2)
			from i3 in Configr<SomeThing>.Ignore(a => a.AnInt3)
			from i4 in Configr<SomeThing>.Ignore(a => a.AnInt4)
			from i5 in Configr<SomeThing>.Ignore(a => a.AString)
			from c in Configr<SomeThing>.Construct(() => new SomeThing(1, 2, 3, 4, "5"))
			from r in Fuzzr.One<SomeThing>()
			select r;
		var result = generator.Generate();
		Assert.Equal(1, result.AnInt1);
		Assert.Equal(2, result.AnInt2);
		Assert.Equal(3, result.AnInt3);
		Assert.Equal(4, result.AnInt4);
		Assert.Equal("5", result.AString);
	}

	public class SomeThing
	{
		public int? AnInt1 { get; private set; }
		public int? AnInt2 { get; private set; }
		public int? AnInt3 { get; private set; }
		public int? AnInt4 { get; private set; }
		public string? AString { get; private set; }

		public SomeThing() { }

		public SomeThing(int anInt1)
		{
			AnInt1 = anInt1;
		}

		public SomeThing(int anInt1, int anInt2)
		{
			AnInt1 = anInt1;
			AnInt2 = anInt2;
		}

		public SomeThing(int anInt1, int anInt2, int anInt3)
		{
			AnInt1 = anInt1;
			AnInt2 = anInt2;
			AnInt3 = anInt3;
		}

		public SomeThing(int anInt1, int anInt2, int anInt3, int anInt4)
		{
			AnInt1 = anInt1;
			AnInt2 = anInt2;
			AnInt3 = anInt3;
			AnInt4 = anInt4;
		}

		public SomeThing(int anInt1, int anInt2, int anInt3, int anInt4, string aString)
		{
			AnInt1 = anInt1;
			AnInt2 = anInt2;
			AnInt3 = anInt3;
			AnInt4 = anInt4;
			AString = aString;
		}
	}
}