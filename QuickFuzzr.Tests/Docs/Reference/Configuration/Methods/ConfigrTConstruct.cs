using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Configuration.Methods;

[DocFile]
[DocFileHeader("Configr<T>.Construct(...)")]
public class ConfigrTConstruct
{
	[DocContent("**Usage:**")]
	[DocExample(typeof(ConfigrTConstruct), nameof(GetConfig))]
	[CodeSnippet]
	[CodeRemove("return")]
	private static FuzzrOf<Intent> GetConfig()
	{
		return Configr<SomeThing>.Construct(Fuzzr.Constant(42));
	}

	[Fact]
	[DocContent("Subsequent calls to `Fuzzr.One<T>()` will then use the registered constructor.")]
	public void Works()
	{
		var generator =
			from i in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from c in GetConfig()
			from r in Fuzzr.One<SomeThing>()
			select r;
		Assert.Equal(42, generator.Generate().AnInt1);
	}

	[Fact]
	[DocContent("Various overloads exist that allow for up to five constructor arguments.")]
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
	[DocContent("\nAfter that, ... you're on your own.")]
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

	[CodeSnippet]
	[CodeRemove("return")]
	private static FuzzrOf<Intent> GetFactoryConfig()
	{
		return Configr<SomeThing>.Construct(() => new SomeThing(1, 2, 3, 4, "5"));
	}

	[Fact]
	[DocContent("Or use the factory method overload: ")]
	[DocExample(typeof(ConfigrTConstruct), nameof(GetFactoryConfig))]
	public void FactoryCtor()
	{
		var generator =
			from i1 in Configr<SomeThing>.Ignore(a => a.AnInt1)
			from i2 in Configr<SomeThing>.Ignore(a => a.AnInt2)
			from i3 in Configr<SomeThing>.Ignore(a => a.AnInt3)
			from i4 in Configr<SomeThing>.Ignore(a => a.AnInt4)
			from i5 in Configr<SomeThing>.Ignore(a => a.AString)
			from c in GetFactoryConfig()
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