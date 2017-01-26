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
    public sealed class KeyValuePair:IDot
    {
        readonly DynValue key;
        readonly DynValue value;

        public DynValue Key { get { return this.key; } }

        public DynValue Value { get { return this.value; } }

        public KeyValuePair(DynValue key, DynValue value)
        {
            this.key = key;
            this.value = value;
        }

        public override string ToString()
        {
            return string.Concat("[ ", key, " -> ", value, " ]");
        }

        public DynValue GetByDot(string fieldName)
        {
            if (string.Equals(fieldName, "key"))
            {
                return key;
            }
            else if (string.Equals(fieldName, "value"))
            {
                return value;
            }
            else
            {
                throw RuntimeException.FieldNotExist("KeyValuePair",fieldName);
            }
        }

        public DynValue SetByDot(string fieldName, DynValue value)
        {
            throw RuntimeException.FieldNotExist("KeyValuePair",fieldName);
        }

        public string[] FieldNames{ get { return new [] { "key", "value" }; } }
    }
}