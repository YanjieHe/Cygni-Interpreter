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
    /// <summary>
    /// Description of DynDictionary.
    /// </summary>
    public sealed class DynDictionary: Dictionary<object,DynValue> ,IEnumerable<DynValue>, IIndexable,IDot,ICountable
    {
        public DynDictionary(int capacity)
            : base(capacity)
        {
            
        }

        public DynValue GetByIndex(DynValue index)
        {
            switch (index.type)
            {
                case DataType.Integer:
                    return base[(long)index.Value];
                case DataType.Boolean:
                case DataType.String:
                    return base[index.Value];
                default:
                    throw new RuntimeException("Dictionary only takes number, boolean and string as keys.");
            }
        }

        public DynValue SetByIndex(DynValue index, DynValue value)
        {
            switch (index.type)
            {
                case DataType.Integer:
                    return base[(long)index.Value] = value;
                case DataType.Boolean:
                case DataType.String:
                    return base[index.Value] = value;
                default:
                    throw new RuntimeException("Dictionary only takes number, boolean and string as keys.");
            }

        }

        public DynValue GetByIndexes(DynValue[] indexes)
        {
            RuntimeException.IndexerArgsCheck(indexes.Length == 1, "Dictionary");
            return GetByIndex(indexes[0]);
        }

        public DynValue SetByIndexes(DynValue[] indexes, DynValue value)
        {
            RuntimeException.IndexerArgsCheck(indexes.Length == 1, "Dictionary");
            return SetByIndex(indexes[0], value);
        }

        public void Add(DynValue key, DynValue value)
        {
            switch (key.type)
            {
                case DataType.Integer:
                    base.Add((long)key.Value, value);
                    return;
                case DataType.Boolean:
                case DataType.String:
                    base.Add(key.Value, value);
                    return;
                default :
                    throw new RuntimeException("Dictionary only takes number, boolean and string as keys.");
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            var iterator = base.GetEnumerator();
            s.Append("{ ");
            if (iterator.MoveNext())
            {
                s.Append(DynValue.FromObject(iterator.Current.Key))
					.Append(" -> ")
					.Append(iterator.Current.Value);
            }
            while (iterator.MoveNext())
            {
                s.Append(", ")
					.Append(DynValue.FromObject(iterator.Current.Key))
					.Append(" -> ")
					.Append(iterator.Current.Value);
            }
            s.Append(" }");
            return s.ToString();
        }

        public new IEnumerator<DynValue> GetEnumerator()
        {
            var iterator = base.GetEnumerator();
            while (iterator.MoveNext())
            {
                IConvertible iconv = iterator.Current.Key as IConvertible;
                if (iconv == null)
                {
                    throw new RuntimeException("Dictionary only takes number, boolean and string as keys.");
                }
                else
                {
                    DynValue key;
                    switch (iconv.GetTypeCode())
                    {
                        case TypeCode.Int64:
                            key = DynValue.FromInteger((long)iterator.Current.Key);
                            break;
                        case TypeCode.Boolean:
                            key = DynValue.FromBoolean((bool)iterator.Current.Key);
                            break;
                        case TypeCode.String:
                            key = DynValue.FromString(iterator.Current.Key as string);
                            break;
                        default:
                            throw new RuntimeException("Dictionary only takes number, boolean and string as keys.");
                    }
                    KeyValuePair pair = new KeyValuePair(key, iterator.Current.Value);
                    yield return pair;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            var iterator = this.GetEnumerator();
            while (iterator.MoveNext())
            {
                yield return iterator.Current;
            }
        }

        public DynValue GetByDot(string fieldName)
        {
            switch (fieldName)
            {
                case "hasKey":
                    return DynValue.FromDelegate("hasKey", (args) => DictionaryLibrary.hasKey(this, args));
                case "hasValue":
                    return DynValue.FromDelegate("hasValue", (args) => DictionaryLibrary.hasValue(this, args));
                case "remove":
                    return DynValue.FromDelegate("remove", (args) => DictionaryLibrary.remove(this, args));
                case "count":
                    return (long)this.Count;
                case "keys":
                    return DynValue.FromDelegate("keys", (args) => DictionaryLibrary.keys(this, args));
                case "values":
                    return DynValue.FromDelegate("values", (args) => DictionaryLibrary.values(this, args));
                case "add":
                    return DynValue.FromDelegate("add", (args) => DictionaryLibrary.add(this, args));
                case "clear":
                    return DynValue.FromDelegate("clear", (args) => DictionaryLibrary.clear(this, args));
                default:
                    throw RuntimeException.FieldNotExist("Dictionary", fieldName);
            }
        }

        public string[] FieldNames
        {
            get
            {
                return new []
                {
                    "hasKey", "hasValue", 
                    "remove", "count", 
                    "keys", "values", "add", "clear"
                };
            }
        }

        public DynValue SetByDot(string fieldName, DynValue value)
        {
            throw RuntimeException.FieldNotExist("Dictionary", fieldName);
        }

    }
}
