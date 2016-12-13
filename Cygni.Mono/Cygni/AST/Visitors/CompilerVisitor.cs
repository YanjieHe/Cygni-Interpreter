using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST.Visitors
{
	internal class CompilerVisitor:ASTVisitor
	{
		Expression expression;
		Stack<Dictionary<string,ParameterExpression>> scopes;

		internal CompilerVisitor ()
		{
			this.expression = null;
			this.scopes = new Stack<Dictionary<string, ParameterExpression>> ();
			this.scopes.Push (new Dictionary<string, ParameterExpression> ()); // Global Scope
		}

		internal BlockExpression Load (BlockEx Program)
		{
			Program.Accept (this);
			return	Expression.Block (this.scopes.Peek ().Values, new []{ this.expression });
		}

		internal override void Visit (BinaryEx binaryEx)
		{
			switch (binaryEx.Op) {
			case BinaryOp.Add:
				{
					binaryEx.Left.Accept (this);
					ConstantExpression left = (ConstantExpression)this.expression;
					binaryEx.Right.Accept (this);
					ConstantExpression right = (ConstantExpression)this.expression;
					this.expression = 
						Expression.Convert (
						Expression.Add (
							Expression.Convert (left, typeof(double)),
							Expression.Convert (right, typeof(double))),
						typeof(DynValue));
					break;
				}
			}
		}

		internal override void Visit (AssignEx assignEx)
		{
			assignEx.Target.Accept (this);
			assignEx.Value.Accept (this);
		}

		internal override void Visit (BlockEx blockEx)
		{
			List<Expression> lines = new List<Expression> ();
			foreach (var item in blockEx.expressions) {
				item.Accept (this);
				lines.Add (this.expression);
			}
			this.expression = Expression.Block (lines);
		}

		internal override void Visit (Constant constant)
		{
			this.expression = Expression.Constant (constant.Value, typeof(DynValue));
		}

		internal override void Visit (DefClassEx defClassEx)
		{
			defClassEx.Body.Accept (this);
		}

		internal override void Visit (DefFuncEx defFuncEx)
		{
			defFuncEx.Body.Accept (this);
		}

		internal override void Visit (DotEx dotEx)
		{
			/*dotEx.Target.Accept (this);
			ConstantExpression target = this.expression;
			string field = dotEx.FieldName;
			this.expression = Expression.Call(
				Expression.Convert(typeof(IDot),
				Expression.Field(target,typeof(DynValue).GetField("Value"))*/
		}

		internal override void Visit (ForEx forEx)
		{
			forEx.Iterator.Accept (this);
			forEx.Start.Accept (this);
			forEx.End.Accept (this);
			if (forEx.Step != null)
				forEx.Step.Accept (this);
			forEx.Body.Accept (this);
		}

		internal override void Visit (ForEachEx forEachEx)
		{
			forEachEx.Iterator.Accept (this);
			forEachEx.Collection.Accept (this);
			forEachEx.Body.Accept (this);
		}

		internal override void Visit (IfEx ifEx)
		{
			ifEx.Condition.Accept (this);
			Expression Condition = this.expression;

			ifEx.IfTrue.Accept (this);
			Expression IfTrue = this.expression;

			if (ifEx.IfFalse != null) {
				ifEx.IfFalse.Accept (this);
				Expression IfFalse = this.expression;
				this.expression = Expression.IfThenElse (
					Expression.Convert (Condition, typeof(bool))
					, IfTrue, IfFalse);
			} else {
				this.expression = Expression.IfThenElse (
					Expression.Convert (Condition, typeof(bool)),
					IfTrue,
					Expression.Constant (DynValue.Nil, typeof(DynValue)));
			}
		}

		internal override void Visit (IndexEx indexEx)
		{
			indexEx.Collection.Accept (this);
			foreach (var item in indexEx.Indexes)
				item.Accept (this);
		}

		internal override void Visit (SingleIndexEx indexEx)
		{
			indexEx.Collection.Accept (this);
			indexEx.Index.Accept (this);
		}

		internal override void Visit (InvokeEx invokeEx)
		{
			invokeEx.Func.Accept (this);
			foreach (var item in invokeEx.Arguments)
				item.Accept (this);
		}

		internal override void Visit (ListInitEx listInitEx)
		{
			foreach (var item in listInitEx._List)
				item.Accept (this);
		}

		internal override void Visit (DictionaryInitEx dictionaryInitEx)
		{
			foreach (var item in dictionaryInitEx._List)
				item.Accept (this);
		}

		internal override void Visit (NameEx nameEx)
		{
			if (scopes.Peek ().ContainsKey (nameEx.Name)) {
				this.expression = scopes.Peek () [nameEx.Name];
			} else {	
				this.expression = Expression.Variable (typeof(DynValue), nameEx.Name);
				scopes.Peek ().Add (nameEx.Name, (ParameterExpression)this.expression);
			}
		}

		internal override void Visit (ReturnEx returnEx)
		{
			returnEx.Value.Accept (this);
		}

		internal override void Visit (UnaryEx unaryEx)
		{
			unaryEx.Operand.Accept (this);
		}

		internal override void Visit (WhileEx whileEx)
		{
			whileEx.Condition.Accept (this);
			whileEx.Body.Accept (this);
		}

	}
}

