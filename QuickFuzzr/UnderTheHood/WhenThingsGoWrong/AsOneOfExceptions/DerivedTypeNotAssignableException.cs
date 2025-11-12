using QuickFuzzr;

namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;

/// <summary>
/// Thrown when <see cref="Configr{T}.AsOneOf"/> or <see cref="Configr{T}.EndOn"/> configuration specifies a derived type that is not assignable to the base type.
/// </summary>
public class DerivedTypeNotAssignableException(
    string baseType,
    DerivedTypeNotAssignableException.Method method,
    List<Type> nonAssignableTypes)
    : QuickFuzzrException(BuildMessage(baseType, method, nonAssignableTypes))
{
    public enum Method { AsOneOf, EndOn }
    private static string BuildMessage(
        string baseType,
        Method method,
        List<Type> nonAssignableTypes)
        => nonAssignableTypes.Count == 1
            ? BuildMessageForSingleType(baseType, MethodToString(method, nonAssignableTypes), nonAssignableTypes[0].Name)
            : BuildMessageForMultipleTypes(baseType, MethodToString(method, nonAssignableTypes), nonAssignableTypes);
    private static string MethodToString(Method method, List<Type> nonAssignableTypes) =>
        method switch
        {
            Method.AsOneOf => "AsOneOf(...)",
            Method.EndOn => $"EndOn<{nonAssignableTypes.Single().Name}>()",
            _ => "UnknownMethod"
        };
    private static string BuildMessageForSingleType(string baseType, string method, string derivedType) =>
$@"The type {derivedType} is not assignable to the base type {baseType}.

Possible solutions:
• Use a compatible type in Configr<{baseType}>.{method}.
• Ensure {derivedType} inherits from or implements {baseType}.
";

    private static string BuildMessageForMultipleTypes(string baseType, string method, List<Type> nonAssignableTypes)
    {
        var nonAssignableList = string.Join(Environment.NewLine, nonAssignableTypes.Select(t => $"• {t.Name}"));
        return
$@"The following types are not assignable to the base type {baseType}:
{nonAssignableList}

Possible solutions:
• Use compatible types in Configr<{baseType}>.{method}.
• Ensure all listed types inherit from or implement {baseType}.
";
    }
}
