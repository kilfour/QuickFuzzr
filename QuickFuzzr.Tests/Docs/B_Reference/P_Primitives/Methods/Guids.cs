using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Guid()`. *There is no overload.*")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces non-empty random `Guid` values.")]
public class Guids
{
	[Fact]
	[DocContent("- The default fuzzr never generates Guid.Empty.")]
	public void NeverGuidEmpty()
	{
		var fuzzr = Fuzzr.Guid();
		for (int i = 0; i < 10; i++)
		{
			var val = fuzzr.Generate();
			Assert.NotEqual(Guid.Empty, val);
		}
	}

	[Fact]
	[DocContent("- `Fuzzr.Guid()` is deterministic when seeded.")]
	public void DeterministicWithSeed()
	{
		Assert.Equal("96ba173e-04ae-3bcd-9986-9e56f0adbf3a", Fuzzr.Guid().Generate(42).ToString().ToLower());
	}

	[Fact]
	[DocContent("- Can be made to return `Guid?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var fuzzr = Fuzzr.Guid().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(Guid.Empty, value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `Guid` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(Guid.Empty, fuzzr.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `Guid?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate().ANullableProperty;
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(Guid.Empty, value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public Guid AProperty { get; set; }
		public Guid? ANullableProperty { get; set; }
	}
}