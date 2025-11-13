using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;
using WibblyWobbly;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("DateOnlys")]
[DocContent("Use `Fuzzr.DateOnly()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default).")]
public class DateOnlys
{
	[Fact]
	[DocContent("- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly greater than or equal to `min` and less than or equal to `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.DateOnly(new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 5)),
			("value >= new DateOnly(2000, 1, 1)", a => a >= new DateOnly(2000, 1, 1)),
			("value <= new DateOnly(2000, 1, 5)", a => a <= new DateOnly(2000, 1, 5)));

	[Fact]
	public void MinMaxShouldGenerateBounds()
		=> CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.DateOnly(1.January(2000), 2.January(2000)),
			("value == 1.January(2000)", a => a == 1.January(2000)),
			("value == 2.January(2000)", a => a == 2.January(2000)));

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
			Assert.True(val <= new DateOnly(2020, 12, 31));
		}
	}

	[Fact]
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
	public void Property()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(new DateOnly(), fuzzr.Generate().AProperty);
		}
	}

	[Fact]
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