using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("TimeOnlys")]
[DocContent("Use `Fuzzr.TimeOnly()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random times between midnight and 23:59:59.")]
public class TimeOnlys
{
	[Fact]
	[DocContent("- The overload `Fuzzr.TimeOnly(TimeOnly min, TimeOnly max)` generates a TimeOnly greater than or equal to `min` and less than `max`.")]
	public void Zero()
	{
		var fuzzr = Fuzzr.TimeOnly(new TimeOnly(1, 0), new TimeOnly(1, 1));
		for (int i = 0; i < 10; i++)
		{
			var value = fuzzr.Generate();
			Assert.True(value >= new TimeOnly(1, 0));
			Assert.True(value < new TimeOnly(1, 1));
		}
	}

	[Fact]
	[DocContent("- **Default:** min = 00:00:00, max = 23:59:59.9999999).")]
	public void DefaultFuzzrNeverGeneratesZero()
	{
		var fuzzr = Fuzzr.TimeOnly();
		for (int i = 0; i < 50; i++)
		{
			var val = fuzzr.Generate();
			Assert.True(val >= new TimeOnly(0));
			Assert.True(val <= TimeOnly.MaxValue);
		}
	}

	[Fact]
	public void Nullable()
	{
		var fuzzr = Fuzzr.TimeOnly().Nullable();
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 50; i++)
		{
			var value = fuzzr.Generate();
			if (value.HasValue)
			{
				isSomeTimesNotNull = true;
				Assert.NotEqual(new TimeOnly(), value.Value);
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
			Assert.NotEqual(new TimeOnly(), fuzzr.Generate().AProperty);
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
				Assert.NotEqual(new TimeOnly(), value.Value);
			}
			else
				isSomeTimesNull = true;
		}
		Assert.True(isSomeTimesNull);
		Assert.True(isSomeTimesNotNull);
	}

	public class SomeThingToGenerate
	{
		public TimeOnly AProperty { get; set; }
		public TimeOnly? ANullableProperty { get; set; }
	}
}