using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("DateTimes")]
[DocContent("Use `Fuzzr.DateTime()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces `DateTime` values between 1970-01-01 and 2020-12-31 (inclusive), snapped to whole seconds.")]
public class DateTimes
{
	[Fact]
	[DocContent("- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a `DateTime` in the inclusive range [min, max], snapped to whole seconds.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.DateTime(new DateTime(2000, 1, 1), new DateTime(2000, 1, 5)),
			("value >= new DateTime(2000, 1, 1)", a => a >= new DateTime(2000, 1, 1)),
			("value <= new DateTime(2000, 1, 5)", a => a <= new DateTime(2000, 1, 5)));

	[Fact]
	public void MinMaxShouldGenerateBounds()
		=> CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.DateTime(new DateTime(1), new DateTime(2)),
			("value == new DateTime(1)", a => a == new DateTime(1)),
			("value == new DateTime(2)", a => a == new DateTime(2)));

	[Fact]
	public void MinMaxShouldGenerateSecondsBounds()
		=> CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.DateTime(new DateTime(1, 1, 1, 1, 1, 1), new DateTime(1, 1, 1, 1, 1, 2)),
			("value == new DateTime(1, 1, 1, 1, 1, 1)", a => a == new DateTime(1, 1, 1, 1, 1, 1)),
			("value == new DateTime(1, 1, 1, 1, 1, 2)", a => a == new DateTime(1, 1, 1, 1, 1, 2)));

	[Fact]
	[DocContent("- Generated values are snapped to whole seconds.")]
	public void MinMaxShouldSnapToSeconds()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.DateTime(new DateTime(2000, 1, 1), new DateTime(2000, 1, 5)),
			("value.Millisecond == 0", a => a.Millisecond == 0),
			("value.Nanosecond == 0", a => a.Nanosecond == 0));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.DateTime(new DateTime(2000, 1, 5), new DateTime(2000, 1, 1)).Generate());

	[Fact]
	[DocContent("- The default fuzzr is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)) inclusive, snapped to whole seconds.")]
	public void DefaultFuzzrNeverGeneratesZero()
	{
		var fuzzr = Fuzzr.DateTime();
		for (int i = 0; i < 50; i++)
		{
			var val = fuzzr.Generate();
			Assert.True(val >= new DateTime(1970, 1, 1));
			Assert.True(val < new DateTime(2020, 12, 31));
		}
	}

	[Fact]
	[DocContent("- Can be made to return `DateTime?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		var fuzzr = Fuzzr.DateTime().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(new DateTime(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	[Fact]
	[DocContent("- `DateTime` is automatically detected and generated for object properties.")]
	public void Property()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>();
		for (int i = 0; i < 10; i++)
		{
			Assert.NotEqual(new DateTime(), fuzzr.Generate().AProperty);
		}
	}

	[Fact]
	[DocContent("- `DateTime?` is automatically detected and generated for object properties.")]
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
				Assert.NotEqual(new DateTime(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public DateTime AProperty { get; set; }
		public DateTime? ANullableProperty { get; set; }
	}
}
