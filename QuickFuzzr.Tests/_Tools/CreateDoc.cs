using System.Text;
using QuickFuzzr;
using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests._Tools;

public class CreateDoc
{
	[Fact]
	public void Now()
	{
		new Document().ToFile("README-TEMP.md", typeof(CreateDoc).Assembly);
	}

	[Fact(Skip = "not now")]
	public void Go()
	{
		var typeattributes =
			typeof(CreateDoc).Assembly
				.GetTypes()
				.SelectMany(t => t.GetCustomAttributes(typeof(DocAttribute), false));

		var methodattributes =
			typeof(CreateDoc).Assembly
				.GetTypes()
				.SelectMany(t => t.GetMethods())
				.SelectMany(t => t.GetCustomAttributes(typeof(DocAttribute), false));

		var attributes = typeattributes.Union(methodattributes)
				.Cast<DocAttribute>();

		var chapters = attributes.OrderBy(a => a.ChapterOrder).Select(a => a.Chapter).Distinct();
		var sb = new StringBuilder();
		//sb.AppendLine(Introduction);
		foreach (var chapter in chapters)
		{
			sb.AppendFormat("## {0}", chapter);
			sb.AppendLine();
			var chapterAttributes = attributes.Where(a => a.Chapter == chapter);
			var captions = chapterAttributes.OrderBy(a => a.CaptionOrder).Select(a => a.Caption).Distinct();
			foreach (var caption in captions)
			{
				sb.AppendFormat("### {0}", caption);
				sb.AppendLine();
				foreach (var attribute in chapterAttributes.Where(a => a.Caption == caption).OrderBy(a => a.Order))
				{
					sb.AppendLine(attribute.Content);
					sb.AppendLine();
				}
				sb.AppendLine();
			}
			sb.AppendLine();
			sb.AppendLine("___");
		}
		using (var writer = new StreamWriter("../../../../README.md", false))
			writer.Write(sb.ToString());
	}

	private const string Introduction =
@"# QuickFuzzr 
> A type-walking cheetah with a hand full of random. 
## Introduction
An evolution from the QuickMGenerate library.
## Installation

QuickFuzzrQuickFuzzr is available on NuGet:

```bash
Install-Package QuickFuzzr
```

Or via the .NET CLI:

```bash
dotnet add package QuickFuzzr
```
";
}
