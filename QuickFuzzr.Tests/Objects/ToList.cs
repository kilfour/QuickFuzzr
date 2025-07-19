namespace QuickFuzzr.Tests.Objects
{
	[ToList(
		Content = "Use The `.ToList()` generator extension.",
		Order = 0)]
	public class ToList
	{
		[Fact]
		[ToList(
			Content =
@"Similar to the `ToArray` method. But instead of an Array, this one returns a, you guessed it, List. 
",
			Order = 1)]
		public void SameValues()
		{
			var values =
				(from ints in Fuzz.Int().Many(2).ToList()
				 from one in Fuzz.Constant(ints)
				 from two in Fuzz.Constant(ints)
				 select new { one, two })
				.Generate();
			Assert.IsType<List<int>>(values.one);
			Assert.Equal(values.one.ElementAt(0), values.two.ElementAt(0));
			Assert.Equal(values.one.ElementAt(1), values.two.ElementAt(1));
		}

		public class SomeThingToGenerate { }

		public class ToListAttribute : GeneratingObjectsAttribute
		{
			public ToListAttribute()
			{
				Caption = "ToList.";
				CaptionOrder = 11;
			}
		}
	}
}