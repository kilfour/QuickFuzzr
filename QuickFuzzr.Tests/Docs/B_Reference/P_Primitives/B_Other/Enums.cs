using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.B_Other;

[DocFile]
[DocContent("Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.")]
[DocContent("> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Randomly selects a defined member of an enum type.")]
public class Enums : Primitive<Enums.MyEnumeration>
{
	protected override FuzzrOf<MyEnumeration> CreateFuzzr() => Fuzzr.Enum<MyEnumeration>();

	[Fact]
	[DocContent("- The default Fuzzr just picks a random value from all enumeration values.")]
	public void DefaultFuzzr()
	{
		CheckIf.TheseValuesAreGenerated(Fuzzr.Enum<MyEnumeration>(),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	[DocContent("- Passing in a non Enum type for T throws an ArgumentException.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Enum<int>().Generate());
	}

	[Fact]
	public override void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzzr.One<PrimitivesBag<MyEnumeration>>().Select(x => x.Value), MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	public enum MyEnumeration { MyOne, Mytwo }
}