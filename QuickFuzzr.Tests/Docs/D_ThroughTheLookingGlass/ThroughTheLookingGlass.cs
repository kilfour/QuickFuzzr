using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests.Docs.D_ThroughTheLookingGlass;

[DocFile]
[DocFileHeader("Through the Looking Glass")]
[DocContent(
@"In the previous chapter we hinted at recursion and depth-control.  

So, let's dive deeper.  

")]
public class ThroughTheLookingGlass
{
    [Fact]
    public void Doc()
    {
        Explain.OnlyThis<ThroughTheLookingGlass>("temp.md");
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
@"You can however influence how deep the rabbit hole goes,
by adding call to `Configr<Folder>.Depth(min, max)`:")]
    [DocExample(typeof(ThroughTheLookingGlass), nameof(GoingDeeper_Example))]
    [DocContent("**Output:**")]
    [DocCodeFile("GoingDeeper.txt", "text")]
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
        from name in Configr<Folder>.Property(a => a.Name,
            from cnt in Fuzzr.Counter("folder") select $"Folder-{cnt}")
        from folderDepth in Configr<Folder>.Depth(2, 5)
        from folder in Fuzzr.One<Folder>()
        select folder;
    }

    [Fact]
    [DocContent(
@"Neat.  
But we can still go *one step beyond*.  

Consider this small model:
")]
    [DocExample(typeof(FileSystemEntry))]
    [DocExample(typeof(FileEntry))]
    [DocExample(typeof(FolderEntry))]
    [DocContent("A bit complicated to fuzz, but let's have a go:\n")]
    [DocContent("**Helper Function for Name Properties**")]
    [DocExample(typeof(ThroughTheLookingGlass), nameof(GetName))]
    [DocContent("**Name Properties Configuration**")]
    [DocExample(typeof(ThroughTheLookingGlass), nameof(OneStepBeyond_Name_Cfg))]
    [DocContent("**Folder List Properties Configuration**")]
    [DocExample(typeof(ThroughTheLookingGlass), nameof(OneStepBeyond_Folder_Cfg))]
    [DocContent("**Combined Fuzzr**")]
    [DocExample(typeof(ThroughTheLookingGlass), nameof(OneStepBeyond_Example))]
    [DocContent("**Output:**")]
    [DocCodeFile("OneStepBeyond.txt", "text")]
    [DocContent(
@"At this point QuickFuzzr has *type walked* an object graph that contains itself, stopped at a reasonable depth,
and made sense of collections nested inside collections, with controlled recursion.

Each type involved carries its own depth constraint, and every recursive property or list of child elements
simply burns through that budget one level at a time.
When the counter hits zero, the generator yields null (or an empty list), and the story ends right there.

It also means you can mix these with inheritance and collection combinators.  
And depth is local, not global: one deep branch does not force all others to go equally deep.")]
    public void OneStepBeyond()
    {
        var result = OneStepBeyond_Example(OneStepBeyond_Name_Cfg(), OneStepBeyond_Folder_Cfg()).Generate(494);
        // FileLog.Write("temp.log").Absorb(
        //     Please.AllowMe()
        //         .ToInline<List<FileEntry>>()
        //         .IntroduceThis(result));
        var root = Assert.IsType<FolderEntry>(result);
        Assert.Equal(2, root.Files.Count);
        Assert.Equal(2, root.Folders.Count);

        Assert.Single(root.Folders[0].Files);
        Assert.Equal(2, root.Folders[0].Folders.Count);
        Assert.Single(root.Folders[0].Folders[0].Files);
        Assert.Empty(root.Folders[0].Folders[0].Folders);
        Assert.Single(root.Folders[0].Folders[1].Files);
        Assert.Empty(root.Folders[0].Folders[1].Folders);

        Assert.Single(root.Folders[1].Files);
        Assert.Single(root.Folders[1].Folders);
        Assert.Single(root.Folders[1].Folders[0].Files);
        Assert.Empty(root.Folders[1].Folders[0].Folders);
    }

    [CodeExample]
    private static FuzzrOf<Intent> GetName<T>(string prefix) where T : FileSystemEntry
    {
        return
            from name in Configr<T>.Property(a => a.Name,
                from cnt in Fuzzr.Counter(prefix) select $"{prefix}-{cnt}")
            select Intent.Fixed;
    }

    [CodeSnippet]
    [CodeRemove("return nameCfg;")]
    private static FuzzrOf<Intent> OneStepBeyond_Name_Cfg()
    {
        var nameCfg =
            from filename in GetName<FileEntry>("File")
            from foldername in GetName<FolderEntry>("Folder")
            select Intent.Fixed;
        return nameCfg;
    }

    [CodeSnippet]
    [CodeRemove("return folderCfg;")]
    private static FuzzrOf<Intent> OneStepBeyond_Folder_Cfg()
    {
        var folderCfg =
            from files in Configr<FolderEntry>.Property(a => a.Files,
                from fs in Fuzzr.One<FileEntry>().Many(1, 3) select fs.ToList())
            from folders in Configr<FolderEntry>.Property(a => a.Folders,
                from fs in Fuzzr.One<FolderEntry>().Many(1, 3) select fs.ToList())
            select Intent.Fixed;
        return folderCfg;
    }


    [CodeSnippet]
    [CodeRemove("return fuzzr;")]
    private static FuzzrOf<FileSystemEntry> OneStepBeyond_Example(FuzzrOf<Intent> nameCfg, FuzzrOf<Intent> folderCfg)
    {
        var fuzzr =
            from names in nameCfg
            from lists in folderCfg
            from folderDepth in Configr<FolderEntry>.Depth(1, 3)
            from inheritance in Configr<FileSystemEntry>.AsOneOf(typeof(FolderEntry), typeof(FileEntry))
            from entry in Fuzzr.One<FolderEntry>()
            select entry;
        return fuzzr;
    }
}