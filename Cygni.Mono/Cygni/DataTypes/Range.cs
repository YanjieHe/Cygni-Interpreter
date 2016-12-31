using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public sealed class Range:IEnumerable<DynValue>
	{
		readonly int start;
		readonly int end;
		readonly int step;

		public int Start{ get { return this.start; } }

		public int End{ get { return this.end; } }

		public int Step{ get { return this.step; } }

		public Range (int start, int end, int step = 1)
		{
			if (step == 0) {
				throw new RuntimeException ("step of range cannot be zero.");
			}
			this.start = start;
			this.end = end;
			this.step = step;
		}

		public override string ToString ()
		{
			if (step == 1) {
				return start.ToString () + ":" + end.ToString ();
			} else {
				return start.ToString () + ":" + end.ToString () + ":" + step.ToString ();
			}
		}

		public IEnumerator<DynValue> GetEnumerator ()
		{
			if (step == 0) {
				throw new RuntimeException ("step of range cannot be zero.");
			} else if (step > 0) {
				if (start > end) {
					throw new RuntimeException ("start value should be less than or equals to the end value if the step is positive .");
				} else {
					for (int i = start; i < end; i += step) {
						yield return new DynValue (DataType.Integer, (long)i);
					}
				}
			} else {
				if (start < end) {
					throw new RuntimeException ("start value should be greater than or equals to the end value if the step is negative .");
				} else {
					for (int i = start; i > end; i += step) {
						yield return new DynValue (DataType.Integer, (long)i);
					}
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}
	}
}

