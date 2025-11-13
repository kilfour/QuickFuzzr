using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("DateOnlys")]
[DocContent("Use `Fuzzr.DateOnly()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default).")]
public class DateOnlys
{
	[Fact]
	[DocContent("- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var fuzzr = Fuzzr.DateOnly(new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 5));
		for (int i = 0; i < 10; i++)
		{
			var value = fuzzr.Generate();
			Assert.True(value >= new DateOnly(2000, 1, 1));
			Assert.True(value < new DateOnly(2000, 1, 5));
		}
	}

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.DateOnly(new DateOnly(2000, 1, 5), new DateOnly(2000, 1, 1)).Generate());


	[Fact]
	[DocContent("- The default fuzzr is (min = new DateOnly(1970, 1, 1), max = new DateOnly(2020, 12, 31)).")]
	public void DefaultFuzzrNeverGeneratesZero()
	{
		var fuzzr = Fuzzr.DateOnly();
		for (int i = 0; i < 50; i++)
		{
			var val = fuzzr.Generate();
			Assert.True(val >= new DateOnly(1970, 1, 1));
			Assert.True(val < new DateOnly(2020, 12, 31));
		}
	}

	[Fact]
	[DocContent("- Can be made to return `DateOnly?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var fuzzr = Fuzzr.DateOnly().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(new DateOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `DateOnly` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(new DateOnly(), fuzzr.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `DateOnly?` is automatically detected and generated for object properties.")]
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
				Assert.NotEqual(new DateOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public DateOnly AProperty { get; set; }
		public DateOnly? ANullableProperty { get; set; }
	}
}