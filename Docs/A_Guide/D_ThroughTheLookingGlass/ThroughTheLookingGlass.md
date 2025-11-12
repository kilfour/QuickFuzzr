# Through the Looking Glass
In the previous chapter we hinted at recursion and depth-control.  

So, let's dive deeper.  

  
Imagine:  
```csharp
public class Folder
{
    public string Name { get; set; } = default!;
    public Folder? SubFolder { get; set; }
}
```
Calling `Fuzzr.One<Folder>().Generate()` results in:  
```text
{  Name: "ljduv", SubFolder: null }
```
You can however influence how deep the rabbit hole goes,
by adding call to `Configr<Folder>.Depth(min, max)`:  
```csharp
from name in Configr<Folder>.Property(a => a.Name,
    from cnt in Fuzzr.Counter("folder") select $"Folder-{cnt}")
from folderDepth in Configr<Folder>.Depth(2, 5)
from folder in Fuzzr.One<Folder>()
select folder;
```
**Output:**  
```text
{
    Name: "Folder-1",
    SubFolder: {
        Name: "Folder-2",
        SubFolder: {
            Name: "Folder-3",
            SubFolder: null
        }
    }
}
```
Neat.  
But we can still go *one step beyond*.  

Consider this small model:
  
```csharp
public abstract class FileSystemEntry
{
    public string Name { get; set; } = string.Empty;
}
```
```csharp
public class FileEntry : FileSystemEntry { }
```
```csharp
public class FolderEntry : FileSystemEntry
{
    public List<FileEntry> Files { get; set; } = [];
    public List<FolderEntry> Folders { get; set; } = [];
}
```
A bit complicated to fuzz, but let's have a go:
  
**Helper Function for Name Properties**  
```csharp
private static FuzzrOf<Intent> GetName<T>(string prefix) where T : FileSystemEntry
{
    return
        from name in Configr<T>.Property(a => a.Name,
            from cnt in Fuzzr.Counter(prefix) select $"{prefix}-{cnt}")
        select Intent.Fixed;
}
```
**Name Properties Configuration**  
```csharp
var nameCfg =
    from filename in GetName<FileEntry>("File")
    from foldername in GetName<FolderEntry>("Folder")
    select Intent.Fixed;
```
**Folder List Properties Configuration**  
```csharp
var folderCfg =
    from files in Configr<FolderEntry>.Property(a => a.Files,
        from fs in Fuzzr.One<FileEntry>().Many(1, 3) select fs.ToList())
    from folders in Configr<FolderEntry>.Property(a => a.Folders,
        from fs in Fuzzr.One<FolderEntry>().Many(1, 3) select fs.ToList())
    select Intent.Fixed;
```
**Combined Fuzzr**  
```csharp
var fuzzr =
    from names in nameCfg
    from lists in folderCfg
    from folderDepth in Configr<FolderEntry>.Depth(1, 3)
    from inheritance in Configr<FileSystemEntry>.AsOneOf(typeof(FolderEntry), typeof(FileEntry))
    from entry in Fuzzr.One<FolderEntry>()
    select entry;
```
**Output:**  
```text
{
    Files: [ { Name: "File-1" }, { Name: "File-2" } ],
    Folders: [
        {
            Files: [ { Name: "File-3" } ],
            Folders: [
                {
                    Files: [ { Name: "File-4" } ],
                    Folders: [ ],
                    Name: "Folder-1"
                },
                {
                    Files: [ { Name: "File-5" } ],
                    Folders: [ ],
                    Name: "Folder-2"
                }
            ],
            Name: "Folder-3"
        },
        {
            Files: [ { Name: "File-6" } ],
            Folders: [
                {
                    Files: [ { Name: "File-7" } ],
                    Folders: [ ],
                    Name: "Folder-4"
                }
            ],
            Name: "Folder-5"
        }
    ],
    Name: "Folder-6"
}
```
At this point QuickFuzzr has *type walked* an object graph that contains itself, stopped at a reasonable depth,
and made sense of collections nested inside collections, with controlled recursion.

Each type involved carries its own depth constraint, and every recursive property or list of child elements
simply burns through that budget one level at a time.
When the counter hits zero, the fuzzr yields null (or an empty list), and the story ends right there.

It also means you can mix these with inheritance and collection combinators.  
And depth is local, not global: one deep branch does not force all others to go equally deep.  
