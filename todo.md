# TODO's 
## For 0.1.6
### Testing:
- check all possible exceptions : current bookmark N_ConfigrWithT
- Add a configr can be set during a LINQ chain test for each option
- Primitive fuzzrs use CheckIt in tests MinMax in impl.
- test depthcontrol many interaction (lists mainly)
- remove all 'Check:'s, and/or search for
  - "Value cannot be null. (Parameter 'source')"
  - @"Object reference not set to an instance of an object.";
  - InvalidOperationException

## After 0.1.6
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
