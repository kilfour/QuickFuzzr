# Configr.RetryLimit
Sets the global retry limit used by fuzzrs.  

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
- If you need more, consider revising your fuzzr logic instead of increasing the limit
```
