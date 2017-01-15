using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
    public sealed class DynObject:IDot,IFunction,IComputable, IComparable<DynValue>
    {
        readonly string name;
        readonly ClassScope classScope;
        readonly DynObject parent;
        readonly bool isInstance;

        public DynObject Parent{ get { return this.parent; } }


        public DynObject(string name, ClassScope classScope, bool isInstance, DynObject parent = null)
        {
            this.name = name;
            this.classScope = classScope;
            this.parent = parent;
            this.isInstance = isInstance;
        }

        public DynValue GetByDot(string fieldName)
        {
            return classScope.Get(fieldName);
        }

        public DynValue SetByDot(string fieldName, DynValue value)
        {
            return classScope.Put(fieldName, value);
        }

        public string[] FieldNames{ get { return this.classScope.FieldNames(); } }

        void SetThis(DynObject obj)
        {
            this.classScope.Put(0, 0, obj);
        }

        public DynValue DynInvoke(DynValue[] args)
        {
            if (!isInstance)
            {
                DynValue constructor;
                var newClass = new DynObject(name, this.classScope.Clone(), true, this.parent);
                newClass.SetThis(newClass);
                if (classScope.TryGetValue("__init__", out constructor))
                {
                    newClass.GetByDot("__init__").As<IFunction>().DynInvoke(args);
                }
                return newClass;
            }
            else
            {
                throw new RuntimeException("class '{0}' is an instance, which cannot be used as a constructor.", this.classScope.Name);
            }
        }

        public DynValue DynEval(ASTNode[] args, IScope scope)
        {
            if (!isInstance)
            {
                DynValue constructor;
                var newClass = new DynObject(name, this.classScope.Clone(), true, this.parent);
                newClass.SetThis(newClass);
                if (classScope.TryGetValue("__init__", out constructor))
                {
                    newClass.GetByDot("__init__").As<IFunction>().DynEval(args, scope);
                }
                return newClass;
            }
            else
            {
                throw new RuntimeException("class '{0}' is an instance, which cannot be used as a constructor.", this.classScope.Name);
            }
        }

        internal Dictionary<string, DynValue> GetFields()
        {
            Dictionary<string, DynValue> fields;
            if (this.parent == null)
            {
                fields = new Dictionary<string, DynValue>();
            }
            else
            {
                fields = this.parent.GetFields();
            }
            foreach (string field in this.FieldNames)
            {
                if (!string.Equals(field, "this"))
                {
                    fields.Add(field, this.GetByDot(field));
                }
            }
            return fields;
        }

        #region IComputable

        public DynValue Add(DynValue other)
        {
            return this.GetByDot("__add__").As<IFunction>().DynInvoke(new DynValue[]{ other });
        }

        public DynValue Subtract(DynValue other)
        {
            return this.GetByDot("__sub__").As<IFunction>().DynInvoke(new DynValue[]{ other });
        }

        public DynValue Multiply(DynValue other)
        {
            return this.GetByDot("__mul__").As<IFunction>().DynInvoke(new DynValue[]{ other });
        }

        public DynValue Divide(DynValue other)
        {
            return this.GetByDot("__div__").As<IFunction>().DynInvoke(new DynValue[]{ other });
        }

        public DynValue Modulo(DynValue other)
        {
            return this.GetByDot("__mod__").As<IFunction>().DynInvoke(new DynValue[]{ other });
        }

        public DynValue Power(DynValue other)
        {
            return this.GetByDot("__pow__").As<IFunction>().DynInvoke(new DynValue[]{ other });
        }

        public DynValue UnaryPlus()
        {
            return this.GetByDot("__unp__").As<IFunction>().DynInvoke(DynValue.Empty);
        }

        public DynValue UnaryMinus()
        {
            return this.GetByDot("__unm__").As<IFunction>().DynInvoke(DynValue.Empty);
        }

        #endregion

        public DynValue GetByIndex(DynValue index)
        {
            return this.GetByDot("__getItem__").As<IFunction>().DynInvoke(new DynValue[] { index });
        }

        public DynValue SetByIndex(DynValue index, DynValue value)
        {
            return this.GetByDot("__setItem__").As<IFunction>().DynInvoke(new DynValue[] { index, value });
        }

        public DynValue GetByIndexes(DynValue[] indexes)
        {
            return this.GetByDot("__getItem").As<IFunction>().DynInvoke(indexes);
        }

        public DynValue SetByIndexes(DynValue[] indexes, DynValue value)
        {
            DynValue[] args = new DynValue [indexes.Length + 1];
            for (int i = 0; i < indexes.Length; i++)
            {
                args[i] = indexes[i];
            }
            args[args.Length - 1] = value;
            return this.GetByDot("__setItem__").As<IFunction>().DynInvoke(args);
        }

        public int CompareTo(DynValue other)
        {
            return this.GetByDot("__cmp__").As<IFunction>().DynInvoke(new DynValue[]{ other }).AsInt32();
        }

        public override string ToString()
        {
            DynValue value;
            if (isInstance && classScope.TryGetValue("__toStr__", out value))
            {
                IFunction f = value.As<IFunction>();
                return f.DynInvoke(DynValue.Empty).AsString();
            }
            else
            {
                return "(Class: " + this.name + ")";
            }
        }
    }
}

