using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools.Models;

[CodeExample]
public class Folder
{
    public string Name { get; set; } = default!;
    public Folder? SubFolder { get; set; }
}

[CodeExample]
public abstract class FileSystemEntry
{
    public string Name { get; set; } = string.Empty;
}

[CodeExample]
public class FileEntry : FileSystemEntry { }

[CodeExample]
public class FolderEntry : FileSystemEntry
{
    public List<FileEntry> Files { get; set; } = [];
    public List<FolderEntry> Folders { get; set; } = [];

}
