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
    public sealed class Closure:IFunction,IEnumerable<DynValue>
    {
        readonly int nArgs;
        readonly ArrayScope funcScope;
        readonly ASTNode body;

        public bool IsSingleLine { get { return this.body.type != NodeType.Block; } }

        public Closure(int nArgs, ASTNode body, ArrayScope funcScope)
        {
            this.nArgs = nArgs;
            this.body = body;
            this.funcScope = funcScope;
        }

        public DynValue Invoke()
        {
            DynValue result = body.Eval(funcScope);
            if (IsSingleLine)
            {
                return result;
            }
            else
            {
                return 
					result.type == DataType.Return ? 
					result.Value as DynValue : 
					DynValue.Void;
            }
        }

        public DynValue DynInvoke(DynValue[] args)
        {
            if (args.Length > nArgs)
            {
                throw RuntimeException.BadArgsNum("Anonymous Function", nArgs);
            }
            DynValue[] values = new DynValue[funcScope.Count];
            int i = 0;
            while (i < args.Length)
            {
                values[i] = args[i];
                i++;
            }
            while (i < nArgs)
            {
                values[i] = DynValue.Nil;
                i++;
            }
            var newScope = new ArrayScope("Anonymous Function", values, funcScope.Parent);
            return new Closure(nArgs, body, newScope).Invoke();
        }

        public DynValue DynEval(ASTNode[] args, IScope scope)
        {
            if (args.Length > nArgs)
                throw RuntimeException.BadArgsNum("Anonymous Function", nArgs);
            DynValue[] values = new DynValue[funcScope.Count];
            int i = 0;
            while (i < args.Length)
            {
                values[i] = args[i].Eval(scope);
                i++;
            }
            while (i < nArgs)
            {
                values[i] = DynValue.Nil;
                i++;
            }
            var newScope = new ArrayScope("Anonymous Function", values, funcScope.Parent);
            return new Closure(nArgs, body, newScope).Invoke();
        }

        public IEnumerator<DynValue> GetEnumerator()
        {
            do
            {
                DynValue value = this.DynInvoke(DynValue.Empty);
                if (value.IsVoid)
                {
                    break;
                }
                else
                {
                    yield return value;
                }
            } while (true);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return "(Anonymous Function)";
        }
    }
}

