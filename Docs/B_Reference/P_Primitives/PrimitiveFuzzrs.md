# Primitive Fuzzrs
QuickFuzzr includes built-in Fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex Fuzzrs are composed, and are used automatically when generating object properties.

> *All primitive Fuzzrs automatically drive object property generation.
> Nullable and non-nullable variants are both supported.
> Each Fuzzr also supports `.Nullable(...)` / `.NullableRef(...)` as appropriate.*

In this reference we categorize these Fuzzrs as follows:  
## Ranged Primitives
Ranged primitives generate numeric or temporal values between a minimum and a maximum.

For all of these the following is true:
- They have a paremeterless function whcich returns a value in a default range.
- They have an overload which allows you to specify a min and max value.
- They throw a `ArgumentOutOfRangeException` if the min is greater than the max.

Furthermore, there are two types of ranged primitives:
  
### Continuous
Values come from a dense numeric space (floating-point types).

For these, the upper bound is conceptually exclusive ([min, max)),
but floating-point rounding may occasionally allow max to appear.
This behaviour is explicitly tested and documented.
  
#### Decimals
Use `Fuzzr.Decimal()`.  
**Default Range and Precision:** min = 1, max = 100, precision = 2).  
Apart from the usual ranged primitive min/max overload, `Fuzzr.Decimal()` adds two more allowing the user to specify a precision.  
- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.  
- Throws an `ArgumentException` when `precision` < `0`.  
- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal in the range [min, max) (min inclusive, max exclusive), with up to `precision` decimal places.  
- Throws an `ArgumentException` when `precision` < `0`.  
#### Doubles
Use `Fuzzr.Double()`.  
- **Default Range:** min = 1, max = 100).  
#### Floats
Use `Fuzzr.Float()`.  
- **Default Range:** min = 1, max = 100).  
#### Halfs
Use `Fuzzr.Half()`.  
- **Default Range:** min = 1, max = 100).  
*Note:* Due to floating-point rounding, max may occasionally be produced.  
### Discrete
Values come from a countable set (integers, shorts, bytes, dates snapped to seconds, etc.).

These appear in two flavours:  
#### Inclusive upper bound.
The maximum value can be produced.  
Used when C# conventions or data types naturally use [min, max] (e.g. DateOnly, Byte, Char).  
#### Bytes
Use `Fuzzr.Byte()`.  
- **Default Range:** min = 1, max = 255.  
#### Chars
Use `Fuzzr.Char()`.  
- **Default Range:** min = 'a', max = 'z'.  
#### DateOnlys
Use `Fuzzr.DateOnly()`.  
- **Default Range:** min = 'DateOnly(1970, 1, 1)', max = 'DateOnly(2020, 12, 31)'.  
#### DateTimes
Use `Fuzzr.DateTime()`.  
- **Default Range:** min = 'DateOnly(1970, 1, 1)', max = 'DateOnly(2020, 12, 31)'.  
- Resulting values are snapped to whole seconds.  
#### Exclusive upper bound.
The maximum value is never produced.  
Matches the C# RNG convention used by Random.Next(min, max) and applies to most integer fuzzrs.  
#### Ints
Use `Fuzzr.Int()`.  
- **Default Range:** min = 1, max = 100).  
#### Longs
Use `Fuzzr.Long()`.  
- **Default: Range** min = 1, max = 100).  
#### Shorts
Use `Fuzzr.Short()`.  
- **Default Range:** min = 1, max = 100).  
#### TimeOnlys
Use `Fuzzr.TimeOnly()`.  
- **Default Range:** min = 00:00:00, max = 23:59:59.9999999).  
#### TimeSpans
Use `Fuzzr.TimeSpan()`.  
- **Default Range:** min = 1, max = 1000).  
#### UInts
Use `Fuzzr.UInt()`.  
- **Default Range:** min = 1, max = 100).  
#### ULongs
Use `Fuzzr.ULong()`.  
- **Default Range:** min = 1, max = 100).  
#### UShorts
Use `Fuzzr.UShort()`.  
- **Default Range:** min = 1, max = 100).  
## Non Ranged Primitives
### Booleans
Use `Fuzzr.Bool()`.  
- Generates `true` or `false`.  
### Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default Fuzzr just picks a random value from all enumeration values.  
- Passing in a non Enum type for T throws an ArgumentException.  
### Guids
Use `Fuzzr.Guid()`.  
- The default Fuzzr never generates Guid.Empty.  
- `Fuzzr.Guid()` is deterministic when seeded.  
### Strings
Use `Fuzzr.String()`.  
- The Default Fuzzr generates a string of length greater than or equal to 1 and less than or equal to 10.  
- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.  
- Throws an `ArgumentOutOfRangeException` when `length` < 0.  
- The default Fuzzr always generates every char element of the string to be between lower case 'a' and lower case 'z'.  
- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.  
