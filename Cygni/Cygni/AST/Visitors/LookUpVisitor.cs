using System;
using System.Collections.Generic;
namespace Cygni.AST.Visitors
{
	internal class LookUpVisitor:ASTVisitor
	{
		List<NameEx> names;
		internal LookUpVisitor (List<NameEx> names)
		{
			this.names = names;	
		}
		internal override void Visit (BinaryEx binaryEx)
		{
			if (binaryEx.Op == BinaryOp.Assign && binaryEx.Left.type == NodeType.Name) {
				var nameEx = binaryEx.Left as NameEx;
				if (names.Contains (nameEx)) {
					nameEx.Accept(this);
				} else {
					var newName = new NameEx (nameEx.Name, names.Count);
					binaryEx.SetLeft (newName);
					names.Add (newName);
				}
				binaryEx.Right.Accept (this);
				return;
			}
			base.Visit (binaryEx);
		}
		internal override void Visit (NameEx nameEx)
		{
			int i =	names.IndexOf (nameEx);
			if (i >= 0)
				nameEx.SetIndex (i);
		}
		internal override void Visit(ForEx forEx){
			if (names.Contains (forEx.Iterator))
				forEx.Iterator.Accept (this);
			else {
				forEx.SetIterator (new NameEx (forEx.Iterator.Name, names.Count));
				names.Add (forEx.Iterator);
			}
			forEx.Start.Accept (this);
			forEx.End.Accept (this);
			if(forEx.Step!=null)
				forEx.Step.Accept (this);
			forEx.Body.Accept (this);
		}
		internal override void Visit(ForEachEx forEachEx){
			if (names.Contains (forEachEx.Iterator))
				forEachEx.Iterator.Accept (this);
			else {
				forEachEx.SetIterator (new NameEx (forEachEx.Iterator.Name, names.Count));
				names.Add (forEachEx.Iterator);
			}
			forEachEx.Collection.Accept (this);
			forEachEx.Body.Accept (this);
		}

	}
}

