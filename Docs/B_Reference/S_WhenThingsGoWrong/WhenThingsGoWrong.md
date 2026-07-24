# When Things Go Wrong
QuickFuzzr exceptions derive from `QuickFuzzrException`, so callers can catch
that base type when they do not need to distinguish between individual failures.

## Construction and instantiation

| Exception | Thrown when |
|---|---|
| `ConstructionException` | A type has no parameterless constructor and no custom construction has been configured. |
| `ConstructorNotFoundException` | `Configr<T>.Construct(...)` cannot find a constructor matching the configured argument types. |
| `FactoryConstructionException` | A factory passed to `Fuzzr.One(...)` returns `null`. |
| `InstantiationException` | QuickFuzzr attempts to instantiate an abstract type. |

## Configuration and type hierarchy

| Exception | Thrown when |
|---|---|
| `PropertyConfigurationException` | A property configuration expression refers to something other than a property. |
| `FieldConfigurationException` | A field configuration expression refers to something other than a field. |
| `RetryLimitOutOfRangeException` | `Configr.RetryLimit(...)` is set outside the allowed range of 1-1024. |
| `DerivedTypeIsNullException` | `Configr<T>.AsOneOf(...)` receives a `null` derived type. |
| `EmptyDerivedTypesException` | `Configr<T>.AsOneOf(...)` receives no derived types. |
| `DuplicateDerivedTypesException` | `Configr<T>.AsOneOf(...)` receives the same derived type more than once. |
| `DerivedTypeNotAssignableException` | A type passed to `AsOneOf(...)` or `EndOn<TEnd>()` is not assignable to the configured base type. |

## Value exhaustion and retries

| Exception | Thrown when |
|---|---|
| `PredicateUnsatisfiedException` | `.Where(...)` cannot produce a value satisfying its predicate within the retry limit. |
| `UniqueValueExhaustedException` | `.Unique(...)` cannot produce a new value within the retry limit. |
| `NonNullValueExhaustedException` | `.NeverReturnNull()` cannot produce a non-null value within the retry limit. |

## Selection and combinator misuse

| Exception | Thrown when |
|---|---|
| `OneOfEmptyOptionsException` | `Fuzzr.OneOf(...)` is asked to select from an empty sequence. |
| `NegativeWeightException` | A weighted `Fuzzr.OneOf(...)` receives one or more negative weights. |
| `ZeroTotalWeightException` | The total weight passed to `Fuzzr.OneOf(...)` is zero or less. |

Exception messages include possible solutions tailored to the failure.  
