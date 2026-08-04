### 0.1.8: You Can't Always Get What You Want

* Added `Fuzzr.FromEach`.

### 0.1.9: Mother's Little Helpers

* Added `.ToList()`.
* Added `.ToArray()`.
* Added `Fuzzr.Tuple(...)`.
* Removed the `Func<T, T>` overload of `.Apply(...)`.  
  Use LINQ `Select` instead.

### 0.2.1: Paint It Black

* Added `Fuzzr.Sequence(...)` for returning values in order, repeating from the
  beginning when it reaches the end.
* Added opt-in generation for public mutable fields with
  `Configr.EnableFieldAccess()`, plus targeted field configuration with
  `Configr<T>.Field(...)` and `Configr.Field(...)`.
* Default primitive fuzzrs now respect replacements configured with
  `Configr.Primitive(...)`.

### 0.2.2: Start Me Up (*not yet released*)

* Added `Configr<T>.Construct(FuzzrOf<TArg>, Func<TArg, T>)` for lazily
  constructing objects from composed or dependent constructor arguments.
