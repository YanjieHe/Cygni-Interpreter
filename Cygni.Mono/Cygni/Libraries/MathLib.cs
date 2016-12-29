using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Errors;

namespace Cygni.Libraries
{
	/// <summary>
	/// Description of MathLib.
	/// </summary>
	public static class MathLib
	{
		public static DynValue sqrt (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "sqrt");
			return Math.Sqrt (args [0].AsNumber ());
		}

		public static DynValue abs (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "abs");
			return Math.Abs (args [0].AsNumber ());
		}

		public static DynValue log (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "log");
			if (args.Length == 1)
				return Math.Log (args [0].AsNumber ());
			else
				return Math.Log (args [0].AsNumber (), args [1].AsNumber ());
		}

		public static DynValue log10 (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "log10");
			return Math.Log10 (args [0].AsNumber ());
		}

		public static DynValue max (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 2, "max");
			double max_number = args [0].AsNumber ();
			for (int i = 1; i < args.Length; i++) {
				double x = args [i].AsNumber ();
				if (max_number < x) {
					max_number = x;
				}
			}
			return max_number;
		}

		public static DynValue min (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "min");
			double min_number = args [0].AsNumber ();
			for (int i = 1; i < args.Length; i++) {
				double x = args [i].AsNumber ();
				if (min_number > x) {
					min_number = x;
				}
			}
			return min_number;
		}

		public static DynValue exp (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "exp");
			return Math.Exp (args [0].AsNumber ());
		}

		public static DynValue sign (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "sign");
			return (long)Math.Sign (args [0].AsNumber ());
		}

		public static DynValue sin (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "sin");
			return Math.Sin (args [0].AsNumber ());
		}

		public static DynValue cos (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "cos");
			return Math.Cos (args [0].AsNumber ());
		}

		public static DynValue tan (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "tan");
			return Math.Tan (args [0].AsNumber ());
		}

		public static DynValue asin (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "asin");
			return Math.Asin (args [0].AsNumber ());
		}

		public static DynValue acos (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "acos");
			return Math.Acos (args [0].AsNumber ());
		}

		public static DynValue atan (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "atan");
			return Math.Atan (args [0].AsNumber ());
		}

		public static DynValue sinh (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "sinh");
			return Math.Sinh (args [0].AsNumber ());
		}

		public static DynValue cosh (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "cosh");
			return Math.Cosh (args [0].AsNumber ());
		}

		public static DynValue tanh (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "tanh");
			return Math.Tanh (args [0].AsNumber ());
		}

		public static DynValue ceiling (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "ceiling");
			return Math.Ceiling (args [0].AsNumber ());
		}

		public static DynValue floor (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "floor");
			return Math.Floor (args [0].AsNumber ());
		}

		public static DynValue round (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "round");
			return Math.Round (args [0].AsNumber (), MidpointRounding.AwayFromZero);
		}

		public static DynValue truncate (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "truncate");
			return Math.Truncate (args [0].AsNumber ());
		}

		internal static long IntegerPow (long x, int n)
		{
			if (n == 0) {
				return 1L;
			} else {
				while ((n & 1) == 0) {
					n >>= 1;
					x *= x;
				}
			}
			long result = x;
			n >>= 1;
			while (n != 0) {
				x *= x;
				if ((n & 1) != 0) {
					result *= x;
				}
				n >>= 1;
			}
			return result;
		}
	}
}
