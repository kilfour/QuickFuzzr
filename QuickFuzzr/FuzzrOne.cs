using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<T> One<T>()
		=> state =>
			new Result<T>((T)Genesis.Create(state, typeof(T)), state);

	public static FuzzrOf<T> One<T>(Func<T> constructor)
		=> state =>
			new Result<T>((T)Genesis.Create(state, typeof(T), _ => constructor()!), state);
}