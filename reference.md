# Reference
## Primitive Fuzzrs
The Fuzzr class has many methods which can be used to obtain a corresponding primitive.  
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
### Fuzzr.Constant&lt;T&gt;(T value)
This generator wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined generators.  
### Fuzzr.Counter(object key)
This generator returns an `int` starting at 1, and incrementing by 1 on each subsequent call.  
### Fuzzr One
Trying to generate an abstract class throws an exception with the following message:  
```text
Cannot generate an instance of the abstract class AbstractPerson.
Possible solution:
• Register one or more concrete subtype(s): Configr<AbstractPerson>.AsOneOf(...)
```
### Fuzzr.OneOf&lt;T&gt;(...)
- Trying to choose from an empty collection throws an exception with the following message:  
```text
Fuzzr.OneOf<Person> cannot select from an empty sequence.
Possible solutions:
• Provide at least one option (ensure the sequence is non-empty).
• Use a fallback: Fuzzr.OneOf(values).WithDefault()
• Guard upstream: values.Any() ? Fuzzr.OneOf(values) : Fuzzr.Constant(default!).
```
- An overload exists that takes `params (int Weight, T Value)[] values` arguments in order to influence the distribution of generated values.  
- An overload exists that takes `params (int Weight, FuzzrOf<T> Generator)[] values` arguments in order to influence the distribution of generated values.  
### Fuzzr Shuffle
## Fuzzr Extension Methods
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
- When asking for more unique values than the generator can supply, an exception is thrown.  
- Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.  
- When using the same key for multiple unique generators all values across these generators are unique.  
- An overload exist taking a function as an argument allowing for a dynamic key.  
### Ext Fuzzr Where
## Configuring
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
### Configr&lt;T&gt;.Construct(...)
**Usage:**  
```csharp
 Configr<SomeThing>.Construct(Fuzzr.Constant(42));
```
Subsequent calls to `Fuzzr.One<T>()` will then use the registered constructor.  
Various overloads exist that allow for up to five constructor arguments.  

After that, ... you're on your own.  
### Configr&lt;T&gt;.Ignore(...)
**Usage:**  
```csharp
 Configr<Thing>.Ignore(s => s.Id);
```
The property specified will be ignored during generation.  
Derived classes generated also ignore the base property.  
### Configr&lt;T&gt;.IgnoreAll()
**Usage:**  
```csharp
 Configr<Thing>.IgnoreAll();
```
Ignore all properties while generating an object.  
`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.  
### Configr&lt;T&gt;.Property(...)
**Usage:**  
```csharp
 Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
```
- The property specified will be generated using the passed in generator.  
- An overload exists which allows for passing a value instead of a generator.  
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
