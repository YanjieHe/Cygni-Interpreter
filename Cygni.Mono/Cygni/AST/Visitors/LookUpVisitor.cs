﻿using System;
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

		internal override void Visit (AssignEx assignEx)
		{
			if (assignEx.Target.type == NodeType.Name) {
				var nameEx = assignEx.Target as NameEx;
				if (names.Contains (nameEx)) {
					nameEx.Accept (this);
				} else {
					var newName = new NameEx (nameEx.Name, names.Count);
					assignEx.SetTarget (newName);
					names.Add (newName);
				}
				assignEx.Value.Accept (this);
			} else {
				base.Visit (assignEx);
			}
		}

		internal override  void Visit (SetEx setEx)
		{
			for (int i = 0; i < setEx.Targets.Length; i++) {
				var item = setEx.Targets [i];
				if (item.type == NodeType.Name) {
					var nameEx = item as NameEx;
					if (names.Contains (nameEx)) {
						nameEx.Accept (this);
					} else {
						var newName = new NameEx (nameEx.Name, names.Count);
						setEx.Targets [i] = newName;
						names.Add (newName);
					}
					setEx.Values [i].Accept (this);
				} else {
					base.Visit (setEx);
				}
			}
		}

		internal override  void Visit (UnpackEx unpackEx)
		{
			for (int i = 0; i < unpackEx.Items.Length; i++) {
				var item = unpackEx.Items [i];
				if (item.type == NodeType.Name) {
					var nameEx = item as NameEx;
					if (names.Contains (nameEx)) {
						nameEx.Accept (this);
					} else {
						var newName = new NameEx (nameEx.Name, names.Count);
						unpackEx.Items[i] = newName;
						names.Add (newName);
					}
					unpackEx.Tuple.Accept (this);
				} else {
					base.Visit (unpackEx);
				}
			}
		}

		internal override void Visit (NameEx nameEx)
		{
			int i =	names.IndexOf (nameEx);
			if (i >= 0)
				nameEx.SetIndex (i);
		}

		internal override void Visit (ForEx forEx)
		{
			if (names.Contains (forEx.Iterator))
				forEx.Iterator.Accept (this);
			else {
				forEx.SetIterator (new NameEx (forEx.Iterator.Name, names.Count));
				names.Add (forEx.Iterator);
			}
			forEx.Start.Accept (this);
			forEx.End.Accept (this);
			if (forEx.Step != null)
				forEx.Step.Accept (this);
			forEx.Body.Accept (this);
		}

		internal override void Visit (ForEachEx forEachEx)
		{
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

