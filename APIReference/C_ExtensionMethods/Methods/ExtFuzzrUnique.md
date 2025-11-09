# .Unique&lt;T&gt;(...)
Using the `.Unique(object key)` extension method.  
- Makes sure that every generated value is unique.  
- When asking for more unique values than the generator can supply, an exception is thrown.  
- Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.  
- When using the same key for multiple unique generators all values across these generators are unique.  
- An overload exist taking a function as an argument allowing for a dynamic key.  
