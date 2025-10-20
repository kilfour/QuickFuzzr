﻿using System.Text;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<string> String()
	{
		return String(1, 10);
	}

	public static Generator<string> String(int min, int max)
	{
		return s =>
				   {
					   int numberOfChars = s.Random.Next(min, max);
					   var sb = new StringBuilder();
					   for (int i = 0; i < numberOfChars; i++)
					   {
						   sb.Append(Char()(s).Value);
					   }
					   return new Result<string>(sb.ToString(), s);
				   };
	}
}