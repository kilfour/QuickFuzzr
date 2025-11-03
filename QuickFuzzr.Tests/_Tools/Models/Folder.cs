using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools.Models;

[CodeExample]
public class Folder
{
    public string Name { get; set; } = default!;
    public Folder? SubFolder { get; set; }
}
