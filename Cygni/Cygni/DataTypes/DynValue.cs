using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynValue.
	/// </summary>
	public struct DynValue: IComputable,IComparable<DynValue>, IEquatable<DynValue>
	{
		readonly DataType _type;
		public DataType type { get { return _type; } }
		readonly object value;
		public object Value{ get { return value; }  }
		
		public DynValue(DataType _type, object value)
		{
			this._type = _type;
			this.value = value;
		}
		
		public static readonly DynValue True = new DynValue(DataType.Boolean, true);
		public static readonly DynValue False = new DynValue(DataType.Boolean, false);
		public static readonly DynValue Null = new DynValue(DataType.Null, null);
		#region implicit conversion
		public static implicit operator DynValue(double value)
		{
			return new DynValue(DataType.Number, value);
		}
		public static implicit operator DynValue(bool value)
		{
			return value ? True : False;
		}
		public static implicit operator DynValue(string value)
		{
			return new DynValue(DataType.String, value);
		}
		public static implicit operator DynValue(Func<DynValue[],DynValue> f)
		{
			return new DynValue(DataType.NativeFunction,new NativeFunction(f));
		}
		#endregion
		
		public static DynValue FromNumber(double value)
		{
			return new DynValue(DataType.Number, value);
		}
		
		public static DynValue FromBoolean(bool value)
		{
			return value ? True : False;
		}
		
		public static DynValue FromString(string value)
		{
			return new DynValue(DataType.String, value);
		}
		
		public static DynValue FromFunction(Function value)
		{
			return new DynValue(DataType.Function, value);
		}
		
		public static DynValue FromNativeFunction(NativeFunction value)
		{
			return new DynValue(DataType.NativeFunction, value);
		}
		
		public static DynValue FromDelegate(Func<DynValue[],DynValue> f)
		{
			return new DynValue(DataType.NativeFunction, new NativeFunction(f));
		}
		
		public static DynValue FromStructure(Structure value)
		{
			return new DynValue(DataType.Struct, value);
		}
		
		public static DynValue FromClass(ClassInfo value)
		{
			return new DynValue(DataType.Class, value);
		}
		
		public static DynValue FromList(DynList list)
		{
			return new DynValue(DataType.List, list);
		}
		
		public static DynValue FromHashTable(DynHashTable hashTable)
		{
			return new DynValue(DataType.HashTable, hashTable);
		}

		public static DynValue FromUserData(object value)
		{
			return new DynValue(DataType.UserData, value);
		}
		
		public static DynValue FromObject(object value)
		{
			if (value == null)
				return DynValue.Null;
			var iconv = value as IConvertible;
			if (iconv != null)
				switch (iconv.GetTypeCode()) {
					case TypeCode.Double:
						return FromNumber((double)value);
					case TypeCode.Boolean:
						return FromBoolean((bool)value);
					case TypeCode.String:
						return FromString(value as string);
					case TypeCode.Int16:
						return FromNumber((Int16)value);
					case TypeCode.Int32:
						return FromNumber((Int32)value);
					case TypeCode.Int64:
						return FromNumber((Int64)value);
					case TypeCode.UInt16:
						return FromNumber((UInt16)value);
					case TypeCode.UInt32:
						return FromNumber((UInt32)value);
					case TypeCode.UInt64:
						return FromNumber((UInt64)value);
					case TypeCode.Single:
						return FromNumber((Single)value);
				}
			return FromUserData(value);
		}
		
		public double AsNumber()
		{
			return (double)value;
		}
		
		public bool AsBoolean()
		{
			return (bool)value;
		}
		
		public string AsString()
		{
			return (string)value;
		}

		public TValue As<TValue>()
		{
			return (TValue)value;
		}

		#region IComputable implementation
		public DynValue Add(DynValue other)
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, (double)value + (double)other.value);
			return (value as IComputable).Add(other);
		}
		public DynValue Subtract(DynValue other)
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, (double)value - (double)other.value);
			return (value as IComputable).Add(other);
		}
		public DynValue Multiply(DynValue other)
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, (double)value * (double)other.value);
			return (value as IComputable).Add(other);
		}
		public DynValue Divide(DynValue other)
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, (double)value / (double)other.value);
			return (value as IComputable).Add(other);
		}
		public DynValue Modulo(DynValue other)
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, (double)value % (double)other.value);
			return (value as IComputable).Add(other);
		}
		public DynValue Power(DynValue other)
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, Math.Pow((double)value, (double)other.value));
			return (value as IComputable).Add(other);
		}

		public DynValue UnaryPlus()
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, +(double)value);
			return (value as IComputable).UnaryPlus();
		}

		public DynValue UnaryMinus()
		{
			if (type == DataType.Number)
				return new DynValue(DataType.Number, -(double)value);
			return (value as IComputable).UnaryMinus();
		}

		#endregion

		#region IComparable implementation


		public int CompareTo(DynValue other)
		{
			if (type == DataType.Number)
				return ((double)value).CompareTo((double)other.value);
			
			return (value as IComparable<DynValue>).CompareTo(other);
		}

		#endregion
		
		internal static readonly DynValue Break = new DynValue(DataType.Break, null);

		internal static readonly DynValue Continue = new DynValue(DataType.Continue, null);
		
		internal static DynValue Return(DynValue value)
		{
			return new DynValue(DataType.Return, value);
		}
		
		#region IEquatable implementation
		public bool Equals(DynValue other)
		{
			return _type == other._type && (value == null || object.Equals(value, other.value));
		}
		#endregion
		public override string ToString()
		{
			if (value == null)
				return string.Empty;
			if (type == DataType.String)
				return string.Concat("\"", value as string, "\"");
			return value.ToString();
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is DynValue) && Equals((DynValue)obj);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public static bool operator ==(DynValue lhs, DynValue rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(DynValue lhs, DynValue rhs)
		{
			return !(lhs == rhs);
		}

		#endregion
		
		public Type GetDynType()
		{
			switch (_type) {
				case DataType.Number:
					return typeof(double);
				case DataType.Boolean:
					return typeof(bool);	
				case DataType.String:
					return typeof(string);
				default:
					return value.GetType();
			}
		}
	}
}
