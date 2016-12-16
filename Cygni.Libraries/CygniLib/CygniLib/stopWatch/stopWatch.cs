using System;
using System.Diagnostics;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.stopWatch
{
	public class stopWatch:Stopwatch,IDot
	{
		public stopWatch () : base ()
		{
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "start":
				return DynValue.FromDelegate ("start", (args) => {
					this.Start ();
					return DynValue.Nil;
				});
			case "stop":
				return DynValue.FromDelegate ("stop", (args) => {
					this.Stop ();
					return DynValue.Nil;
				});
			case "reset":
				return DynValue.FromDelegate ("reset", (args) => {
					this.Reset ();
					return DynValue.Nil;
				});
			case "restart":
				return DynValue.FromDelegate ("restart", (args) => {
					this.Restart ();
					return DynValue.Nil;
				});
			case "elapsedMilliseconds":
				return (double)this.ElapsedMilliseconds;
			default:
				throw RuntimeException.FieldNotExist ("StopWatch", fieldName);
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("StopWatch", fieldName);
		}

		public string[] FieldNames {
			get{ return new string[]{ "start", "stop", "reset", "restart", "elapsedMilliseconds" }; }
		}

		public override string ToString ()
		{
			return "(Native Class: StopWatch)";
		}
	}
}

