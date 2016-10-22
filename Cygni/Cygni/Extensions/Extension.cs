using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
namespace Cygni.Extensions
{
	/// <summary>
	/// Description of Extension.
	/// </summary>
	public static class Extension
	{
		public static R[] Map<T,R>(this IList<T>list, Func<T,R> converter)
		{
			int n = list.Count;
			var array = new R[n];
			for (int i = 0; i < n; i++) {
				array[i] = converter(list[i]);
			}
			return array;
		}
		public static R[] SkipMap<T,R>(this IList<T>list, int count, Func<T,R> converter)
		{
			int n = list.Count - count;
			var array = new R[n];
			for (int i = 0; i < n; i++) {
				array[i] = converter(list[i + count]);
			}
			return array;
		}
		public static DynList ToDynList<T>(this IEnumerable<T> collection, Func<T,DynValue> converter)
		{
			return new DynList(collection.Select(converter));
		}
		public static DynList ToDynList<T>(this IEnumerable<T> collection, Func<T,DynValue> converter,int count)
		{
			return new DynList(collection.Select(converter),count);
		}
	}
}
