using System.Diagnostics.Metrics;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;
using QuickPulse.Instruments;
using QuickPulse.Show;

namespace QuickFuzzr.Tests.Docs.D_ADeepForest;

[DocFile]
[DocContent(
@"In the previous chapter we hinted at recursion and depth-control.  

So, let's dive deeper.  

")]
public class ADeepForest
{
    [Fact]
    public void Doc()
    {
        Explain.OnlyThis<ADeepForest>("temp.md");
    }

    [Fact]
    [DocContent("Imagine:")]
    [DocExample(typeof(Folder))]
    [DocContent("Calling `Fuzzr.One<Folder>().Generate()` results in:")]
    [DocCode("{  Name: \"ljcuu\", SubFolder: null }", "text")]
    public void StartingPoint()
    {
        var result = Fuzzr.One<Folder>().Generate(101);
        Assert.Equal("ljcuu", result.Name);
        Assert.Null(result.SubFolder);
    }

    [Fact]
    [DocContent(
@"You can however influence how deep the rabbit hole goes
by adding call to `Configr<Folder>.Depth(min, max)`:")]
    [DocExample(typeof(ADeepForest), nameof(GoingDeeper_Example))]
    public void GoingDeeper()
    {
        var result = GoingDeeper_Example().Generate(102).PulseToLog("temp.log");
        Assert.Equal("Folder-1", result.Name);
        Assert.NotNull(result.SubFolder);
        Assert.Equal("Folder-2", result.SubFolder.Name);
        Assert.NotNull(result.SubFolder.SubFolder);
        Assert.Equal("Folder-3", result.SubFolder.SubFolder.Name);
        Assert.Null(result.SubFolder.SubFolder.SubFolder);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<Folder> GoingDeeper_Example()
    {
        return
        from name in Configr<Folder>.With(
            Fuzzr.Counter("folder"),
            cnt => Configr<Folder>.Property(a => a.Name, $"Folder-{cnt}"))
        from folderDepth in Configr<Folder>.Depth(2, 5)
        from folder in Fuzzr.One<Folder>()
        select folder;
        // Results in =>
        // {
        //     Name: "Folder-1",
        //     SubFolder: {
        //         Name: "Folder-2",
        //         SubFolder: {
        //             Name: "Folder-3",
        //             SubFolder: null
        //         }
        //     }
        // }
    }

    [Fact]
    [DocContent(
@"Neat.  
But we can still go *one step beyond*.  

Consider:
")]
    [DocExample(typeof(FileSystemEntry))]
    [DocExample(typeof(FolderEntry))]
    [DocExample(typeof(FileEntry))]
    [DocContent("A bit complicated, but let's have a go:")]
    [DocExample(typeof(ADeepForest), nameof(OneStepBeyond_Example))]
    public void OneStepBeyond()
    {
        var result = OneStepBeyond_Example().Generate(1234);
        var logFile = "temp.log";
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));
        Please.AllowMe()
            .ToInline<List<FileEntry>>()
            .IntroduceThis(result)
            .PulseToLog(logFile);
        // Assert.Equal("Folder-1", result.Name);
        // Assert.NotNull(result.SubFolder);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<FileSystemEntry> OneStepBeyond_Example()
    {
        FuzzrOf<Intent> GetName<T>(string prefix) where T : FileSystemEntry =>
            from name in Configr<T>.Property(a => a.Name,
                from cnt in Fuzzr.Counter(prefix) select $"{prefix}-{cnt}")
            select Intent.Fixed;

        var fileCfg =
            from name in GetName<FileEntry>("File")
            select Intent.Fixed;

        var folderCfg =
            from name in GetName<FolderEntry>("Folder")
            from folderDepth in Configr<FolderEntry>.Depth(2, 2)
            from files in Configr<FolderEntry>.Property(
                a => a.Files,
                from fs in Fuzzr.One<FileEntry>().Many(1, 3) select fs.ToList())
            from folders in Configr<FolderEntry>.Property(
                a => a.Folders,
                from fs in Fuzzr.One<FolderEntry>().Many(1, 2) select fs.ToList())
            select Intent.Fixed;

        var fuzzr =
            from _1 in fileCfg
            from _2 in folderCfg
            from inheritance in Configr<FileSystemEntry>.AsOneOf(typeof(FolderEntry), typeof(FileEntry))
            from entry in Fuzzr.One<FolderEntry>()
            select entry;

        return fuzzr;
        // Results in =>
        // {
        //     Name: "Folder-1",
        //     SubFolder: {
        //         Name: "Folder-2",
        //         SubFolder: {
        //             Name: "Folder-3",
        //             SubFolder: null
        //         }
        //     }
        // }
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<FileSystemEntry> OneStepBeyond_Example_Old()
    {
        return
        // from foldername in Configr<FolderEntry>.With(
        //     Fuzzr.Counter("folder"),
        //     cnt => Configr<FolderEntry>.Property(a => a.Name, $"Folder-{cnt}"))
        // from filename in Configr<FileEntry>.With(
        //     Fuzzr.Counter("file"),
        //     cnt => Configr<FileEntry>.Property(a => a.Name, $"File-{cnt}"))
        from foldername in Configr<FileSystemEntry>.Property(a => a.Name, Fuzzr.Counter("folder").AsString())
            //from filename in Configr<FileEntry>.Property(a => a.Name, Fuzzr.Counter("file").AsString())
        from folderDepth in Configr<FolderEntry>.Depth(3, 3)
        from entryInheritance in Configr<FileSystemEntry>.AsOneOf(typeof(FolderEntry), typeof(FileEntry))
            // from files in Configr<FolderEntry>.With(
            //     Fuzzr.One<FileEntry>().Many(0, 3),
            //     f => Configr<FolderEntry>.Property(a => a.Files, [.. f]))
        from _ in Configr<FolderEntry>.Property(a => a.Files, from fs in Fuzzr.One<FileEntry>().Many(0, 3) select fs.ToList())
        from __ in Configr<FolderEntry>.Property(a => a.Folders, from fs in Fuzzr.One<FolderEntry>().Many(0, 3) select fs.ToList())

            // from folders in Configr<FolderEntry>.With(
            //     Fuzzr.One<FolderEntry>().Many(1, 3),
            //     f => Configr<FolderEntry>.Property(a => a.Folders, [.. f]))
        from entry in Fuzzr.One<FolderEntry>()
        select entry;
        // Results in =>
        // {
        //     Name: "Folder-1",
        //     SubFolder: {
        //         Name: "Folder-2",
        //         SubFolder: {
        //             Name: "Folder-3",
        //             SubFolder: null
        //         }
        //     }
        // }
    }
}