# Configuring
`Configr` provides a fluent API to influence how QuickFuzzr builds objects.
Use it to set global defaults, customize properties, control recursion depth,
select derived types, or wire dynamic behaviors that apply when calling `Fuzzr.One<T>()`.
  
## Contents
| Configr| Description |
| -| - |
| [Configr&lt;T&gt;.Ignore(...)](Methods/A_ConfigrIgnoreT.md)|   |
| [Configr.Ignore(Func&lt;PropertyInfo, bool&gt; predicate)](Methods/B_ConfigrIgnore.md)|   |
| [Configr&lt;T&gt;.IgnoreAll()](Methods/C_ConfigrIgnoreAllT.md)|   |
| [Configr.IgnoreAll()](Methods/D_ConfigrIgnoreAll.md)|   |
| [Configr&lt;T&gt;.Property(...)](Methods/E_ConfigrPropertyT.md)|   |
| [Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)](Methods/F_ConfigrProperty.md)|   |
| [Configr&lt;T&gt;.Construct(FuzzrOf&lt;T1&gt; arg1)](Methods/G_ConfigrConstructT.md)|   |
| [Configr&lt;T&gt;AsOneOf(params Type[] types)](Methods/H_ConfigrAsOneOfT.md)|   |
| [Configr&lt;T&gt;.EndOn&lt;TEnd&gt;()](Methods/I_ConfigrEndOnT.md)|   |
| [Configr&lt;T&gt;.Depth(int min, int max)](Methods/J_ConfigrDepthT.md)|   |
| [Configr.RetryLimit(int limit)](Methods/K_ConfigrRetryLimit.md)|   |
| [Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)](Methods/L_ConfigrWithT.md)|   |
| [Configr.Primitive&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr)](Methods/N_ConfigrPrimitive.md)|   |
| [Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) / Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)](Methods/O_ConfigrPropertyAccess.md)|   |
