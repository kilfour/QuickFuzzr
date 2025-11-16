using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>.Construct")]
[DocColumn(Configuring.Columns.Description, "Registers which constructor QuickFuzzr should use for type T.")]
[DocContent(
@"Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
")]
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
	public void Null_Arg1()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int>(null!));
		Assert.Equal(Null_Arg_Message(), ex.Message);
	}

	[Fact]
	public void Null_Arg2()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int>(
				null!, Fuzzr.Constant(42)));
		Assert.Equal(Null_Arg_Message("1"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int>(
				Fuzzr.Constant(42), null!));
		Assert.Equal(Null_Arg_Message("2"), ex.Message);
	}

	[Fact]
	public void Null_Arg3()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int>(
				null!, Fuzzr.Constant(42), Fuzzr.Constant(42)));
		Assert.Equal(Null_Arg_Message("1"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int>(
				Fuzzr.Constant(42), null!, Fuzzr.Constant(42)));
		Assert.Equal(Null_Arg_Message("2"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int>(
				Fuzzr.Constant(42), Fuzzr.Constant(42), null!));
		Assert.Equal(Null_Arg_Message("3"), ex.Message);
	}

	[Fact]
	public void Null_Arg4()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int>(
				null!, Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant(42)));
		Assert.Equal(Null_Arg_Message("1"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int>(
				Fuzzr.Constant(42), null!, Fuzzr.Constant(42), Fuzzr.Constant(42)));
		Assert.Equal(Null_Arg_Message("2"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int>(
				Fuzzr.Constant(42), Fuzzr.Constant(42), null!, Fuzzr.Constant(42)));
		Assert.Equal(Null_Arg_Message("3"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int>(
				Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant(42), null!));
		Assert.Equal(Null_Arg_Message("4"), ex.Message);
	}

	[Fact]
	public void Null_Arg5()
	{
		var ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int, string>(
				null!, Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant("answer")));
		Assert.Equal(Null_Arg_Message("1"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int, string>(
				Fuzzr.Constant(42), null!, Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant("answer")));
		Assert.Equal(Null_Arg_Message("2"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int, string>(
				Fuzzr.Constant(42), Fuzzr.Constant(42), null!, Fuzzr.Constant(42), Fuzzr.Constant("answer")));
		Assert.Equal(Null_Arg_Message("3"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int, string>(
				Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant(42), null!, Fuzzr.Constant("answer")));
		Assert.Equal(Null_Arg_Message("4"), ex.Message);
		ex = Assert.Throws<ArgumentNullException>(
			() => Configr<MultiCtorContainer>.Construct<int, int, int, int, string>(
				Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant(42), Fuzzr.Constant(42), null!));
		Assert.Equal(Null_Arg_Message("5"), ex.Message);
	}

	private static string Null_Arg_Message(string suffix = "")
		=> $"Value cannot be null. (Parameter 'fuzzr{suffix}')";

	[Fact]
	[DocException("ConstructorNotFoundException", "If no matching constructor is found on type T.")]
	public void No_Such_Ctor()
	{
		var cfg = Configr<MultiCtorContainer>.Construct(Fuzzr.Constant("nope"));
		var ex = Assert.Throws<ConstructorNotFoundException>(() => Generate(cfg));
		Assert.Equal(No_Such_Ctor_Message(), ex.Message);
	}

	private static string No_Such_Ctor_Message() =>
@"Cannot construct instance of MultiCtorContainer.
No matching constructor found for argument type: (String).

Possible solutions:
- Add a constructor with the required signature.
- Verify the argument types order is correct.
- Update Configr<Person>.Construct(...) to match an existing constructor.
";

	[Fact]
	public void Multiple_Configrs()
	{
		var fuzzr =
			from c1 in Configr<MultiCtorContainer>.Construct(Fuzzr.Constant(42))
			from container1 in Fuzzr.One<MultiCtorContainer>()
			from c2 in Configr<MultiCtorContainer>.Construct(Fuzzr.Constant(43), Fuzzr.Constant(44))
			from container2 in Fuzzr.One<MultiCtorContainer>()
			select (container1, container2);
		var (first, second) = fuzzr.Generate();
		Assert.Equal(42, first.AnInt1);
		Assert.Null(first.AnInt2);
		Assert.Equal(43, second.AnInt1);
		Assert.Equal(44, second.AnInt2);
	}
}