Shuffle

3. Circular Reference Handling

You mention avoiding infinite recursion, but what about circular references that aren't recursive?

csharp
class A { public B Child { get; set; } }
class B { public A Parent { get; set; } }


2. Method Naming Inconsistencies
    TreeLeaf<T>() - the name doesn't clearly communicate it's for constraining generation

WEIGHTS

Built-in counters


Fuzzr.Byte()

Fuzzr.SByte()

Fuzzr.UInt16()

Fuzzr.UInt32()

Fuzzr.UInt64()

Fuzzr.DateTimeOffset()

Fuzzr.BigInteger()

Fuzzr.Half()

Fuzzr.Rune()

Fuzzr.Bytes()


