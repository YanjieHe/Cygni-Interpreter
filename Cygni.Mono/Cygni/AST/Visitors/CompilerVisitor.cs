using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using System.Reflection;
using Cygni.Errors;

namespace Cygni.AST.Visitors
{
	internal class CompilerVisitor:ASTVisitor
	{
		Expression expression;
		Stack<Dictionary<string, ParameterExpression>> scopes;
		LabelTarget ReturnTarget;
		// LabelTarget BreakTarget;
		// LabelTarget ContinueTarget;

		private static readonly MethodInfo FromNumber = typeof(DynValue).GetMethod ("FromNumber");
		private static readonly MethodInfo FromBoolean = typeof(DynValue).GetMethod ("FromBoolean");
		// private static readonly MethodInfo FromString = typeof(DynValue).GetMethod ("FromString");
		private static readonly MethodInfo FromList = typeof(DynValue).GetMethod ("FromList");
		private static readonly MethodInfo FromDelegate = typeof(DynValue).GetMethod ("FromDelegate",
			                                                  new []{ typeof(Func<DynValue[],DynValue>), typeof(string) });

		// private static readonly MethodInfo Return = typeof(DynValue).GetMethod ("Return");
		private static readonly MethodInfo GetByDot = typeof(IDot).GetMethod ("GetByDot");

		private static readonly ConstantExpression Nil = Expression.Constant (DynValue.Nil, typeof(DynValue));

		internal CompilerVisitor (BasicScope scope)
		{
			this.expression = null;
			this.scopes = new Stack<Dictionary<string, ParameterExpression>> ();
			var globalScope = new Dictionary<string, ParameterExpression> ();
			this.scopes.Push (globalScope); // Global Scope
			foreach (var item in scope.GetTable()) {
				globalScope.Add (item.Key, Expression.Variable (typeof(DynValue), item.Key));
			}
			BuiltInScope builtInScope =(BuiltInScope) scope.Parent;
			foreach (var item in builtInScope.GetTable()) {
				globalScope.Add (item.Key, Expression.Variable (typeof(DynValue), item.Key));
			}
		}

		internal BlockExpression Load (BlockEx Program)
		{
			Program.Accept (this);
			return Expression.Block (scopes.Peek ().Values, new Expression[]{ this.expression });
		}

