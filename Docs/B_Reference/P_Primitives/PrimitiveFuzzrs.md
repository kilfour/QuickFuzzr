# Primitive Fuzzrs
QuickFuzzr includes built-in fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex fuzzrs are composed, and are used automatically when generating object properties.

> *All primitive fuzzrs automatically drive object property generation.
> Nullable and non-nullable variants are both supported.
> Each fuzzr also supports `.Nullable(...)` / `.NullableRef(...)` as appropriate.*
  
| Fuzzr| Description |
| -| - |
| [Booleans](Methods/Booleans.md)| Generates random `true` or `false` values.  |
| [Bytes](Methods/Bytes.md)| Produces random bytes in the range 0-255 or within a custom range. |
| [Chars](Methods/Chars.md)| Generates random lowercase letters or characters within a specified range. |
| [DateOnlys](Methods/DateOnlys.md)| Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default). |
| [DateTimes](Methods/DateTimes.md)| Produces `DateTime` values between 1970-01-01 and 2020-12-31 (inclusive), snapped to whole seconds. |
| [Decimals](Methods/Decimals.md)| Generates random decimal numbers (default 1-100). |
| [Doubles](Methods/Doubles.md)| Generates random double-precision numbers (default 1-100). |
| [Enums](Methods/Enums.md)| Randomly selects a defined member of an enum type. |
| [Floats](Methods/Floats.md)| Generates random single-precision numbers (default 1-100). |
| [Guids](Methods/Guids.md)| Produces non-empty random `Guid` values. |
| [Halfs](Methods/Halfs.md)| Generates random 16-bit floating-point numbers (default 1-100). |
| [Ints](Methods/Ints.md)| Produces random integers (default 1-100). |
| [Longs](Methods/Longs.md)| Generates random 64-bit integers (default 1-100). |
| [Shorts](Methods/Shorts.md)| Generates random 16-bit integers (default 1-100). |
| [Strings](Methods/Strings.md)| Creates random lowercase strings (default length 1-10). |
| [TimeOnlys](Methods/TimeOnlys.md)| Produces random times between midnight and 23:59:59. |
| [TimeSpans](Methods/TimeSpans.md)| Generates random durations up to 1000 ticks by default. |
| [UInts](Methods/UInts.md)| Produces unsigned integers (default 1-100). |
| [ULongs](Methods/ULongs.md)| Generates unsigned 64-bit integers (default 1-100). |
| [UShorts](Methods/UShorts.md)| Produces unsigned 16-bit integers (default 1-100). |
