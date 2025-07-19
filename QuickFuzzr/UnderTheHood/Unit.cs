namespace QuickFuzzr.UnderTheHood;

public readonly struct Unit : IComparable<Unit>, IEquatable<Unit>
{
	public static readonly Unit Instance = new();

	public static bool operator ==(Unit left, Unit right) => true;
	public static bool operator !=(Unit left, Unit right) => false;

	public override bool Equals(object? obj) => obj is Unit;
	public override int GetHashCode() => 0;

	public int CompareTo(Unit other) => 0;
	public bool Equals(Unit other) => true;
}
