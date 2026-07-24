using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools.Models;

[CodeExample]
public record HeapNode(int Value, HeapNode? Left, HeapNode? Right);
