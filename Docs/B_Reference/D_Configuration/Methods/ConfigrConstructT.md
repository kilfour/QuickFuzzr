# Configr&lt;T&gt;.Construct(FuzzrOf&lt;T1&gt; arg1)
Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
  

**Usage:**  
```csharp
Configr<SomeThing>.Construct(Fuzzr.Constant(42));
```

**Overloads:**  
```csharp
Construct<T1,T2>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2)
```
```csharp
Construct<T1,T2,T3>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3)
```
```csharp
Construct<T1,T2,T3,T4>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4)
```
```csharp
Construct<T1,T2,T3,T4,T5>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4, FuzzrOf<T5> arg5)
```

**Exceptions:**  
- `ArgumentNullException`: If one of the `TArg` parameters is null.  
- `InvalidOperationException`: If no matching constructor is found on type T.  
