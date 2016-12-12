using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using Cygni.Errors;
using Cygni.Libraries;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynValue.
	/// </summary>
	public struct DynValue:IEnumerable<DynValue>, IComparable<DynValue>, IComparer<DynValue>, IEquatable<DynValue>, IDot
	{
		readonly DataType _type;

		public DataType type { get { return _type; } }

		readonly object value;

		public object Value{ get { return value; } }

		public DynValue (DataType _type, object value)
		{
			this._type = _type;
			this.value = value;
		}

		public static readonly DynValue True = new DynValue (DataType.Boolean, true);
		public static readonly DynValue False = new DynValue (DataType.Boolean, false);
		public static readonly DynValue Nil = new DynValue (DataType.Nil, null);
		public static readonly DynValue[] Empty = new DynValue[0];

		#region implicit conversion

		public static implicit operator DynValue (double value)
		{
			return new DynValue (DataType.Number, value);
		}

		public static implicit operator DynValue (bool value)
		{
			return value ? True : False;
		}

		public static implicit operator DynValue (string value)
		{
			return new DynValue (DataType.String, value);
		}

		public static implicit operator DynValue (Function value)
		{
			return new DynValue (DataType.Function, value);
		}

		public static implicit operator DynValue (Structure value)
		{
			return new DynValue (DataType.Struct, value);
		}

		public static implicit operator DynValue (ClassInfo value)
		{
			return new DynValue (DataType.Class, value);
		}

		public static implicit operator DynValue (DynList list)
		{
			return new DynValue (DataType.List, list);
		}

		public static implicit operator DynValue (DynDictionary dictionary)
		{
			return new DynValue (DataType.Dictionary, dictionary);
		}

		public static implicit operator DynValue (NativeFunction f)
		{
			return new DynValue (DataType.NativeFunction, f);
		}

		public static implicit operator DynValue (Func<DynValue[],DynValue> f)
		{
			return new DynValue (DataType.NativeFunction, new NativeFunction (f));
		}

		#endregion

		public static explicit operator double (DynValue value)
		{
			return (double)value.value;
		}

		public static explicit operator bool (DynValue value)
		{
			return (bool)value.value;
		}

		public static explicit operator string (DynValue value)
		{
			return (string)value.value;
		}

		public static DynValue FromNumber (double value)
		{
			return new DynValue (DataType.Number, value);
		}

		public static DynValue FromBoolean (bool value)
		{
			return value ? True : False;
		}

		public static DynValue FromString (string value)
		{
			return new DynValue (DataType.String, value);
		}

		public static DynValue FromFunction (Function value)
		{
			return new DynValue (DataType.Function, value);
		}

		public static DynValue FromNativeFunction (NativeFunction value)
		{
			return new DynValue (DataType.NativeFunction, value);
		}

		public static DynValue FromDelegate (Func<DynValue[],DynValue> f)
		{
			return new DynValue (DataType.NativeFunction, new NativeFunction (f));
		}

		public static DynValue FromStructure (Structure value)
		{
			return new DynValue (DataType.Struct, value);
		}

		public static DynValue FromClass (ClassInfo value)
		{
			return new DynValue (DataType.Class, value);
		}

		public static DynValue FromList (DynList list)
		{
			return new DynValue (DataType.List, list);
		}

		public static DynValue FromDictionary (DynDictionary dictionary)
		{
			return new DynValue (DataType.Dictionary, dictionary);
		}

		public static DynValue FromUserData (object value)
		{
			return new DynValue (DataType.UserData, value);
		}

		public static DynValue FromObject (object value)
		{
			if (value == null)
				return DynValue.Nil;
			var iconv = value as IConvertible;
			if (iconv != null)
				switch (iconv.GetTypeCode ()) {
				case TypeCode.Double:
					return FromNumber ((double)value);
				case TypeCode.Boolean:
					return FromBoolean ((bool)value);
				case TypeCode.String:
					return FromString (value as string);
				case TypeCode.Int16:
					return FromNumber ((Int16)value);
				case TypeCode.Int32:
					return FromNumber ((Int32)value);
				case TypeCode.Int64:
					return FromNumber ((Int64)value);
				case TypeCode.UInt16:
					return FromNumber ((UInt16)value);
				case TypeCode.UInt32:
					return FromNumber ((UInt32)value);
				case TypeCode.UInt64:
					return FromNumber ((UInt64)value);
				case TypeCode.Single:
					return FromNumber ((Single)value);
				}
			return FromUserData (value);
		}

		public double AsNumber ()
		{
			return (double)value;
		}

		public bool AsBoolean ()
		{
			return (bool)value;
		}

		public string AsString ()
		{
			return (string)value;
		}

		public TValue As<TValue> ()
		{
			return (TValue)value;
		}

		#region IComparable implementation


		public int CompareTo (DynValue other)
		{
			switch (type) {
			case DataType.Number:
				return ((double)value).CompareTo ((double)other.value);
			case DataType.Boolean:
				return ((bool)value).CompareTo ((bool)other.value);
			case DataType.String:
				return ((string)value).CompareTo ((string)other.value);
			default:
				return (value as IComparable<DynValue>).CompareTo (other);
			}
		}

		#endregion

		public int Compare (DynValue x, DynValue y)
		{
			return x.CompareTo (y);
		}

		internal static readonly DynValue Break = new DynValue (DataType.Break, null);

		internal static readonly DynValue Continue = new DynValue (DataType.Continue, null);

		internal static DynValue Return (DynValue value)
		{
			return new DynValue (DataType.Return, value);
		}

		#region IEquatable implementation

		public bool Equals (DynValue other)
		{
			return _type == other._type && (value == null || object.Equals (value, other.value));
		}

		#endregion

		public override string ToString ()
		{
			if (value == null)
				return string.Empty;
			if (type == DataType.String)
				return string.Concat ("\"", value as string, "\"");
			return value.ToString ();
		}

		#region Equals and GetHashCode implementation

		public override bool Equals (object obj)
		{
			return (obj is DynValue) && Equals ((DynValue)obj);
		}

		public override int GetHashCode ()
		{
			return value.GetHashCode ();
		}

		public static bool operator == (DynValue lhs, DynValue rhs)
		{
			return lhs.Equals (rhs);
		}

		public static bool operator != (DynValue lhs, DynValue rhs)
		{
			return !(lhs == rhs);
		}

		#endregion

		public Type GetDynType ()
		{
			switch (_type) {
			case DataType.Number:
				return typeof(double);
			case DataType.Boolean:
				return typeof(bool);	
			case DataType.String:
				return typeof(string);
			default:
				return value.GetType ();
			}
		}

		public IEnumerator<DynValue> GetEnumerator ()
		{
			if (type == DataType.String) {
				foreach (var c in value as string) {
					yield return new DynValue (DataType.String, char.ToString (c));
				}
			} else {
				var collection = value as IEnumerable<DynValue>;
				if (collection == null)
					throw new RuntimeException ("Target is not enumerable");
				foreach (var item in collection) {
					yield return item;
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			yield return this.AsEnumerable ();
		}


		public DynValue GetByDot (string fieldName)
		{
			// support 'string' type
			string str = value as string;
			switch (fieldName) {
			case "length":
				return (double)str.Length;
			case "replace":
				return DynValue.FromDelegate ((args) => StrLib.replace (str, args));
			case "format":
				return DynValue.FromDelegate ((args) => StrLib.format (str, args));
			case "join":
				return DynValue.FromDelegate ((args) => StrLib.join (str, args));
			case "split":
				return DynValue.FromDelegate ((args) => StrLib.split (str, args));
			case "find":
				return DynValue.FromDelegate ((args) => StrLib.find (str, args));
			case "lower":
				return DynValue.FromDelegate ((args) => str.ToLower ());
			case "upper":
				return DynValue.FromDelegate ((args) => str.ToUpper ());
			case "trim":
				return DynValue.FromDelegate ((args) => StrLib.trim (str, args));
			case "trimStart":
				return DynValue.FromDelegate ((args) => StrLib.trimStart (str, args));
			case "trimEnd":
				return DynValue.FromDelegate ((args) => StrLib.trimEnd (str, args));
			case "subString":
				return DynValue.FromDelegate ((args) => StrLib.subString (str, args));
			default:
				throw RuntimeException.NotDefined (fieldName);
			}
		}

		public string[] FieldNames {
			get {
				return new string[] {
					"length", "replace", "format", "join", "split", "find", "lower", "upper", "trim", "trimStart", "trimEnd", "subString"
				};
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.NotDefined (fieldName);
		}
	}
}
