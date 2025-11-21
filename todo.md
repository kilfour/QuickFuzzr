# TODO's 
- configr retry limit doc, use DocExceptions
## After 0.1.7
- test depthcontrol many interaction (lists mainly)

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

## QuickFuzzr.Evil (namespace for now)
* Configr.EnableFieldAccess() 
* Structure & Static Factory classes
  * Configr.Primitive as the hook = exactly right.
  * Evilr.* as FuzzrOf<string> (and maybe later more types) = good separation.
  * Evilizr.* as “install a profile of Evilr + normal into primitives” = great ergonomics.
  * Weighted evil is the default mode; hardcore / soft modes are just different mixes.
  * Domain-focused evil (JsonStrings, HtmlStrings) is content curation, not scope creep.

