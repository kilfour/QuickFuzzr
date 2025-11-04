// namespace QuickFuzzr.Tests._Tools.Models;

// public class PropertyShowcase
// {
//     // The usual suspect
//     public string PublicSetter { get; set; } = string.Empty;

//     // Common patterns
//     public string PrivateSetter { get; private set; } = string.Empty;
//     public string InitOnly { get; init; } = string.Empty;
//     public string ReadOnly { get; } = "fixed";

//     // Less common but important
//     public string ProtectedSetter { get; protected set; } = string.Empty;
//     public string InternalSetter { get; internal set; } = string.Empty;

//     // The edge cases
//     private string _privateProperty = "hidden";
//     public string PrivateProperty => _privateProperty;

//     // Calculated properties
//     public string Calculated => $"{PublicSetter} {PrivateSetter}";

//     // Fields (if you support them)
//     public string PublicField = "field";
//     private string _privateField = "private";
// }


// // Record with mixed accessibility - correct syntax
// public record EmployeeRecord(string Name, int Age)
// {
//     // Additional properties with different access
//     public string Email { get; set; } = string.Empty;           // Mutable
//     public string Department { get; private set; } = string.Empty;  // Private setter
//     public DateTime Created { get; } = DateTime.UtcNow;        // Read-only
//     public string InitOnlyExtra { get; init; } = string.Empty; // Init-only

//     // Private backing field scenario
//     private string _secretCode = "default";
//     public string SecretCode => _secretCode;
// }

// public record ComplexRecord(
//     string RequiredName,                    // Init-only positional
//     int RequiredAge                         // Init-only positional
// )
// {
//     // Every type of property in one record
//     public string PublicMutable { get; set; } = "mutable";
//     public string PrivateMutable { get; private set; } = "private_set";
//     public string InitOnlyProp { get; init; } = "init_only";
//     public string ReadOnlyProp { get; } = "read_only";
//     public string ProtectedSetter { get; protected set; } = "protected";
//     public string InternalSetter { get; internal set; } = "internal";

//     // Calculated
//     public string Display => $"{RequiredName} ({RequiredAge})";

//     // With private backing
//     private Guid _internalId = Guid.NewGuid();
//     public Guid InternalId => _internalId;
// }