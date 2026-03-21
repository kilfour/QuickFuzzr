# Tuple
Creates a Fuzzr that combines two source Fuzzrs into a tuple of their generated values.  

**Signature:**  
```csharp
Tuple<T1, T2>(FuzzrOf<T1> fuzzrOfT1, FuzzrOf<T2> fuzzrOfT2)
```
  

**Usage:**  
```csharp
Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int());
```
The `Tuple(...)` method supports up to seven source Fuzzrs.  
