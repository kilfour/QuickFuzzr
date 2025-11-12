using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr&lt;T&gt;.Construct(FuzzrOf&lt;T1&gt; arg1)")]
[DocContent(
@"Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
")] // Check: check multiple constructors random selection

public class ConfigrConstructT
{

	[CodeSnippet]
	[CodeRemove("return ")]
	private static FuzzrOf<Intent> GetConfig()
	{
		return Configr<SomeThing>.Construct(Fuzzr.Constant(42));
	}

	private static SomeThing Generate(FuzzrOf<Intent> config)
	{
		var fuzzr =
			from ignore in Configr.IgnoreAll()
			from cfg in config
			from thing in Fuzzr.One<SomeThing>()
			select thing;
		return fuzzr.Generate();
	}

	[Fact]
	[DocUsage]
	[DocExample(typeof(ConfigrConstructT), nameof(GetConfig))]
	public void Works() =>
		Assert.Equal(42, Generate(GetConfig()).AnInt1);

	[Fact]
	[DocOverloads]
	[DocCode("Construct<T1,T2>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2)")]
	public void TwoArgs()
	{
		var cfg = Configr<SomeThing>.Construct(
			Fuzzr.Constant(42),
			Fuzzr.Constant(43));
		var result = Generate(cfg);
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
	}

	[Fact]
	[DocCode("Construct<T1,T2,T3>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3)")]
	public void ThreeArgs()
	{
		var cfg = Configr<SomeThing>.Construct(
			Fuzzr.Constant(42),
			Fuzzr.Constant(43),
			Fuzzr.Constant(44));
		var result = Generate(cfg);
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
	}

	[Fact]
	[DocCode("Construct<T1,T2,T3,T4>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4)")]
	public void FourArgs()
	{
		var cfg = Configr<SomeThing>.Construct(
			Fuzzr.Constant(42),
			Fuzzr.Constant(43),
			Fuzzr.Constant(44),
			Fuzzr.Constant(45));
		var result = Generate(cfg);
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
		Assert.Equal(45, result.AnInt4);
	}

	[Fact]
	[DocCode("Construct<T1,T2,T3,T4,T5>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4, FuzzrOf<T5> arg5)")]
	public void FiveArgs()
	{
		var cfg = Configr<SomeThing>.Construct(
			Fuzzr.Constant(42),
			Fuzzr.Constant(43),
			Fuzzr.Constant(44),
			Fuzzr.Constant(45),
			Fuzzr.Constant("answer"));
		var result = Generate(cfg);
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
		Assert.Equal(45, result.AnInt4);
		Assert.Equal("answer", result.AString);
	}

	[Fact]
	[DocExceptions]
	[DocException("ArgumentNullException", "If one of the `TArg` parameters is null.")]
	public void Null_Arg()
	{
		var cfg = Configr<SomeThing>.Construct<int>(null!);
		var ex = Assert.Throws<NullReferenceException>(() => Generate(cfg));
		Assert.Equal(Null_Arg_Message(), ex.Message);
	}

	private static string Null_Arg_Message() =>
@"Object reference not set to an instance of an object."; // Check: Update Message

	[Fact]
	[DocException("InvalidOperationException", "If no matching constructor is found on type T.")]
	public void No_Such_Ctor()
	{
		var cfg = Configr<SomeThing>.Construct(Fuzzr.Constant("nope"));
		var ex = Assert.Throws<InvalidOperationException>(() => Generate(cfg));
		Assert.Equal(No_Such_Ctor_Message(), ex.Message);
	}

	private static string No_Such_Ctor_Message() =>
@"No constructor found on QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods.ConfigrConstructT+SomeThing with args (String).";

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