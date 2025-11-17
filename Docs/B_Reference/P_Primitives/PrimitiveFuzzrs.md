# Primitive Fuzzrs
QuickFuzzr includes built-in Fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex Fuzzrs are composed, and are used automatically when generating object properties.

All range-based numeric fuzzrs follow .NET conventions: the lower bound is inclusive and the upper bound is exclusive, unless explicitly stated otherwise.

> *All primitive Fuzzrs automatically drive object property generation.
> Nullable and non-nullable variants are both supported.
> Each Fuzzr also supports `.Nullable(...)` / `.NullableRef(...)` as appropriate.*
  
| Fuzzr| Description |
| -| - |
| [Booleans](Methods/Booleans.md)| Generates random `true` or `false` values.  |
| [Enums](Methods/Enums.md)| Randomly selects a defined member of an enum type. |
| [Guids](Methods/Guids.md)| Produces non-empty random `Guid` values. |
| [Strings](Methods/Strings.md)| Creates random lowercase strings (default length 1-10). |
| [TimeSpans](Methods/TimeSpans.md)| Generates random durations up to 1000 ticks by default. |
| Fuzzr| Description |
| -| - |
| [Bytes](Ranged/Bytes.md)| Produces random bytes in the range 0-255 or within a custom range. |
| [Chars](Ranged/Chars.md)| Generates random lowercase letters or characters within a specified range. |
| [DateOnlys](Ranged/DateOnlys.md)| Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default). |
| [DateTimes](Ranged/DateTimes.md)| Produces `DateTime` values between 1970-01-01 and 2020-12-31 (inclusive), snapped to whole seconds. |
| [Decimals](Ranged/Decimals.md)| Generates random decimal numbers (default 1-100). |
| [Doubles](Ranged/Doubles.md)| Generates random double-precision numbers (default 1-100). |
| [Floats](Ranged/Floats.md)| Generates random single-precision numbers (default 1-100). |
| [Halfs](Ranged/Halfs.md)| Generates random 16-bit Halfing-point numbers (default 1-100). |
| [Ints](Ranged/Ints.md)| Produces random integers (default 1-100). |
| [Longs](Ranged/Longs.md)| Generates random 64-bit longegers (default 1-100). |
| [Shorts](Ranged/Shorts.md)| Generates random 16-bit integers (default 1-100). |
| [TimeOnlys](Ranged/TimeOnlys.md)| Produces random times between midnight and 23:59:59. |
| [UInts](Ranged/UInts.md)| Produces unsigned integers (default 1-100). |
| [ULongs](Ranged/ULongs.md)| Generates unsigned 64-bit integers (default 1-100). |
| [UShorts](Ranged/UShorts.md)| Produces unsigned 16-bit integers (default 1-100). |
