using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.AST;
using Cygni.AST.Scopes;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynValue.
	/// </summary>
	public sealed class DynValue:IComparable<DynValue>, IComparer<DynValue>, IEquatable<DynValue>
	{
		private readonly DataType _type;

		public DataType type { get { return _type; } }

		private readonly object value;

		public object Value{ get { return value; } }

		public DynValue (DataType _type, object value)
		{
			this._type = _type;
			this.value = value;
		}

		public static readonly DynValue True = new DynValue (DataType.Boolean, true);
		public static readonly DynValue False = new DynValue (DataType.Boolean, false);
		public static readonly DynValue Nil = new DynValue (DataType.Nil, null);
		public static readonly DynValue Void = new DynValue (DataType.Void, null);
		public static readonly DynValue[] Empty = new DynValue[0];

#region implicit conversion

		public static implicit operator DynValue (int value)
		{
			return new DynValue (DataType.Integer, (long)value);
		}

		public static implicit operator DynValue (long value)
		{
			return new DynValue (DataType.Integer, value);
		}

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

		public static implicit operator DynValue (DynObject value)
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

		public static implicit operator DynValue (DynTuple tuple)
		{
			return new DynValue (DataType.Tuple, tuple);
		}

		public static implicit operator DynValue (NativeFunction f)
		{
			return new DynValue (DataType.NativeFunction, f);
		}

		public static implicit operator DynValue (Range range)
		{
			return new DynValue (DataType.Range, range);
		}

#endregion

		public static DynValue FromInteger (long value)
		{
			return new DynValue (DataType.Integer, value);
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

		public static DynValue FromClosure (Closure value)
		{
			return new DynValue (DataType.Closure, value);
		}

		public static DynValue FromNativeFunction (NativeFunction value)
		{
			return new DynValue (DataType.NativeFunction, value);
		}

		public static DynValue FromDelegate (string name, Func<DynValue[],DynValue> f)
		{
			return new DynValue (DataType.NativeFunction, new NativeFunction (name, f));
		}

		public static DynValue FromDelegate (string name, Func<ASTNode[], IScope, DynValue> f)
		{
			return new DynValue (DataType.Command, new Command (name, f));
		}

		public static DynValue FromStructure (Structure value)
		{
			return new DynValue (DataType.Struct, value);
		}

		public static DynValue FromClass (DynObject value)
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

		public static DynValue FromTuple (DynTuple tuple)
		{
			return new DynValue (DataType.Tuple, tuple);
		}

		public  static DynValue FromRange (Range range)
		{
			return new DynValue (DataType.Range, range);
		}

		public static DynValue FromUserData (object value)
		{
			return new DynValue (DataType.UserData, value);
		}

		public static DynValue FromObject (object value)
		{
			if (value == null) {
				return DynValue.Nil;
			} else {
				var iconv = value as IConvertible;
				if (iconv != null)
					switch (iconv.GetTypeCode ()) {
						case TypeCode.Single:
							return FromNumber ((Single)value);
						case TypeCode.Double:
							return FromNumber ((double)value);
						case TypeCode.Boolean:
							return FromBoolean ((bool)value);
						case TypeCode.String:
							return FromString (value as string);
						case TypeCode.Int16:
							return FromInteger ((Int16)value);
						case TypeCode.Int32:
							return FromInteger ((Int32)value);
						case TypeCode.Int64:
							return FromInteger ((Int64)value);
						case TypeCode.UInt16:
							return FromInteger ((UInt16)value);
						case TypeCode.UInt32:
							return FromInteger ((UInt32)value);
						case TypeCode.UInt64:
							return FromInteger (checked((long)(UInt64)value));
					}
				return FromUserData (value);
			}
		}

		public bool IsInteger { get { return this.type == DataType.Integer; } }
		public bool IsNumber { get { return this.type == DataType.Number; } }
		public bool IsBoolean { get { return this.type == DataType.Boolean; } }
		public bool IsString { get { return this.type == DataType.String; } }
		public bool IsNil { get { return this.type == DataType.Nil; } }
		public bool IsVoid { get { return this.type == DataType.Void; } }

		public int AsInt32 ()
		{
			if (this.type == DataType.Integer) {
				return (int)(long)value;
			} else if (this.type == DataType.Number) {
				return (int)(double)value;
			} else {
				throw new RuntimeException ("Cast from '{0}' to Int32 is invalid.", value.GetType ().Name);
			}
		}

		public long AsInteger ()
		{
			if (this.type == DataType.Integer) {
				return (long)value;
			} else if (this.type == DataType.Number) {
				return (long)(double)value;
			} else {
				throw new RuntimeException ("Cast from '{0}' to integer is invalid.", value.GetType ().Name);
			}
		}

		public double AsNumber ()
		{
			if (this.type == DataType.Number) {
				return (double)value;
			} else if (this.type == DataType.Integer) {
				return (double)(long)value;
			} else {
				throw new RuntimeException ("Cast from '{0}' to number is invalid.", value.GetType ().Name);
			}
		}

		public bool AsBoolean ()
		{
			if (this.type == DataType.Boolean) {
				return (bool)value;
			} else {
				throw new RuntimeException ("Cast from '{0}' to boolean is invalid.", value.GetType ().Name);
			}
		}

		public string AsString ()
		{
			if (this.type == DataType.String) {
				return value as string;
			} else {
				throw new RuntimeException ("Cast from '{0}' to string is invalid.", value.GetType ().Name);
			}
		}

		public TValue As<TValue> () where TValue:class
		{
			TValue v = value as TValue;
			if (v == null) {
				throw new RuntimeException ("Cast from '{0}' to '{1}' is invalid.", 
						value.GetType ().Name, typeof(TValue).Name);
			} else {
				return v;
			}
		}

#region IComparable implementation

		public int CompareTo (DynValue other)
		{
			if (this._type == other._type) {
				switch (this._type) {
					case DataType.Integer:
						return ((long)value).CompareTo ((long)other.value);
					case DataType.Number:
						return ((double)value).CompareTo ((double)other.value);
					case DataType.Boolean:
						return ((bool)value).CompareTo ((bool)other.value);
					case DataType.String:
						return (value as string).CompareTo (other.value as string);
					default:
						return this.As<IComparable<DynValue>> ().CompareTo (other);
				}
			} else {
				if (this._type == DataType.Integer && other._type == DataType.Number) {
					return ((double)(long)this.value).CompareTo ( (double)other.value);
				} else if (this._type == DataType.Number && other._type == DataType.Integer) {
					return ((double)this.value).CompareTo((double)(long)other.value);
				} else {
					return this.As<IComparable<DynValue>> ().CompareTo (other);
				}
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
			if (this._type == other._type) {
				if (this.value == null) {
					return true;
				} else {
					return object.Equals (this.value, other.value);
				}
			} else {
				return false;
			}
		}

#endregion

		public override string ToString ()
		{
			if (value == null) {
				return string.Empty;
			}
			else if (type == DataType.String){
				return string.Concat ("\"", value as string, "\"");
			} else{
				return value.ToString ();
			}
		}

#region Equals and GetHashCode implementation

		public override bool Equals (object obj)
		{
			return (obj is DynValue) && Equals ((DynValue)obj);
		}

		public override int GetHashCode ()
		{
			return this.value.GetHashCode ();
		}

#endregion
	}
}
