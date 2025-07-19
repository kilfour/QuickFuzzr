namespace QuickFuzzr.Tests.Combining
{
	[UsingExtensions(
		Content = "When applying the various extension methods onto a generator, they get *combined* into a new generator.",
		Order = 0)]
	public class UsingExtensions
	{
		[Fact]
		[UsingExtensions(
			Content =
@"Jumping slightly ahead of ourselves as below example will use methods that are explained more thoroughly further below.

E.g. :
```
Fuzz.ChooseFrom(someValues).Unique(""key"").Many(2)
```
",
			Order = 1)]
		public void SimpleCombination()
		{
			var generator =
				from a in Fuzz.ChooseFromThese(1, 2).Unique("key").Many(2)
				select a;
			for (int i = 0; i < 10; i++)
			{
				var values = generator.Generate().ToArray();
				Assert.Equal(values[0] == 1 ? 2 : 1, values[1]);
			}
		}

		public class UsingExtensionsAttribute : CombiningGeneratorsAttribute
		{
			public UsingExtensionsAttribute()
			{
				Caption = "Using Extensions.";
				CaptionOrder = 1;
			}
		}
	}
}