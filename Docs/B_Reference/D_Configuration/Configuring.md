# Configuring
`Configr` provides a configuration pipeline to influence how QuickFuzzr builds objects.
Use it to set global defaults, customize properties, control recursion depth,
select derived types, or wire dynamic behaviors that apply when calling `Fuzzr.One<T>()`.
  
## Contents
| Configr| Description |
| -| - |
| [Configr&lt;T&gt;.Ignore](Methods/A_ConfigrIgnoreT.md)| Ignores one specific property on type T during generation. |
| [Configr.Ignore](Methods/B_ConfigrIgnore.md)| Globally ignores all properties matching the predicate. |
| [Configr&lt;T&gt;.IgnoreAll](Methods/C_ConfigrIgnoreAllT.md)| Ignores all properties of type T. |
| [Configr.IgnoreAll](Methods/D_ConfigrIgnoreAll.md)| Disables auto-generation for all properties on all types. |
| [Configr&lt;T&gt;.Property](Methods/E_ConfigrPropertyT.md)| Sets a custom Fuzzr or value for one property on type T. |
| [Configr.Property](Methods/F_ConfigrProperty.md)| Applies a custom Fuzzr or value to all matching properties across all types. |
| [Configr&lt;T&gt;.Construct](Methods/G_ConfigrConstructT.md)| Registers which constructor QuickFuzzr should use for type T. |
| [Configr&lt;T&gt;AsOneOf](Methods/H_ConfigrAsOneOfT.md)| Chooses randomly between the given derived types when generating T. |
| [Configr&lt;T&gt;.EndOn](Methods/I_ConfigrEndOnT.md)| Replaces deeper recursion with the specified end type. |
| [Configr&lt;T&gt;.Depth](Methods/J_ConfigrDepthT.md)| Sets min and max recursion depth for type T. |
| [Configr.RetryLimit](Methods/K_ConfigrRetryLimit.md)| Sets the global retry limit for retry-based Fuzzrs. |
| [Configr&lt;T&gt;.Apply](Methods/L_ConfigrApplyT.md)| Registers an action executed for each generated value of type `T`. |
| [Configr&lt;T&gt;.With](Methods/N_ConfigrWithT.md)| Applies configuration for T based on a generated value. |
| [Configr.Primitive](Methods/O_ConfigrPrimitiveT.md)| Overrides the default Fuzzr for a primitive type. |
| [Property Access](Methods/P_ConfigrPropertyAccess.md)| Controls auto-generation for specific property access levels. |
