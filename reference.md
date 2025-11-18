# Reference
This reference provides a **complete, factual overview** of QuickFuzzr's public API.
It lists all available Fuzzrs, configuration points, and extension methods, organized by category.  
Each entry includes a concise description of its purpose and behavior,
serving as a quick lookup for day-to-day use or library integration.

All examples and summaries are real, verified through executable tests, ensuring what you see here is exactly what QuickFuzzr does.

QuickFuzzr exposes three kinds of building blocks: `FuzzrOf<T>` for value production, `Configr` for generation behavior, and extension methods for modifying Fuzzrs.  
Everything in this reference fits into one of these three roles.

If you're looking for examples or background explanations, see the guide or cookbook.
  
## Contents

- [Fuzzing][Fuzzing]
- [Configuration][Configuring]
- [Fuzzr Extension Methods][FuzzrExtensionMethods]
- [Primitive Fuzzrs][PrimitiveFuzzrs]
  

[PrimitiveFuzzrs]: #primitive-fuzzrs

[Fuzzing]: #fuzzing

[FuzzrExtensionMethods]: #fuzzr-extension-methods

[Configuring]: #configuring
## Fuzzing
This section lists the core Fuzzrs responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a `FuzzrOf<T>` and can be composed using standard LINQ syntax.  
### Contents
| Fuzzr| Description |
| -| - |
| [One](#one)| Creates a Fuzzr that produces an instances of type `T`. |
| [OneOf](#oneof)| Randomly selects one of the provided values. |
| [Shuffle](#shuffle)| Creates a Fuzzr that randomly shuffles an input sequence. |
| [Counter](#counter)| Generates a sequential integer per key, starting at 1. |
| [Constant](#constant)| Wraps a fixed value in a Fuzzr, producing the same result every time. |
### One
Creates a Fuzzr that produces complete instances of type `T` using QuickFuzzr's automatic construction rules.  

**Signature:**  
```csharp
Fuzzr.One<T>()
```
  

**Usage:**  
```csharp
Fuzzr.One<Person>();
// Results in => { Name: "ddnegsn", Age: 18 }
```
 - Uses `T`'s public parameterless constructor. Parameterized ctors aren't auto-filled unless configured.  
- Primitive properties are generated using their default `Fuzzr` equivalents.  
- Enumerations are filled using `Fuzzr.Enum<T>()`.  
- Object properties are generated where possible.  
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  
- Field generation is not supported.  

**Overloads:**  
- `Fuzzr.One<T>(Func<T> constructor)`:  
  Creates a Fuzzr that produces instances of T by invoking the supplied factory on each generation.  

**Exceptions:**  
- `ConstructionException`: When type T cannot be constructed due to missing default constructor.  
- `InstantiationException`: When type T is an interface and cannot be instantiated.  
- `FactoryConstructionException`: When the factory method returns `null`.  
- `ArgumentNullException`: When the factory method is `null`.  
### OneOf
Creates a Fuzzr that randomly selects one value or Fuzzr from the provided options.  

**Signature:**  
```csharp
Fuzzr.OneOf(params T[] values)
```
  

**Usage:**  
```csharp
 Fuzzr.OneOf("a", "b", "c");
```
- Selection is uniform unless weights are specified (see below).  
- **null items** are allowed in `params T[] values`.  

**Overloads:**  
- `Fuzzr.OneOf(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  
- `Fuzzr.OneOf(params FuzzrOf<T>[] fuzzrs)`:  
  Randomly selects and executes one of the provided Fuzzrs.  
- `Fuzzr.OneOf(params (int Weight, T Value)[] weightedValues)`:  
  Selects a value using weighted probability. The higher the weight, the more likely the value is to be chosen.  
```csharp
 Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c"));
```
- `Fuzzr.OneOf(params (int Weight, FuzzrOf<T> fuzzr)[] weightedFuzzrs)`:  
  Like the weighted values overload, but applies weights to Fuzzrs.  

**Exceptions:**  
  - `OneOfEmptyOptionsException`: When trying to choose from an empty collection.  
  - `NegativeWeightException`: When one or more weights are negative.  
  - `ZeroTotalWeightException`: When the total of all weights is zero or negative.  
  - `ArgumentNullException`: When the provided sequence is null.  
### Shuffle
Creates a Fuzzr that produces a random permutation of the provided sequence.  
Use for randomized ordering, unbiased sampling without replacement.
  

**Signature:**  
```csharp
Fuzzr.Shuffle(params T[] values)
```
  

**Usage:**  
```csharp
Fuzzr.Shuffle("John", "Paul", "George", "Ringo");
// Results in => ["Paul", "Ringo", "John", "George"]
```
- If the input sequence is empty, the result is also empty.  

**Overloads:**  
- `Shuffle<T>(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  

**Exceptions:**  
- `ArgumentNullException`: When the input collection is `null`.  
### Counter
This Fuzzr returns an `int` starting at 1, and incrementing by 1 on each call.  
Useful for generating unique sequential IDs or counters.  
  

**Signature:**  
```csharp
Fuzzr.Counter(object key)
```
  

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
### Constant
This Fuzzr wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined Fuzzrs.  

**Signature:**  
```csharp
Fuzzr.Constant(T value)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(42);
// Results in => 42
```
## Configuring
`Configr` provides a configuration pipeline to influence how QuickFuzzr builds objects.
Use it to set global defaults, customize properties, control recursion depth,
select derived types, or wire dynamic behaviors that apply when calling `Fuzzr.One<T>()`.
  
### Contents
| Configr| Description |
| -| - |
| [Configr&lt;T&gt;.Ignore](#configrtignore)| Ignores one specific property on type T during generation. |
| [Configr.Ignore](#configrignore)| Globally ignores all properties matching the predicate. |
| [Configr&lt;T&gt;.IgnoreAll](#configrtignoreall)| Ignores all properties of type T. |
| [Configr.IgnoreAll](#configrignoreall)| Disables auto-generation for all properties on all types. |
| [Configr&lt;T&gt;.Property](#configrtproperty)| Sets a custom Fuzzr or value for one property on type T. |
| [Configr.Property](#configrproperty)| Applies a custom Fuzzr or value to all matching properties across all types. |
| [Configr&lt;T&gt;.Construct](#configrtconstruct)| Registers which constructor QuickFuzzr should use for type T. |
| [Configr&lt;T&gt;AsOneOf](#configrtasoneof)| Chooses randomly between the given derived types when generating T. |
| [Configr&lt;T&gt;.EndOn](#configrtendon)| Replaces deeper recursion with the specified end type. |
| [Configr&lt;T&gt;.Depth](#configrtdepth)| Sets min and max recursion depth for type T. |
| [Configr.RetryLimit](#configrretrylimit)| Sets the global retry limit for retry-based Fuzzrs. |
| [Configr&lt;T&gt;.Apply](#configrtapply)| Registers an action executed for each generated value of type `T`. |
| [Configr&lt;T&gt;.With](#configrtwith)| Applies configuration for T based on a generated value. |
| [Configr.Primitive](#configrprimitive)| Overrides the default Fuzzr for a primitive type. |
| [Property Access](#property-access)| Controls auto-generation for specific property access levels. |
### Configr&lt;T&gt;.Ignore
The property specified will be ignored during generation.  

**Signature:**  
```csharp
Configr<T>.Ignore(Expression<Func<T, TProperty>> expression)
```
  

**Usage:**  
```csharp
from ignore in Configr<Person>.Ignore(a => a.Name)
from person in Fuzzr.One<Person>()
select person;
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
- Derived classes generated also ignore the base property.  

**Exceptions:**  
  - `ArgumentNullException`: When the expression is `null`.  
### Configr.Ignore
Skips all properties matching the predicate across all types during generation.  
Use to exclude recurring patterns like identifiers, foreign keys, or audit fields.  

**Signature:**  
```csharp
Configr.Ignore(Func<PropertyInfo, bool> predicate)
```
  

**Usage:**  
```csharp
from ignore in Configr.Ignore(a => a.Name == "Name")
from person in Fuzzr.One<Person>()
from fileEntry in Fuzzr.One<FileEntry>()
select (person, fileEntry);
// Results in => 
// ( Person { Name: "", Age: 67 }, FileEntry { Name: "" } )
```

**Exceptions:**  
  - `ArgumentNullException`: When the expression is `null`.  
### Configr&lt;T&gt;.IgnoreAll
Ignore all properties while generating an object.  

**Signature:**  
```csharp
Configr<T>.IgnoreAll()
```
  

**Usage:**  
```csharp
from ignore in Configr<Person>.IgnoreAll()
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
- `IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.  
### Configr.IgnoreAll
Ignore all properties while generating anything.  

**Signature:**  
```csharp
Configr.IgnoreAll()
```
  

**Usage:**  
```csharp
from ignore in Configr.IgnoreAll()
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
### Configr&lt;T&gt;.Property
The property specified will be generated using the passed in Fuzzr.  

**Signature:**  
```csharp
Configr<T>.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)
```
  

**Usage:**  
```csharp
 Configr<Person>.Property(s => s.Age, Fuzzr.Constant(42));
```
- Derived classes generated also use the custom property.  

**Overloads:**  
- `Configr<T>.Property<TProperty>(Func<PropertyInfo, bool> predicate, TProperty value)`  
  Allows for passing a value instead of a Fuzzr.  

**Exceptions:**  
- `ArgumentNullException`: When the expression is `null`.  
- `ArgumentNullException`: When the Fuzzr is `null`.  
- `PropertyConfigurationException`: When the expression points to a field instead of a property.  
### Configr.Property
Any property matching the predicate will use the specified Fuzzr during generation.  

**Signature:**  
```csharp
Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)
```
  

**Usage:**  
```csharp
 Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
```

**Overloads:**  
- `Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)`  
  Allows you to pass in a value instead of a Fuzzr.  
- `Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, FuzzrOf<TProperty>> factory)`  
  Allows you to create a Fuzzr dynamically using a factory method.  
- `Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, TProperty> factory)`  
  Allows you to create a value dynamically using a factory method.  

**Exceptions:**  
- `ArgumentNullException`: When the predicate is `null`.  
- `ArgumentNullException`: When the Fuzzr is `null`.  
- `ArgumentNullException`: When the factory function is `null`.  
### Configr&lt;T&gt;.Construct
Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
  

**Signature:**  
```csharp
Configr<T>.Construct(FuzzrOf<T1> arg1);
```
  

**Usage:**  
```csharp
Configr<MultiCtorContainer>.Construct(Fuzzr.Constant(42));
```

**Overloads:**  
- `Construct<T1, T2>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2)`  
- `Construct<T1, T2, T3>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3)`  
- `Construct<T1, T2, T3, T4>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4)`  
- `Construct<T1, T2, T3, T4, T5>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4, FuzzrOf<T5> arg5)`  

**Exceptions:**  
- `ArgumentNullException`: If one of the `TArg` parameters is null.  
- `ConstructorNotFoundException`: If no matching constructor is found on type T.  
### Configr&lt;T&gt;AsOneOf
Configures inheritance resolution for BaseType, 
allowing QuickFuzzr to randomly select one of the specified derived types when generating instances.  

Useful when generating domain hierarchies where multiple concrete subtypes exist.  
  

**Signature:**  
```csharp
Configr<T>AsOneOf(params Type[] types)
```
  

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
- `DerivedTypeNotAssignableException`: If any listed type is not a valid subclass of `BaseType`.  
- `DerivedTypeIsNullException`: If any listed type is `null`.  
### Configr&lt;T&gt;.EndOn
Configures a recursion stop condition for type `T`,
instructing QuickFuzzr to generate `TEnd` instances instead of continuing deeper.  

**Signature:**  
```csharp
Configr<T>.EndOn<TEnd>()
```
  
  

**Usage:**  
```csharp
from ending in Configr<Turtle>.EndOn<MoreTurtles>()
from turtle in Fuzzr.One<Turtle>()
select turtle;
// Results in => 
// MoreTurtles { Down: null }
```
With depth constraints, QuickFuzzr respects the specified min/max depth when applying the `EndOn<TEnd>()` rule.  
```csharp
from depth in Configr<Turtle>.Depth(1, 3)
from ending in Configr<Turtle>.EndOn<MoreTurtles>()
from turtle in Fuzzr.One<Turtle>()
select turtle;
// Results in => 
// Turtle { Down: Turtle { Down: MoreTurtles { Down: null } } }
```

**Exceptions:**  
- `DerivedTypeNotAssignableException`: When `TEnd` is not assignable to `T`.  
### Configr&lt;T&gt;.Depth

**Signature:**  
```csharp
Configr<T>.Depth(int min, int max)
```
  
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
### Configr.RetryLimit
Sets the global retry limit used by Fuzzrs.  

**Signature:**  
```csharp
Configr.RetryLimit(int limit)
```
  

**Usage:**  
```csharp
 Configr.RetryLimit(256);
```
- Throws when trying to set limit to a value lesser than 1.  
- Throws when trying to set limit to a value greater than 1024.  
```text
Invalid retry limit value: 1025
Allowed range: 1-1024
Possible solutions:
- Use a value within the allowed range
- Check for unintended configuration overrides
- If you need more, consider revising your Fuzzr logic instead of increasing the limit
```
### Configr&lt;T&gt;.Apply
Registers an action executed for each generated value of type `T` without modifying the value itself. Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.  

**Signature:**  
```csharp
Configr<T>.Apply(Action<T> action)
```
  

**Usage:**  
```csharp
var seen = new List<Person>();
var fuzzr =
    from look in Configr<Person>.Apply(seen.Add)
    from person in Fuzzr.One<Person>()
    from employee in Fuzzr.One<Employee>()
    select (person, employee);
var value = fuzzr.Generate();
// seen now equals 
// [ ( 
//     Person { Name: "ddnegsn", Age: 18 },
//     Employee { Email: "ggnijgna", SocialSecurityNumber: "pkdcsvobs", Name: "xs", Age: 52 }
//) ]
```

**Exceptions:**  
- `ArgumentNullException`: When the provided Action is `null`.  
### Configr&lt;T&gt;.With

**Signature:**  
```csharp
Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)
```
  
Applies configuration for type `T` based on a value produced by another Fuzzr,
allowing dynamic, data-dependent configuration inside LINQ chains.
  

**Exceptions:**  
- `NullReferenceException`: When the provided `Fuzzr` is null.  
- `NullReferenceException`: When the provided `Configr` factory is null.  
### Configr.Primitive
Registers a global default Fuzzr for primitive types.
Use this to override how QuickFuzzr generates built-in types across all automatically created objects.
  

**Signature:**  
```csharp
Configr.Primitive<T>(this FuzzrOf<T> fuzzr)
```
  

**Usage:**  
```csharp
from cfgInt in Configr.Primitive(Fuzzr.Constant(42))
from person in Fuzzr.One<Person>()
from timeslot in Fuzzr.One<TimeSlot>()
select (person, timeslot);
// Results in => 
// ( Person { Name: "ddnegsn", Age: 42 }, TimeSlot { Day: Monday, Time: 42 } )
```
- Replacing a primitive Fuzzr automatically impacts its nullable counterpart.  

**Overloads:**  
- `Primitive<T>(this FuzzrOf<T?> fuzzr)`:  
  Registers a global default Fuzzr for nullable primitives `T?`, overriding all nullable values produced across generated objects.  
  Replacing a nullable primitive Fuzzr does not impacts it's non-nullable counterpart.  
```csharp
from cfgString in Configr.Primitive(Fuzzr.Constant<int?>(42))
from person in Fuzzr.One<Person>()
from nullablePerson in Fuzzr.One<NullablePerson>()
select (person, nullablePerson);
// Results in => 
// ( Person { Name: "cmu", Age: 66 }, NullablePerson { Name: "ycqa", Age: 42 } )
```
- `Fuzzr.Primitive(this FuzzrOf<string> fuzzr)`:  
  Registers a global default Fuzzr for strings, overriding all string values produced across generated objects.  
```csharp
from cfgString in Configr.Primitive(Fuzzr.Constant("FIXED"))
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "FIXED", Age: 67 }, Address { Street: "FIXED", City: "FIXED" } )
```

**Exceptions:**  
- `NullReferenceException`: When the provided `Fuzzr` is null.  
### Property Access
Control which kinds of properties QuickFuzzr is allowed to populate.  

**Signature:**
```csharp
Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) 
Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)
```  

**Usage:**  
```csharp
from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
from person1 in Fuzzr.One<PrivatePerson>()
from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
from person2 in Fuzzr.One<PrivatePerson>()
select (person1, person2);
// Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
```
- Updates state flags using bitwise enable/disable semantics.  
- The default value is `PropertyAccess.PublicSetters`.  
- `ReadOnly` only applies to get-only **auto-properties**.  
- Getter-only properties *without* a compiler-generated backing field (i.e.: calculated or manually-backed) are never auto-generated.  
## Fuzzr Extension Methods
QuickFuzzr provides a collection of extension methods that enhance the expressiveness and composability of `FuzzrOf<T>`.
These methods act as modifiers, they wrap existing Fuzzrs to alter behavior, add constraints,
or chain side-effects without changing the underlying LINQ-based model.
  
### Contents
| Method| Description |
| -| - |
| [Apply](#apply)| Executes a side-effect for, or transform each value generated by the wrapped fuzzr. |
| [AsObject](#asobject)| Boxes generated values as `object` without modifying them. |
| [Many](#many)| Produces a number of values from the wrapped Fuzzr. |
| [NeverReturnNull](#neverreturnnull)| Filters out null values, retrying until a non-null value is produced or the retry limit is exceeded. |
| [Nullable](#nullable)| Converts a non-nullable value-type Fuzzr into a nullable one with a default 20% null probability. |
| [NullableRef](#nullableref)| Wraps a reference-type Fuzzr to sometimes return null (default 20% chance). |
| [Shuffle](#shuffle)| Randomly shuffles the sequence produced by the source Fuzzr. |
| [Unique](#unique)| Ensures all generated values are unique within the given key scope. |
| [Where](#where)| Filters generated values so only values satisfying the predicate are returned. |
| [WithDefault](#withdefault)| Returns a default value when the underlying Fuzzr fails due to empty choices. |
### Apply
Executes a side-effect per generated value without altering it.  

**Signature:**  
```csharp
ExtFuzzr.Apply(this FuzzrOf<T> fuzzr, Action<T> action)
```
  

**Usage:**  
```csharp
var seen = new List<int>();
var fuzzr = Fuzzr.Int().Apply(seen.Add);
var value = fuzzr.Generate();
// seen now equals [ 67 ]
```

**Overloads:**  
- `Apply(this FuzzrOf<T> fuzzr, Func<T,T> func)`  
  Transforms generated values while preserving generation context.  
```csharp
Fuzzr.Constant(41).Apply(x => x + 1);
// Results in => 42
```

**Exceptions:**  
- `ArgumentNullException`: When the provided `Action` or `Func` is null.  
### AsObject
Boxes generated values as object without altering them.  

**Signature:**  
```csharp
ExtFuzzr.AsObject<T>(this FuzzrOf<T> fuzzr)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(42).AsObject();
// Results in => 42
```
### Many
Produces a fixed number of values from a Fuzzr.  

**Signature:**  
```csharp
ExtFuzzr.Many(this FuzzrOf<T> fuzzr, int number)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(6).Many(3);
// Results in => [6, 6, 6]
```

**Overloads:**  
- `Many(this FuzzrOf<T> fuzzr, int min, int max)`  
  Produces a variable number of values within bounds.  
### NeverReturnNull
Filters out nulls from a nullable Fuzzr, retrying up to the retry limit.  

**Signature:**  
```csharp
ExtFuzzr.NeverReturnNull<T>(this FuzzrOf<T?> fuzzr)
```
  

**Exceptions:**  
- `NonNullValueExhaustedException`: When all attempts result in null.  
### Nullable
Wraps a value type Fuzzr to sometimes yield null values.  

**Signature:**  
```csharp
ExtFuzzr.Nullable(this FuzzrOf<T> fuzzr)
```
  
### NullableRef
Wraps a reference type Fuzzr to sometimes yield null values.  

**Signature:**  
```csharp
ExtFuzzr.NullableRef(this FuzzrOf<T> fuzzr)
```
  
### Shuffle
Randomly shuffles the sequence produced by the source Fuzzr.  

**Signature:**  
```csharp
ExFuzzr.Shuffle<T>(this FuzzrOf<IEnumerable<T>> source)
```
  

**Usage:**  
```csharp
Fuzzr.Counter("num").Many(4).Shuffle();
// Results in => [ 2, 4, 1, 3 ]
```
- Preserves the elements of the source sequence.  
### Unique
Makes sure that every generated value is unique.  

**Signature:**  
```csharp
ExtFuzzr.Unique<T>(this FuzzrOf<T> fuzzr, object key)
```
  
- Multiple unique Fuzzrs can be defined in one 'composed' Fuzzr, without interfering with eachother by using a different key.  
- When using the same key for multiple unique Fuzzrs all values across these Fuzzrs are unique.  

**Overloads:**  
- `Unique<T>(this FuzzrOf<T> fuzzr, object key, int maxAttempts)`  
  Overwrites the global retry limit with the provided value.  
- `Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key)`  
  Takes a function as an argument allowing for a dynamic key.  

**Overloads:**  
- `Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key, int maxAttempts)`  
  Overwrites the global retry limit with the provided value.  

**Exceptions:**  
- `UniqueValueExhaustedException`: When the Fuzzr cannot find enough unique values within the retry limit.   
- `NullReferenceException`: When the provided `Fuzzr` is null.  
### Where

**Signature:**  
```csharp
ExtFuzzr.Where(this FuzzrOf<T> fuzzr, Func<T,bool> predicate)
```
  
Filters generated values to those satisfying the predicate.  

**Exceptions:**  
- `PredicateUnsatisfiedException`: When no value satisfies the predicate within the retry limit.  
- `ArgumentNullException`: When the predicate is `null`.  
### WithDefault
Returns the (optionally) provided default value instead of throwing when the underlying Fuzzr fails due to empty choices.  

**Signature:**  
```csharp
ExtFuzzr.WithDefault(this FuzzrOf<T> fuzzr, T def = default)
```
  
## Primitive Fuzzrs
QuickFuzzr includes built-in Fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex Fuzzrs are composed, and are used automatically when generating object properties.

> *All primitive Fuzzrs automatically drive object property generation.
> Nullable and non-nullable variants are both supported.
> Each Fuzzr also supports `.Nullable(...)` / `.NullableRef(...)` as appropriate.*

In this reference we categorize these Fuzzrs as follows:  
### Ranged Primitives
Ranged primitives generate numeric or temporal values between a minimum and a maximum.

For all of these the following is true:
- They have a paremeterless function whcich returns a value in a default range.
- They have an overload which allows you to specify a min and max value.
- They throw a `ArgumentOutOfRangeException` if the min is greater than the max.

Furthermore, there are two types of ranged primitives:
  
#### Continuous
Values come from a dense numeric space (floating-point types).

For these, the upper bound is conceptually exclusive ([min, max)),
but floating-point rounding may occasionally allow max to appear.
This behaviour is explicitly tested and documented.
  
##### Decimals
Use `Fuzzr.Decimal()`.  
**Default Range and Precision:** min = 1, max = 100, precision = 2).  
Apart from the usual ranged primitive min/max overload, `Fuzzr.Decimal()` adds two more allowing the user to specify a precision.  
- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.  
- Throws an `ArgumentException` when `precision` < `0`.  
- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal in the range [min, max) (min inclusive, max exclusive), with up to `precision` decimal places.  
- Throws an `ArgumentException` when `precision` < `0`.  
##### Doubles
Use `Fuzzr.Double()`.  
- **Default Range:** min = 1, max = 100).  
##### Floats
Use `Fuzzr.Float()`.  
- **Default Range:** min = 1, max = 100).  
##### Halfs
Use `Fuzzr.Half()`.  
- **Default Range:** min = 1, max = 100).  
*Note:* Due to floating-point rounding, max may occasionally be produced.  
#### Discrete
Values come from a countable set (integers, shorts, bytes, dates snapped to seconds, etc.).

These appear in two flavours:  
##### Inclusive upper bound.
The maximum value can be produced.  
Used when C# conventions or data types naturally use [min, max] (e.g. DateOnly, Byte, Char).  
##### Bytes
Use `Fuzzr.Byte()`.  
- **Default Range:** min = 1, max = 255.  
##### Chars
Use `Fuzzr.Char()`.  
- **Default Range:** min = 'a', max = 'z'.  
##### DateOnlys
Use `Fuzzr.DateOnly()`.  
- **Default Range:** min = 'DateOnly(1970, 1, 1)', max = 'DateOnly(2020, 12, 31)'.  
##### DateTimes
Use `Fuzzr.DateTime()`.  
- **Default Range:** min = 'DateOnly(1970, 1, 1)', max = 'DateOnly(2020, 12, 31)'.  
- Resulting values are snapped to whole seconds.  
##### Exclusive upper bound.
The maximum value is never produced.  
Matches the C# RNG convention used by Random.Next(min, max) and applies to most integer fuzzrs.  
##### Ints
Use `Fuzzr.Int()`.  
- **Default Range:** min = 1, max = 100).  
##### Longs
Use `Fuzzr.Long()`.  
- **Default: Range** min = 1, max = 100).  
##### Shorts
Use `Fuzzr.Short()`.  
- **Default Range:** min = 1, max = 100).  
##### TimeOnlys
Use `Fuzzr.TimeOnly()`.  
- **Default Range:** min = 00:00:00, max = 23:59:59.9999999).  
##### TimeSpans
Use `Fuzzr.TimeSpan()`.  
- **Default Range:** min = 1, max = 1000).  
##### UInts
Use `Fuzzr.UInt()`.  
- **Default Range:** min = 1, max = 100).  
##### ULongs
Use `Fuzzr.ULong()`.  
- **Default Range:** min = 1, max = 100).  
##### UShorts
Use `Fuzzr.UShort()`.  
- **Default Range:** min = 1, max = 100).  
### Non Ranged Primitives
#### Booleans
Use `Fuzzr.Bool()`.  
- Generates `true` or `false`.  
#### Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default Fuzzr just picks a random value from all enumeration values.  
- Passing in a non Enum type for T throws an ArgumentException.  
#### Guids
Use `Fuzzr.Guid()`.  
- The default Fuzzr never generates Guid.Empty.  
- `Fuzzr.Guid()` is deterministic when seeded.  
#### Strings
Use `Fuzzr.String()`.  
- The Default Fuzzr generates a string of length greater than or equal to 1 and less than or equal to 10.  
- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.  
- Throws an `ArgumentOutOfRangeException` when `length` < 0.  
- The default Fuzzr always generates every char element of the string to be between lower case 'a' and lower case 'z'.  
- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.  
