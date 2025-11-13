# Fuzzr Extension Methods
QuickFuzzr provides a collection of extension methods that enhance the expressiveness and composability of `FuzzrOf<T>`.
These methods act as modifiers, they wrap existing fuzzrs to alter behavior, add constraints,
or chain side-effects without changing the underlying LINQ-based model.
  
## Contents
| Method| Description |
| -| - |
| [ExtFuzzr.Apply(this FuzzrOf&lt;T&gt; fuzzr, Action&lt;T&gt; action)](Methods/ExtFuzzrApply.md)|   |
| [ExtFuzzr.AsObject(this FuzzrOf&lt;T&gt; fuzzr)](Methods/ExtFuzzrAsObject.md)|   |
| [ExtFuzzr.Many(this FuzzrOf&lt;T&gt; fuzzr, int number)](Methods/ExtFuzzrMany.md)|   |
| [ExtFuzzr.NeverReturnNull(this FuzzrOf&lt;T?&gt; fuzzr)](Methods/ExtFuzzrNeverReturnNull.md)|   |
| [ExtFuzzr.Nullable(this FuzzrOf&lt;T&gt; fuzzr)](Methods/ExtFuzzrNullable.md)|   |
| [ExtFuzzr.NullableRef(this FuzzrOf<T> fuzzr)](Methods/ExtFuzzrNullableRef.md)|   |
| [ExtFuzzr.Shuffle&lt;T&gt;(this FuzzrOf&lt;IEnumerable&lt;T&gt;&gt; source)](Methods/ExtFuzzrShuffle.md)|   |
| [ExtFuzzr.Unique&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr, object key)](Methods/ExtFuzzrUnique.md)|   |
| [ExtFuzzr.Where(this FuzzrOf&lt;T&gt; fuzzr, Func&lt;T,bool&gt; predicate)](Methods/ExtFuzzrWhere.md)|   |
| [ExtFuzzr.WithDefault(this FuzzrOf&lt;T&gt; fuzzr, T def = default)](Methods/ExtFuzzrWithDefault.md)|   |
