using System;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
namespace CygniLib.Collections
{
	public class array2:IIndexable,IDot
	{
		DynValue[,] values;
		public array2 (int row,int col)
		{
			values = new DynValue[row,col];
			for (int i = 0; i < row; i++)
				for (int j = 0; j < col; j++)
					values [i, j] = DynValue.Null;
		}
		public DynValue this [DynValue[] indexes] {
			get {
				RuntimeException.IndexerArgsCheck (indexes.Length == 2, "array2");
				return values [(int)indexes [0].AsNumber (),
					(int)indexes[1].AsNumber()];
			}
			set {
				RuntimeException.IndexerArgsCheck (indexes.Length == 2, "array2");
				values [(int)indexes [0].AsNumber (),
					(int)indexes[1].AsNumber()] = value;
			}
		}
		public DynValue GetByDot (string fieldname){
			switch (fieldname) {
			case "nRows":
				return (double)values.GetLength (0);
			case "nCols":
				return (double)values.GetLength (1);
			default:
				throw RuntimeException.NotDefined (fieldname);
			}
		}

		public	DynValue SetByDot (string fieldname, DynValue value){
			throw RuntimeException.NotDefined (fieldname);
		}

		public override string ToString ()
		{
			var s = new StringBuilder ("array2([");
			s.AppendLine ();
			int nRows = values.GetLength (0);
			int nCols = values.GetLength (1);
			for (int i = 0; i < nRows; i++) {
				s.Append ('[');
				for (int j = 0; j < nCols; j++) {
					s.Append (values [i, j].ToString ());
					if (j != nCols - 1)
						s.Append (", ");
				}
				s.AppendLine ("]");
			}
			s.Append ("] )");
			return s.ToString ();
		}
	}
}

