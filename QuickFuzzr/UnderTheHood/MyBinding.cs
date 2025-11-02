using System.Reflection;

namespace QuickFuzzr.UnderTheHood;

public static class MyBinding
{
	// public const BindingFlags Flags =
	// 	BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
	public const BindingFlags Flags =
		BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
}