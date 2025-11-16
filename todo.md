# TODO's 
## For 0.1.7
### Testing:
- check all possible exceptions : current bookmark primitives
- Add a configr can be set during a LINQ chain test for each option
- Primitive fuzzrs use CheckIt in tests MinMax in impl.
- test depthcontrol many interaction (lists mainly)
- check Congigr explosion: see F_ConfigrProperty.Configr_DoesNotMultiply

## After 0.1.7
Error section: a single table listing all QuickFuzzr exceptions with one-liners 
per category ? construction problems, value exhausted ...
* Construction / instantiation
  * ConstructionException
  * InstantiationException
  * DerivedTypeIsNullException
  * EmptyDerivedTypesException
  * DerivedTypeNotAssignableException (or whatever you ended on)
*Configuration / property / configr
  * PropertyConfigurationException
  * PredicateUnsatisfiedException (if used in config)
  * RetryLimitOutOfRangeException
* Value exhaustion / uniqueness / retries
  * UniqueValueExhaustedException
  * NullValueExhaustedException
  * ZeroTotalWeightException
  * NegativeWeightException
* OneOf / combinator misuses
  * OneOfEmptyOptionsException

## For 0.2.0
* Benchmarks
* Add the missing Primitives

## QuickFuzzr.Evil 
Configr.EnableFieldAccess() 
static class name:
- Breakr 
- Chaosr *
- Hexr
- just use a type per category 
