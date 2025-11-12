# Reference
This reference provides a **complete, factual overview** of QuickFuzzr's public API.
It lists all available generators, configuration points, and extension methods, organized by category.  
Each entry includes a concise description of its purpose and behavior,
serving as a quick lookup for day-to-day use or library integration.

If you're looking for examples or background explanations, see the main documentation.

All examples and summaries are real, verified through executable tests, ensuring what you see here is exactly what QuickFuzzr does.  
## Contents

- [A. Primitive Fuzzrs][PrimitiveFuzzrs]
- [B. Fuzzing][Fuzzing]
- [C. Fuzzr Extension Methods][FuzzrExtensionMethods]
- [D. Configuration][Configuring]
  

[PrimitiveFuzzrs]: #primitive-fuzzrs

[Fuzzing]: #fuzzing

[FuzzrExtensionMethods]: #fuzzr-extension-methods

[Configuring]: #configuring
## Primitive Fuzzrs
QuickFuzzr includes built-in generators for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex generators are composed, and are used automatically when generating object properties.
  
| Fuzzr| Description |
| -| - |
| [Booleans](#booleans)| Generates random `true` or `false` values.  |
| [Bytes](#bytes)| Produces random bytes in the range 0-255 or within a custom range. |
| [Chars](#chars)| Generates random lowercase letters or characters within a specified range. |
| [DateOnlys](#dateonlys)| Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default). |
| [DateTimes](#datetimes)| Produces `DateTime` values between 1970-01-01 and 2020-12-31. |
| [Decimals](#decimals)| Generates random decimal numbers (default 1-100). |
| [Doubles](#doubles)| Generates random double-precision numbers (default 1-100). |
| [Enums](#enums)| Randomly selects a defined member of an enum type. |
| [Floats](#floats)| Generates random single-precision numbers (default 1-100). |
| [Guids](#guids)| Produces non-empty random `Guid` values. |
| [Halfs](#halfs)| Generates random 16-bit floating-point numbers (default 1-100). |
| [Ints](#ints)| Produces random integers (default 1-100). |
| [Longs](#longs)| Generates random 64-bit integers (default 1-100). |
| [Shorts](#shorts)| Generates random 16-bit integers (default 1-100). |
| [Strings](#strings)| Creates random lowercase strings (default length 1-10). |
| [TimeOnlys](#timeonlys)| Produces random times between midnight and 23:59:59. |
| [TimeSpans](#timespans)| Generates random durations up to 1000 ticks by default. |
| [UInts](#uints)| Produces unsigned integers (default 1-100). |
| [ULongs](#ulongs)| Generates unsigned 64-bit integers (default 1-100). |
| [UShorts](#ushorts)| Produces unsigned 16-bit integers (default 1-100). |
### Booleans
Use `Fuzzr.Bool()`.  
- The default generator generates True or False.  
- Can be made to return `bool?` using the `.Nullable()` combinator.  
- `bool` is automatically detected and generated for object properties.  
- `bool?` is automatically detected and generated for object properties.  
### Bytes
Use `Fuzzr.Byte()`.  
- The default generator produces a `byte` in the full range (`0`-`255`).  
- The overload `Fuzzr.Byte(int min, int max)` generates a value greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- Throws an `ArgumentOutOfRangeException` when `min` < `byte.MinValue` (i.e. `< 0`).  
- Throws an `ArgumentOutOfRangeException` when `max` > `byte.MaxValue` (i.e. `> 255`).  
- When `min == max`, the generator always returns that exact value.  
- Boundary coverage: over time, values at both ends of the interval should appear.  
- Can be made to return `byte?` using the `.Nullable()` combinator.  
- `byte` is automatically detected and generated for object properties.  
- `byte?` is automatically detected and generated for object properties.  
### Chars
Use `Fuzzr.Char()`.  
- The overload `Fuzzr.Char(char min, char max)` generates a char greater than or equal to `min` and less than or equal to `maxs`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator always generates a char between lower case 'a' and lower case 'z'.  
- Can be made to return `char?` using the `.Nullable()` combinator.  
- `char` is automatically detected and generated for object properties.  
- `char?` is automatically detected and generated for object properties.  
### DateOnlys
Use `Fuzzr.DateOnly()`.  
- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = new DateOnly(1970, 1, 1), max = new DateOnly(2020, 12, 31)).  
- Can be made to return `DateOnly?` using the `.Nullable()` combinator.  
- `DateOnly` is automatically detected and generated for object properties.  
- `DateOnly?` is automatically detected and generated for object properties.  
### DateTimes
Use `Fuzzr.DateTime()`.  
- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a DateTime greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).  
- Can be made to return `DateTime?` using the `.Nullable()` combinator.  
- `DateTime` is automatically detected and generated for object properties.  
- `DateTime?` is automatically detected and generated for object properties.  
### Decimals
Use `Fuzzr.Decimal()`.  
- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `decimal?` using the `.Nullable()` combinator.  
- `decimal` is automatically detected and generated for object properties.  
- `decimal?` is automatically detected and generated for object properties.  
### Doubles
Use `Fuzzr.Double()`.  
- The overload `Fuzzr.Double(double min, double max)` generates a double greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `double?` using the `.Nullable()` combinator.  
- `double` is automatically detected and generated for object properties.  
- `double?` is automatically detected and generated for object properties.  
### Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default generator just picks a random value from all enumeration values.  
- An Enumeration is automatically detected and generated for object properties.  
- A nullable enumeration is automatically detected and generated for object properties.  
- Passing in a non Enum type for T throws an ArgumentException.  
### Floats
Use `Fuzzr.Float()`.  
- The overload `Fuzzr.Float(float min, float max)` generates a float greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `float?` using the `.Nullable()` combinator.  
- `float` is automatically detected and generated for object properties.  
- `float?` is automatically detected and generated for object properties.  
### Guids
Use `Fuzzr.Guid()`. *There is no overload.*  
- The default generator never generates Guid.Empty.  
- `Fuzzr.Guid()` is deterministic when seeded.  
- Can be made to return `Guid?` using the `.Nullable()` combinator.  
- `Guid` is automatically detected and generated for object properties.  
- `Guid?` is automatically detected and generated for object properties.  
### Halfs
Use `Fuzzr.Half()`.  
- The overload Fuzzr.Half(Half min, Half max) generates a half-precision floating-point number greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = (Half)1, max = (Half)100).  
- Can be made to return `Half?` using the `.Nullable()` combinator.  
- `Half` is automatically detected and generated for object properties.  
- `Half?` is automatically detected and generated for object properties.  
### Ints
Use `Fuzzr.Int()`.  
- The overload `Fuzzr.Int(int min, int max)` generates an int greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `int?` using the `.Nullable()` combinator.  
- `int` is automatically detected and generated for object properties.  
- `int?` is automatically detected and generated for object properties.  
### Longs
Use `Fuzzr.Long()`.  
- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `long?` using the `.Nullable()` combinator.  
- `long` is automatically detected and generated for object properties.  
- `long?` is automatically detected and generated for object properties.  
### Shorts
Use `Fuzzr.Short()`.  
- The overload `Fuzzr.Short(short min, short max)` generates a short greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `short?` using the `.Nullable()` combinator.  
- `short` is automatically detected and generated for object properties.  
- `short?` is automatically detected and generated for object properties.  
### Strings
Use `Fuzzr.String()`.  
- The Default generator generates a string of length greater than or equal to 1 and less than or equal to 10.  
- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.  
- Throws an `ArgumentOutOfRangeException` when `length` < 0.  
- The default generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.  
- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.  
- Can be made to return `string?` using the `.NullableRef()` combinator.  
- `string` is automatically detected and generated for object properties.  
- `string?` is automatically detected and generated for object properties.  
### TimeOnlys
Use `Fuzzr.TimeOnly()`.  
- The overload `Fuzzr.TimeOnly(TimeOnly min, TimeOnly max)` generates a TimeOnly greater than or equal to `min` and less than `max`.  
- The default generator is (min = 00:00:00, max = 23:59:59.9999999).  
- Can be made to return `TimeOnly?` using the `.Nullable()` combinator.  
- `TimeOnly` is automatically detected and generated for object properties.  
- `TimeOnly?` is automatically detected and generated for object properties.  
### TimeSpans
Use `Fuzzr.TimeSpan()`.  
- The overload `Fuzzr.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.  
- The default generator is (max = 1000).  
- Can be made to return `TimeSpan?` using the `.Nullable()` combinator.  
- `TimeSpan` is automatically detected and generated for object properties.  
- `TimeSpan?` is automatically detected and generated for object properties.  
### UInts
Use `Fuzzr.UInt()`.  
- The overload `Fuzzr.UInt(uint min, uint max)` generates an uint greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `uint?` using the `.Nullable()` combinator.  
- `uint` is automatically detected and generated for object properties.  
- `uint?` is automatically detected and generated for object properties.  
### ULongs
Use `Fuzzr.ULong()`.  
- The overload `Fuzzr.ULong(ulong min, ulong max)` generates a ulong greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `ulong?` using the `.Nullable()` combinator.  
- `ulong` is automatically detected and generated for object properties.  
- `ulong?` is automatically detected and generated for object properties.  
### UShorts
Use `Fuzzr.UShort()`.  
- The overload `Fuzzr.UShort(ushort min, ushort max)` generates a ushort greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `ushort?` using the `.Nullable()` combinator.  
- `ushort` is automatically detected and generated for object properties.  
- `ushort?` is automatically detected and generated for object properties.  
## Fuzzing
This section lists the core generators responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a FuzzrOf<T> and can be composed using standard LINQ syntax.  
### Contents
| Fuzzr| Description |
| -| - |
| [Fuzzr.Constant&lt;T&gt;(T value)](#fuzzrconstanttt-value)| Wraps a fixed value in a generator, producing the same result every time. |
| [Fuzzr.Counter(object key)](#fuzzrcounterobject-key)| Generates a sequential integer per key, starting at 1. |
| [Fuzzr.One&lt;T&gt;()](#fuzzronet)| Creates a generator that produces an instances of type `T`. |
| [Fuzzr.OneOf&lt;T&gt;(params &lt;T&gt;[] values)](#fuzzroneoftparams-t-values)| Randomly selects one of the provided values. |
| [Fuzzr.Shuffle&lt;T&gt;()](#fuzzrshufflet)| Creates a generator that randomly shuffles an input sequence. |
### Fuzzr.Constant&lt;T&gt;(T value)
This generator wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined generators.  
### Fuzzr.Counter(object key)
This generator returns an `int` starting at 1, and incrementing by 1 on each call.  
Useful for generating unique sequential IDs or counters.  
  
**Usage:**  
```csharp
Fuzzr.Counter("the-key").Many(5).Generate();
// Returns => [1, 2, 3, 4, 5]
```
- Each `key` maintains its own independent counter sequence.  
- Counter state resets between separate `Generate()` calls.  
- Works seamlessly in LINQ chains and with .Apply(...) to offset or transform the sequence.  

**Exceptions:**  
- `ArgumentNullException`: When the provided key is null.  
### Fuzzr.One&lt;T&gt;()
Creates a generator that produces complete instances of type `T` using QuickFuzzr's automatic construction rules:   
**Usage:**  
```csharp
Fuzzr.One<Person>();
```
 - Uses `T`'s public parameterless constructor. Parameterized ctors aren't auto-filled.  
- Primitive properties are generated using their default `Fuzzr` equivalents.  
- Enumerations are filled using `Fuzzr.Enum<T>()`.  
- Object properties are generated where possible.  
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  
- Field generation is not supported.  

**Overloads:**  
- `Fuzzr.One<T>(Func<T> constructor)`:  
  Creates a generator that produces instances of T by invoking the supplied factory on each generation.  

**Exceptions:**  
- `ConstructionException`: When type T cannot be constructed due to missing default constructor.  
- `InstantiationException`: When type T is an interface and cannot be instantiated.  
- `NullReferenceException`:  
  - When the factory method returns null.  
  - When the factory method is null.  
### Fuzzr.OneOf&lt;T&gt;(params &lt;T&gt;[] values)
Creates a generator that randomly selects one value or generator from the provided options.  
**Usage:**  
```csharp
 Fuzzr.OneOf("a", "b", "c");
```
- Selection is uniform unless weights are specified (see below).  

**Overloads:**  
- `Fuzzr.OneOf(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  
- `Fuzzr.OneOf(params FuzzrOf<T>[] generators)`:  
  Randomly selects and executes one of the provided generators.  
- `Fuzzr.OneOf(params (int Weight, T Value)[] weightedValues)`:  
  Selects a value using weighted probability. The higher the weight, the more likely the value is to be chosen.  
```csharp
 Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c"));
```
- `Fuzzr.OneOf(params (int Weight, FuzzrOf<T> Generator)[] weightedGenerators)`:  
  Like the weighted values overload, but applies weights to generators.  

**Exceptions:**  
  - `OneOfEmptyOptionsException`: When trying to choose from an empty collection.  
  - `NegativeWeightException`: When one or more weights are negative.  
  - `ZeroTotalWeightException`: When the total of all weights is zero or negative.  
  - `ArgumentNullException`: When the provided sequence is null.  
### Fuzzr.Shuffle&lt;T&gt;()
Creates a generator that produces a random permutation of the provided sequence.  
Use for randomized ordering, unbiased sampling without replacement.  
**Usage:**  
```csharp
Fuzzr.Shuffle("John", "Paul", "George", "Ringo");
// Results in => ["Paul", "Ringo", "John", "George"]
```

**Overloads:**  
- `Shuffle<T>(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  

**Exceptions:**  
  - `ArgumentNullException`: When the input collection is `null`.  
## Fuzzr Extension Methods
QuickFuzzr provides a collection of extension methods that enhance the expressiveness and composability of `FuzzrOf<T>`.
These methods act as modifiers, they wrap existing generators to alter behavior, add constraints,
or chain side-effects without changing the underlying LINQ-based model.
  
### Ext Fuzzr Apply
### Ext Fuzzr As Object
### Ext Fuzzr Many
### Ext Fuzzr Never Return Null
### Ext Fuzzr Nullable
### Ext Fuzzr Nullable Ref
### Ext Fuzzr Shuffle
### .Unique&lt;T&gt;(...)
Using the `.Unique(object key)` extension method.  
- Makes sure that every generated value is unique.  
- When asking for more unique values than the fuzzr can supply, an exception is thrown.  
- Multiple unique fuzzrs can be defined in one 'composed' fuzzr, without interfering with eachother by using a different key.  
- When using the same key for multiple unique fuzzrs all values across these fuzzrs are unique.  
- An overload exist taking a function as an argument allowing for a dynamic key.  
### Ext Fuzzr Where
### Ext Fuzzr With Default
## Configuring
### Configr&lt;T&gt;AsOneOf(params Type[] types)
Configures inheritance resolution for BaseType, 
allowing QuickFuzzr to randomly select one of the specified derived types when generating instances.  

Useful when generating domain hierarchies where multiple concrete subtypes exist.  
  
**Usage:**  
```csharp
var personFuzzr =
    from asOneOf in Configr<Person>.AsOneOf(typeof(Person), typeof(Employee))
    from item in Fuzzr.One<Person>()
    select item;
personFuzzr.Many(2).Generate();
// Results in =>
// [
//     Employee {
//         Email: "dn",
//         SocialSecurityNumber: "gs",
//         Name: "etggni",
//         Age: 38
//     },
//     Person { Name: "avpkdc", Age: 70 }
// ]
```

**Exceptions:**  
- `EmptyDerivedTypesException`: When no types are provided.  
- `DuplicateDerivedTypesException`: When the list of derived types contains duplicates.  
  - `DuplicateDerivedTypesException`: When the list of derived types contains duplicates.  
- `DerivedTypeNotAssignableException`: If any listed type is not a valid subclass of `BaseType`.  
- `DerivedTypeIsNullException`: If any listed type is `null`.  
- `DerivedTypeIsAbstractException`: When one or more derived types cannot be instantiated.  
### Configr&lt;T&gt;.Construct(FuzzrOf&lt;T1&gt; arg1)
Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
  
**Usage:**  
```csharp
Configr<SomeThing>.Construct(Fuzzr.Constant(42));
```

**Overloads:**  
```csharp
Construct<T1,T2>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2)
```
```csharp
Construct<T1,T2,T3>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3)
```
```csharp
Construct<T1,T2,T3,T4>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4)
```
```csharp
Construct<T1,T2,T3,T4,T5>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4, FuzzrOf<T5> arg5)
```

**Exceptions:**  
- `ArgumentNullException`: If one of the `TArg` parameters is null.  
- `InvalidOperationException`: If no matching constructor is found on type T.  
### Configr&lt;T&gt;.Depth(int min, int max)
Configures depth constraints for type `T` to control recursive object graph generation. 
  
**Usage:**  
```csharp
Configr<Turtle>.Depth(2, 5);
// Results in =>
// Turtle { Down: { Down: { Down: { Down: null } } } }
```
Subsequent calls to `Fuzzr.One<T>()` will generate between 2 and 5 nested levels of `Turtle` instances,
depending on the random draw and available recursion budget.  
Depth is per type, not global. Each recursive type manages its own budget.
  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When min is negative.  
- `ArgumentOutOfRangeException`: When max is lesser than min  
### Configr End On T
### Configr.Ignore(...)
**Usage:**  
```csharp
 Configr.Ignore(a => a.Name == "Id");
```
Any property matching the predicate will be ignored during generation.  
### Configr.IgnoreAll()
**Usage:**  
```csharp
 Configr<Thing>.IgnoreAll();
```
Ignore all properties while generating anything.  
### Configr&lt;T&gt;.IgnoreAll()
**Usage:**  
```csharp
 Configr<Thing>.IgnoreAll();
```
Ignore all properties while generating an object.  
`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.  
### Configr&lt;T&gt;.Ignore(...)
**Usage:**  
```csharp
 Configr<Thing>.Ignore(s => s.Id);
```
The property specified will be ignored during generation.  
Derived classes generated also ignore the base property.  
### Configr Primitive
### Configr.Property(...)
**Usage:**  
```csharp
 Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
```
Any property matching the predicate will use the specified Fuzzr during generation.  
A utility overload exists that allows one to pass in a value instead of a fuzzr.  
```csharp
 Configr.Property(a => a.Name == "Id", 42);
```
Another overload allows you to create a fuzzr dynamically using a `Func<PropertyInfo, FuzzrOf<T>>` factory method.  
```csharp
 Configr.Property(a => a.Name == "Id", a => Fuzzr.Constant(42));
```
With the same *pass in a value* conveniance helper.  
```csharp
 Configr.Property(a => a.Name == "Id", a => 42);
```
### Configr Property Access
### Configr&lt;T&gt;.Property(...)
**Usage:**  
```csharp
 Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
```
- The property specified will be generated using the passed in fuzzr.  
- An overload exists which allows for passing a value instead of a fuzzr.  
```csharp
 Configr<Thing>.Property(s => s.Id, 666);
```
- Derived classes generated also use the custom property.  
- Trying to configure a field throws an exception with the following message:  
```text
Cannot configure expression 'a => a.Name'.
It does not refer to a property.
Fields and methods are not supported by default.
Possible solutions:
• Use a property selector (e.g. a => a.PropertyName).
• Then pass it to Configr<PersonOutInTheFields>.Property(...) to configure generation.
```
### Configr.RetryLimit(int limit)
**Usage:**  
```csharp
 Configr.RetryLimit(256);
```
- Sets the global retry limit used by generators.  
- Throws when trying to set limit to a value lesser than 1.  
- Throws when trying to set limit to a value greater than 1024.  
```text
Invalid retry limit value: 1025
Allowed range: 1-1024
Possible solutions:
• Use a value within the allowed range
• Check for unintended configuration overrides
• If you need more, consider revising your generator logic instead of increasing the limit
```
### Configr With T
