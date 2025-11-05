namespace QuickFuzzr;

/// <summary>
/// Represents a configuration action that influences generation without producing a value.
/// Use as a marker type for Configr operations that modify generation behavior rather than creating data.
/// </summary>
public readonly struct Intent
{
	/// <summary>
	/// A predefined intent value used to complete configuration expressions in LINQ queries.
	/// Use to satisfy select clauses when the actual value is unimportant but the configuration side-effect matters.
	/// </summary>
	public static readonly Intent Fixed = default;
}
