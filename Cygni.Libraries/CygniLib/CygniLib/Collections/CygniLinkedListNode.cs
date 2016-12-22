using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Collections
{
	public class CygniLinkedListNode: IDot
	{
		LinkedListNode<DynValue> node;

		public CygniLinkedListNode (LinkedListNode<DynValue>node)
		{
			this.node = node;
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "value":
				return this.node.Value;
			case "previous":
				return DynValue.FromUserData (new CygniLinkedListNode (this.node.Previous));
			case "next":
				return DynValue.FromUserData (new CygniLinkedListNode (this.node.Next));
			case "list":
				return DynValue.FromUserData (this.node.List as CygniLinkedList);
			default:
				throw RuntimeException.FieldNotExist ("LinkedListNode", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"value", "previous", "next", "list"
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			switch (fieldName) {
			case "value":
				return this.node.Value = value;
			default:
				throw RuntimeException.FieldNotExist ("LinkedListNode", fieldName);
			}
		}
	}
}

