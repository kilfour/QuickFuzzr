# Quick Fuzzr
> A type-walking cheetah with a hand full of random.
## Introduction
An evolution from the QuickMGenerate library.
## Installation
QuickFuzzrQuickFuzzr is available on NuGet:
```bash
Install-Package QuickFuzzr
```
Or via the .NET CLI:
```bash
dotnet add package QuickFuzzr
```
## Generating Primitives
The Fuzz class has many methods which can be used to obtain a corresponding primitive.
F.i. `Fuzz.Int()`. 

Full details below in the chapter 'The Primitive Generators'.
## Combining Generators
### Linq Syntax
Each Fuzz Generator can be used as a building block and combined using query expressions.
F.i. the following :
```
var stringGenerator =
	from a in Fuzz.Int()
	from b in Fuzz.String()
	from c in Fuzz.Int()
	select a + b + c;
Console.WriteLine(stringGenerator.Generate());
```
Will output something like `28ziicuiq56`.
Generators are reusable building blocks. 

In the following :
```
var generator =
	from str in stringGenerator.Replace()
	from thing in Fuzz.One<SomeThingToGenerate>()
	select thing;
```
We reuse the 'stringGenerator' defined above and replace the default string generator with our custom one. 
All strings in the generated object will have the pattern defined by 'stringGenerator'.
This approach removes the problem of combinatoral explosion. No need for a Transform<T, U>(...) combinator for example
as this can be easily achieved using Linq. 

```
var generator =
	from chars in Fuzz.Constant('-').Many(5)
	let composed = chars.Aggregate(", (a, b) => a + b.ToString())
	select composed;
```
Generates: "-----".
### Using Extensions
When applying the various extension methods onto a generator, they get *combined* into a new generator.
Jumping slightly ahead of ourselves as below example will use methods that are explained more thoroughly further below.

E.g. :
```
Fuzz.ChooseFrom(someValues).Unique("key").Many(2)
```

## Generating Objects
### A Simple Object
Use `Fuzz.One<T>()`, where T is the type of object you want to generate.
- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.
- The enumeration properties of the object will be automatically filled in using the default (or replaced) Fuzz.Enum<T> generator.
- The object properties will also be automatically filled in using the default (or replaced) generators, similar to calling Fuzz.One<TProperty>() and setting the value using `Apply` (see below) explicitly.
- Also works for properties with private setters.
- Can generate any object that has a parameterless constructor, be it public, protected, or private.
- The overload `Fuzz.One<T>(Func<T> constructor)` allows for specific constructor selection.
### Ignoring Properties
Use the `Fuzz.For<T>().Ignore<TProperty>(Expression<Func<T, TProperty>> func)` method chain.

F.i. :
```
Fuzz.For<SomeThingToGenerate>().Ignore(s => s.Id)
```
The property specified will be ignored during generation.
Derived classes generated also ignore the base property.
Sometimes it is useful to ignore all properties while generating an object.  
For this use `Fuzz.For<SomeThingToGenerate>().IgnoreAll()`
`IgnoreAll()` does not ignore properties on derived classes, even inherited properties.
**Note :** `The Ignore(...)` combinator does not actually generate anything, it only influences further generation.
### Customizing Properties
Use the `Fuzz.For<T>().Customize<TProperty>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
Fuzz.For<SomeThingToGenerate>().Customize(s => s.MyProperty, Fuzz.Constant(42))
```
The property specified will be generated using the passed in generator.
An overload exists which allows for passing a value instead of a generator.
Derived classes generated also use the custom property.
*Note :* The Customize combinator does not actually generate anything, it only influences further generation.
### Customizing Constructors
Use the `Fuzz.For<T>().Construct<TArg>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
Fuzz.For<SomeThing>().Construct(Fuzz.Constant(42))
```
Subsequent calls to `Fuzz.One<T>()` will then use the registered constructor.
Various overloads exist : 
 -  `Fuzz.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2)`
 -  `Fuzz.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3)`
 -  `Fuzz.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4)`
 -  `Fuzz.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4, Generator<T5> g5)`  

After that, ... you're on your own.
Or use the factory method overload:  
`Fuzz.For<T>().Construct<T>(Func<T> ctor)`
*Note :* The Construct combinator does not actually generate anything, it only influences further generation.
### Many Objects
Use The `.Many(int number)` generator extension.
The generator will generate an IEnumerable<T> of `int number` elements where T is the result type of the extended generator.
An overload exists (`.Many(int min, int max`) where the number of elements is in between the specified arguments.
### Inheritance
Use The `Fuzz.For<T>().GenerateAsOneOf(params Type[] types)` method chain.

F.i. :
```
Fuzz.For<SomeThingAbstract>().GenerateAsOneOf(
	typeof(SomethingDerived), typeof(SomethingElseDerived))
```
When generating an object of type T, an object of a random chosen type from the provided list will be generated instead.
**Note :** The `GenerateAsOneOf(...)` combinator does not actually generate anything, it only influences further generation.
### To Array
Use The `.ToArray()` generator extension.
The `Many` generator above returns an IEnumerable.
This means it's value would be regenerated if we were to iterate over it more than once.
Use `ToArray` to *fix* the IEnumerable in place, so that it will return the same result with each iteration.
It can also be used to force evaluation in case the IEnumerable is not enumerated over because there's nothing in your select clause
referencing it. 

### To List
Use The `.ToList()` generator extension.
Similar to the `ToArray` method. But instead of an Array, this one returns, you guessed it, a List.
### Replacing Primitive Generators
Use the `.Replace()` extension method.
Example
```
var generator =
	from _ in Fuzz.Constant(42).Replace()
	from result in Fuzz.One<SomeThingToGenerate>()
	select result;
```
When executing above generator it will return a SomeThingToGenerate object where all integers have the value 42.

Replacing a primitive generator automatically impacts its nullable counterpart.
Replacing a nullable primitive generator does not impacts it's non-nullable counterpart.
Replacements can occur multiple times during one generation :
```
var generator =
	from _ in Fuzz.Constant(42).Replace()
	from result1 in Fuzz.One<SomeThingToGenerate>()
	from __ in Fuzz.Constant(666).Replace()
	from result2 in Fuzz.One<SomeThingToGenerate>()
	select new[] { result1, result2
};
```
When executing above generator result1 will have all integers set to 42 and result2 to 666.
*Note :* The Replace combinator does not actually generate anything, it only influences further generation.