		internal override void Visit (BinaryEx binaryEx)
		{
			switch (binaryEx.Op) {
			case BinaryOp.Add:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromNumber,
						Expression.Add (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Sub:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromNumber,
						Expression.Add (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Mul:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromNumber,
						Expression.Multiply (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Div:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromNumber,
						Expression.Divide (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Mod:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromNumber,
						Expression.Modulo (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Pow:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromNumber,
						Expression.Power (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.And:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.And (
							Expression.Convert (Expression.Property (left, "Value"), typeof(bool)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(bool))));
					break;
				}
			case BinaryOp.Or:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.Or (
							Expression.Convert (Expression.Property (left, "Value"), typeof(bool)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(bool))));
					break;
				}
			case BinaryOp.Less:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.LessThan (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Greater:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.GreaterThan (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.LessOrEqual:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.LessThanOrEqual (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.GreaterOrEqual:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.GreaterThanOrEqual (
							Expression.Convert (Expression.Property (left, "Value"), typeof(double)),
							Expression.Convert (Expression.Property (right, "Value"), typeof(double))));
					break;
				}
			case BinaryOp.Equal:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.Equal (left, right));
					break;
				}
			case BinaryOp.NotEqual:
				{
					binaryEx.Left.Accept (this);
					Expression left = this.expression;
					binaryEx.Right.Accept (this);
					Expression right = this.expression;
					this.expression = 
						Expression.Call (FromBoolean,
						Expression.NotEqual (left, right));
					break;
				}
			}
		}

		internal override void Visit (AssignEx assignEx)
		{
			assignEx.Target.Accept (this);
			Expression target = this.expression;
			assignEx.Value.Accept (this);
			Expression value = this.expression;

			if (target.NodeType == ExpressionType.Parameter) {
				var variable = (ParameterExpression)target;
				if (!scopes.Peek ().ContainsKey (variable.Name)) {
					scopes.Peek ().Add (variable.Name, variable);
				}
				this.expression = Expression.Assign (target, value);
			} else {
				throw new NotSupportedException ();
			}
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
			ParameterExpression function;
			if (!scopes.Peek ().ContainsKey (defFuncEx.Name)) {
				function = Expression.Variable (typeof(DynValue), defFuncEx.Name);
				scopes.Peek ().Add (function.Name, function);
			} else {
				function = scopes.Peek () [defFuncEx.Name];
			}


			scopes.Push (new Dictionary<string, ParameterExpression> ());
			var args = new List<ParameterExpression> ();
			for (int i = 0; i < defFuncEx.Parameters.Length; i++) {
				args.Add (Expression.Variable (typeof(DynValue), defFuncEx.Parameters [i]));
				scopes.Peek ().Add (args [i].Name, args [i]);
			}
			this.ReturnTarget = Expression.Label (typeof(DynValue));


			defFuncEx.Body.Accept (this);

			var body = this.expression;
			scopes.Pop ();
			var parameters = Expression.Parameter (typeof(DynValue[]));
			var lines = new List<Expression> ();
			for (int i = 0; i < args.Count; i++) {
				var line = Expression.Assign (args [i], Expression.ArrayIndex (parameters, Expression.Constant (i, typeof(int))));
				lines.Add (line);
			}
			lines.Add (body);
			lines.Add (Expression.Label (ReturnTarget, Nil));
			var func_body = Expression.Block (args, lines);
		
			this.expression = Expression.Assign (function,
				Expression.Call (FromDelegate,
					Expression.Lambda (func_body, parameters), Expression.Constant (defFuncEx.Name, typeof(string))));
			this.ReturnTarget = null;
		}

		internal override void Visit (DotEx dotEx)
		{
			dotEx.Target.Accept (this);
			Expression target = this.expression;
			string field = dotEx.FieldName;
			this.expression = Expression.Call (
				Expression.Convert (
					Expression.Property (target, "Value"), typeof(IDot)), 
				GetByDot, Expression.Constant (field));
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
				this.expression = Expression.Condition (
					Expression.Convert (Expression.Property (Condition, "Value"), typeof(bool))
					, IfTrue, IfFalse);
			} else {
				this.expression = Expression.Condition (
					Expression.Convert (Expression.Property (Condition, "Value"), typeof(bool)),
					IfTrue, Nil);
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
			var function = this.expression;
			var args = new List<Expression> ();
			foreach (var item in invokeEx.Arguments) {
				item.Accept (this);
				args.Add (this.expression);
			}
			this.expression = Expression.Call (
				Expression.Convert (
					Expression.Property (
						function, "Value"), typeof(IFunction)),
				typeof(IFunction).GetMethod ("DynInvoke"), Expression.NewArrayInit (typeof(DynValue), args));
		}

		internal override void Visit (ListInitEx listInitEx)
		{
			var elements = new List<Expression> ();
			foreach (var item in listInitEx._List) {
				item.Accept (this);
				elements.Add (this.expression);
			}
			var types = new Type[] {
				typeof(IEnumerable<DynValue>), typeof(int)
			};
			ConstructorInfo constructor = typeof(DynList).GetConstructor (
				                              types);
			Expression args = Expression.NewArrayInit (typeof(DynValue), elements);
			this.expression = 
				Expression.Call (FromList,
				Expression.New (constructor,
					new Expression[] {
						args, Expression.Constant (elements.Count, typeof(int))
					}));
		}

		internal override void Visit (DictionaryInitEx dictionaryInitEx)
		{
			foreach (var item in dictionaryInitEx._List)
				item.Accept (this);
		}

		internal override void Visit (NameEx nameEx)
		{
			foreach (var scope in scopes) {
				if (scope.ContainsKey (nameEx.Name)) {
					this.expression = scope [nameEx.Name];
					return;
				}
			}
			ParameterExpression variable = Expression.Variable (typeof(DynValue), nameEx.Name);
			this.expression = variable;
		}

		internal override void Visit (ReturnEx returnEx)
		{
			returnEx.Value.Accept (this);
			var value = this.expression;
			this.expression = Expression.Return (this.ReturnTarget, value, typeof(DynValue));
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

