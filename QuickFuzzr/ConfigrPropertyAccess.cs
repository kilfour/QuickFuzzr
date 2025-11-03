using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

[Flags]
public enum PropertyAccess
{
    None = 0,

    // Standard setters
    PublicSetters = 1,
    InternalSetters = 2,
    ProtectedSetters = 4,
    PrivateSetters = 8,

    // Special cases
    InitOnly = 16,
    ReadOnly = 32,        // True read-only (get-only)
    Calculated = 64,      // No setter, just get

    // Groups
    AllSetters = PublicSetters | InternalSetters | ProtectedSetters | PrivateSetters,
    AllWritable = AllSetters | InitOnly,
    All = AllWritable | ReadOnly | Calculated
}

public static partial class Configr
{
    public static FuzzrOf<Intent> EnablePropertyAccessFor(PropertyAccess propertyAccess) =>
        state =>
        {
            state.PropertyAccess |= propertyAccess;
            return Result.Unit(state);
        };

    public static FuzzrOf<Intent> DisablePropertyAccessFor(PropertyAccess propertyAccess) =>
        state =>
        {
            state.PropertyAccess &= ~propertyAccess;
            return Result.Unit(state);
        };
}