using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>.Construct")]
[DocColumn(Configuring.Columns.Description, "Registers which constructor QuickFuzzr should use for type T.")]
[DocContent(
@"Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
")] // Check: check multiple constructors random selection
[DocSignature("Configr<T>.Construct(FuzzrOf<T1> arg1);")]
public class G_ConfigrConstructT
{

	[CodeSnippet]
	[CodeRemove("return ")]
	private static FuzzrOf<Intent> GetConfig()
	{
		return Configr<MultiCtorContainer>.Construct(Fuzzr.Constant(42));
	}

	private static MultiCtorContainer Generate(FuzzrOf<Intent> config)
	{
		var fuzzr =
			from ignore in Configr.IgnoreAll()
			from cfg in config
			from thing in Fuzzr.One<MultiCtorContainer>()
			select thing;
		return fuzzr.Generate();
	}

	[Fact]
	[DocUsage]
	[DocExample(typeof(G_ConfigrConstructT), nameof(GetConfig))]
	public void Works() =>
		Assert.Equal(42, Generate(GetConfig()).AnInt1);

	[Fact]
	[DocOverloads]
	[DocOverload("Construct<T1, T2>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2)")]
	public void TwoArgs()
	{
		var cfg = Configr<MultiCtorContainer>.Construct(
			Fuzzr.Constant(42),
			Fuzzr.Constant(43));
		var result = Generate(cfg);
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
	}

	[Fact]
	[DocOverload("Construct<T1, T2, T3>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3)")]
	public void ThreeArgs()
	{
		var cfg = Configr<MultiCtorContainer>.Construct(
			Fuzzr.Constant(42),
			Fuzzr.Constant(43),
			Fuzzr.Constant(44));
		var result = Generate(cfg);
		Assert.Equal(42, result.AnInt1);
		Assert.Equal(43, result.AnInt2);
		Assert.Equal(44, result.AnInt3);
	}

	[Fact]
	[DocOverload("Construct<T1, T2, T3, T4>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4)")]
	public void FourArgs()
	{
		var cfg = Configr<MultiCtorContainer>.Construct(
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
	[DocOverload("Construct<T1, T2, T3, T4, T5>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4, FuzzrOf<T5> arg5)")]
	public void FiveArgs()
	{
		var cfg = Configr<MultiCtorContainer>.Construct(
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
	public void Null_Arg() // TODO : test all versions
	{
		var ex = Assert.Throws<ArgumentNullException>(() => Configr<MultiCtorContainer>.Construct<int>(null!));
		Assert.Equal(Null_Arg_Message(), ex.Message);
	}

	private static string Null_Arg_Message() => "Value cannot be null. (Parameter 'fuzzr')";

	[Fact]
	[DocException("InvalidOperationException", "If no matching constructor is found on type T.")]
	public void No_Such_Ctor()
	{
		var cfg = Configr<MultiCtorContainer>.Construct(Fuzzr.Constant("nope"));
		var ex = Assert.Throws<InvalidOperationException>(() => Generate(cfg));
		Assert.Equal(No_Such_Ctor_Message(), ex.Message);
	}

	private static string No_Such_Ctor_Message() =>
@"No constructor found on MultiCtorContainer with args (String)."; // Check: Update Message
}