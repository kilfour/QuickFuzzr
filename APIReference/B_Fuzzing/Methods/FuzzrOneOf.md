# Fuzzr.OneOf&lt;T&gt;(...)
- Trying to choose from an empty collection throws an exception with the following message:  
```text
Fuzzr.OneOf<Person> cannot select from an empty sequence.
Possible solutions:
• Provide at least one option (ensure the sequence is non-empty).
• Use a fallback: Fuzzr.OneOf(values).WithDefault()
• Guard upstream: values.Any() ? Fuzzr.OneOf(values) : Fuzzr.Constant(default!).
```
- An overload exists that takes `params (int Weight, T Value)[] values` arguments in order to influence the distribution of generated values.  
- An overload exists that takes `params (int Weight, FuzzrOf<T> Generator)[] values` arguments in order to influence the distribution of generated values.  
