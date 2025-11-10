# Fuzzr.One&lt;T&gt;()
Creates a generator that produces complete instances of type T using QuickFuzzr's automatic construction rules.  
Object properties are filled using default generators for their types unless configured otherwise.  
Trying to generate an abstract class throws an exception with the following message:  
```text
Cannot generate an instance of the abstract class AbstractPerson.
Possible solution:
• Register one or more concrete subtype(s): Configr<AbstractPerson>.AsOneOf(...)
```
