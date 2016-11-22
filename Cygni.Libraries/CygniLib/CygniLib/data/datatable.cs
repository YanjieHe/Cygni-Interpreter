using System;
using System.Data;
using Cygni.Errors;
using Cygni.DataTypes;
namespace CygniLib.data
{
	public class datatable:DataTable,IIndexable, IDot
	{
		public datatable ():base()
		{
			
		}

		public DynValue this [DynValue[] indexes] {
			get {
				RuntimeException.IndexerArgsCheck (indexes.Length == 2, "datatable");
				if (indexes [0].type == DataType.String)
					return DynValue.FromObject( this.Rows [(int)indexes [1].AsNumber ()] [indexes [0].AsString ()]);
				else //if (indexes [0].type == DataType.Number)
					return DynValue.FromObject( this.Rows [(int)indexes [0].AsNumber ()] [(int)indexes [1].AsNumber ()]);
			}
			set {	
				RuntimeException.IndexerArgsCheck (indexes.Length == 2, "datatable");
				if (indexes [0].type == DataType.String)
					this.Rows [(int)indexes [1].AsNumber ()] [indexes [0].AsString ()] = value;
				else// if (indexes [0].type == DataType.Number)
					 this.Rows [(int)indexes [0].AsNumber ()] [(int)indexes [1].AsNumber ()] = value;
			}
		}
		 public DynValue GetByDot(string fieldname){
			switch(fieldname){
			case "nRows":
				return (double)this.Rows.Count;
			case "nCols":
				return (double)this.Columns.Count;
			case "addRow":
				return DynValue.FromDelegate (
					args => {
						RuntimeException.FuncArgsCheck(args.Length == 1,"addRow");
						var list = args[0].As<DynList>();
						var objs = new object[list.Count];
						for(int i = 0;i<objs.Length;i++)
							objs[i]=list[i].Value;
						this.Rows.Add(objs);
						return DynValue.Null;
					});
			case "addColumn":
				return DynValue.FromDelegate (
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 2, "addColumn");
						string name = args [0].AsString ();
						Type type = Type.GetType (args [1].AsString ());
						this.Columns.Add (name, type);
						return DynValue.Null;
					});
			default:
				throw RuntimeException.NotDefined (fieldname);
			}
		}
		public string[] FieldNames{get{ return new string[] {
					"nRows","nCols","addRow","addColumn"
			};
			}
		}
		public DynValue SetByDot(string fieldname, DynValue value){
			throw	RuntimeException.NotDefined (fieldname);
		}
		public override string ToString ()
		{
			return "(DataTable)";
		}
	}
}

