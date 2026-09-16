### 0.2.3: So Long And Thanks For All The FishPeople

* Added `.Mutate(...)` for choosing and repeating a generator transformation,
  using `(mutation, times)` or `(mutation, min, max)` tuple arguments.
  Ranged repetition uses an exclusive upper bound. equal bounds give an exact count.
* Added string insertion and replacement transformations in `QuickFuzzr.Strings`:
  `.InsertOneOf(...)`, `.ReplaceOneOf(...).WithOneOf(...)`, and `.ReplaceOneWithOneOf(...)`.
  Replacement leaves the source unchanged when no candidate matches or the source is empty.
* Added `CharSet` groups for ASCII digits, letters, alphanumerics, hexadecimal digits,
  quotation marks, line breaks, and control characters.

### 0.2.2: Start Me Up

* Added `Configr<T>.Construct(FuzzrOf<TArg>, Func<TArg, T>)` for lazily
  constructing objects from composed or dependent constructor arguments.
* Added `Configr.Combine(...)` for applying multiple configuration operations
  in argument order.


### 0.2.1: Paint It Black

* Added `Fuzzr.Sequence(...)` for returning values in order, repeating from the
  beginning when it reaches the end.
* Added opt-in generation for public mutable fields with
  `Configr.EnableFieldAccess()`, plus targeted field configuration with
  `Configr<T>.Field(...)` and `Configr.Field(...)`.
* Default primitive fuzzrs now respect replacements configured with
  `Configr.Primitive(...)`.


### 0.1.9: Mother's Little Helpers

* Added `.ToList()`.
* Added `.ToArray()`.
* Added `Fuzzr.Tuple(...)`.
* Removed the `Func<T, T>` overload of `.Apply(...)`.  
  Use LINQ `Select` instead.


### 0.1.8: You Can't Always Get What You Want

* Added `Fuzzr.FromEach`.
