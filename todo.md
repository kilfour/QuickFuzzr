# TODO's 
- test depthcontrol many interaction (lists mainly)
* Generate a valid Heap
  A heap is a binary tree where every node’s value is less than or equal to the values of its children.

## DOC
- configr retry limit doc, use DocExceptions
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

## QuickFuzzr.Evil (namespace for now)
* Configr.EnableFieldAccess() 




