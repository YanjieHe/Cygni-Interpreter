using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;

namespace Cygni.Libraries
{
    /// <summary>
    /// Description of StringLibrary.
    /// </summary>
    public static class StringLibrary
    {
        internal static DynValue StrGetByDot(string str, string fieldName)
        {
            switch (fieldName)
            {
                case "length":
                    return (long)str.Length;
                case "replace":
                    return DynValue.FromDelegate("replace", (args) => StringLibrary.replace(str, args));
                case "format":
                    return DynValue.FromDelegate("format", (args) => StringLibrary.format(str, args));
                case "join":
                    return DynValue.FromDelegate("join", (args) => StringLibrary.join(str, args));
                case "split":
                    return DynValue.FromDelegate("split", (args) => StringLibrary.split(str, args));
                case "find":
                    return DynValue.FromDelegate("find", (args) => StringLibrary.find(str, args));
                case "lower":
                    return DynValue.FromDelegate("lower", (args) => str.ToLower());
                case "upper":
                    return DynValue.FromDelegate("upper", (args) => str.ToUpper());
                case "trim":
                    return DynValue.FromDelegate("trim", (args) => StringLibrary.trim(str, args));
                case "trimStart":
                    return DynValue.FromDelegate("trimStart", (args) => StringLibrary.trimStart(str, args));
                case "trimEnd":
                    return DynValue.FromDelegate("trimEnd", (args) => StringLibrary.trimEnd(str, args));
                default:
                    throw RuntimeException.FieldNotExist("string", fieldName);
            }
        }

        public static DynValue concat(DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length == 1, "concat");
            DynList list = args[0].As<DynList>();
            string[] values = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                values[i] = list[i].Value.ToString();
            }
            return string.Concat(values);
        }

        public static DynValue join(string str, DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length == 1, "join");
            return string.Join(str, args[0].As<DynList>().Select(i => i.Value));
        }

        public static DynValue format(string str, DynValue[] args)
        {
            object[] values = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                values[i] = args[i].Value;
            }
            return string.Format(str, values);
        }

        public static DynValue split(string str, DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length >= 1, "strsplit");

            char[] arr = new char[args.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = char.Parse(args[i].AsString());
            string[] result = str.Split(arr);

            DynList list = new DynList(result.Length);
            foreach (string item in result)
            {
                list.Add(item);
            }

            return DynValue.FromList(list);
        }

        public static DynValue replace(string str, DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length == 2, "replace");
            string oldValue = args[0].AsString();
            string newValue = args[1].AsString();
            return str.Replace(oldValue, newValue);
        }

        public static DynValue compare(DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length == 2, "compare");
            return string.Compare(
                strA: args[0].AsString(),
                strB: args[1].AsString());
        }

        public static DynValue compareOrdinal(DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length == 2, "compare");
            return string.CompareOrdinal(
                strA: args[0].AsString(),
                strB: args[1].AsString());
        }

        public static DynValue find(string str, DynValue[] args)
        {
            RuntimeException.FuncArgsCheck(args.Length == 1 || args.Length == 2 || args.Length == 3, "find");
            if (args.Length == 1)
            {
                return str.IndexOf(value: args[0].AsString());
            }
            else if (args.Length == 2)
            {
                return str.IndexOf(value: args[0].AsString(), startIndex: args[1].AsInt32());
            }
            else
            {
                return str.IndexOf(
                    value: args[0].AsString(), 
                    startIndex: args[1].AsInt32(), 
                    count: args[2].AsInt32());
            }
        }

        public static DynValue trim(string str, DynValue[] args)
        {
            if (args.Length == 0)
            {
                return str.Trim();
            }
            else
            {
                char[] arr = new char[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    arr[i] = char.Parse(args[i].AsString());
                }
                return str.Trim(arr);
            }
        }

        public static DynValue trimStart(string str, DynValue[] args)
        {
            if (args.Length == 0)
            {
                return str.TrimStart();
            }
            else
            {
                char[] arr = new char[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    arr[i] = char.Parse(args[i].AsString());
                }
                return str.TrimStart(arr);
            }
        }

        public static DynValue trimEnd(string str, DynValue[] args)
        {
            if (args.Length == 0)
            {
                return str.TrimEnd();
            }
            else
            {
                char[] arr = new char[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    arr[i] = char.Parse(args[i].AsString());
                }
                return str.TrimEnd(arr);
            }
        }

        public static DynValue Slice(string str, Range range)
        {
            int start = range.Start;
            int end = range.End;
            int step = range.Step;
            RuntimeException.SliceCheck(str.Length, range);
            char[] chArr = new char [(end - start + 1) / step];
            int j = 0;
            if (range.IsForward)
            {
                for (int i = start; i < end; i += step)
                {
                    chArr[j] = (str[i]);
                    j++;
                }
            }
            else
            {
                for (int i = start; i > end; i += step)
                {
                    chArr[j] = (str[i]);
                    j++;
                }
            }
            return new string(chArr);
        }

    }
}
