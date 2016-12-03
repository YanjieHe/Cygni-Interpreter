using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;

namespace CygniLib.Collections
{
	public class linkedListNode: IDot
	{
		LinkedListNode<DynValue> node;

		public linkedListNode (LinkedListNode<DynValue>node)
		{
			this.node = node;
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "value":
				return this.node.Value;
			case "previous":
				return DynValue.FromUserData (new linkedListNode (this.node.Previous));
			case "next":
				return DynValue.FromUserData (new linkedListNode (this.node.Next));
			case "list":
				return DynValue.FromUserData (this.node.List as linkedList);
			default:
				throw RuntimeException.FieldNotExist ("linkedListNode", fieldName);
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
				throw RuntimeException.FieldNotExist ("linkedListNode", fieldName);
			}
		}
	}
}

