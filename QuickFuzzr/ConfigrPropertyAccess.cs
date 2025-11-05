using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

/// <summary>
/// Defines flags for controlling which property access levels should be included during automatic generation.
/// Use to fine-tune property population behavior based on accessibility modifiers and property characteristics.
/// </summary>
[Flags]
public enum PropertyAccess
{
    /// <summary>No properties will be auto-generated.</summary>
    None = 0,
    /// <summary>Includes properties with public setters.</summary>
    PublicSetters = 1,
    /// <summary>Includes properties with internal setters.</summary>
    InternalSetters = 2,
    /// <summary>Includes properties with protected setters.</summary>
    ProtectedSetters = 4,
    /// <summary>Includes properties with private setters.</summary>
    PrivateSetters = 8,
    /// <summary>Includes properties with init-only setters.</summary>
    InitOnly = 16,
    /// <summary>Includes read-only properties (getters only).</summary>
    ReadOnly = 32,
    /// <summary>Includes calculated properties (no backing field).</summary>
    Calculated = 64,
    /// <summary>Includes all properties with any setter accessibility.</summary>
    AllSetters = PublicSetters | InternalSetters | ProtectedSetters | PrivateSetters,
    /// <summary>Includes all writable properties including init-only.</summary>
    AllWritable = AllSetters | InitOnly,
    /// <summary>Includes all properties regardless of accessibility.</summary>
    All = AllWritable | ReadOnly | Calculated
}

public static partial class Configr
{
    /// <summary>
    /// Creates a generator that enables property generation for the specified access levels.
    /// Use to expand auto-generation to include properties with specific accessibility like init-only, internal, or private setters.
    /// </summary>
    public static FuzzrOf<Intent> EnablePropertyAccessFor(PropertyAccess propertyAccess) =>
        state =>
        {
            state.PropertyAccess |= propertyAccess;
            return Result.Unit(state);
        };

    /// <summary>
    /// Creates a generator that disables property generation for the specified access levels.
    /// Use to restrict auto-generation by excluding properties with specific accessibility like read-only or calculated properties.
    /// </summary>
    public static FuzzrOf<Intent> DisablePropertyAccessFor(PropertyAccess propertyAccess) =>
        state =>
        {
            state.PropertyAccess &= ~propertyAccess;
            return Result.Unit(state);
        };
}