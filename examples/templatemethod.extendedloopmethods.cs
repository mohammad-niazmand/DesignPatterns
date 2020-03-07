static class MyExtensionMethods
{
	public static int Count<T>(this IEnumerable<T> items)
	{
		var count = 0;
		var enumerator = items.GetEnumerator();
		while (enumerator.MoveNext())
		{
			count++;
		}
		return count;
	}
	public static int IndexOf<T>(this IEnumerable<T> items, T item) where T: IEquatable<T>
	{
		var index = -1;
		var enumerator = items.GetEnumerator();
		while (enumerator.MoveNext())
		{
			index++;
			if (enumerator.Current.Equals(item))
				break;
		}
		return index;
	}
	public static int FindMax<T>(this IEnumerable<T> items) where T: IComparable<T>
	{
		var max = default(T);
		var index = -1;
		var enumerator = items.GetEnumerator();
		while (enumerator.MoveNext())
		{
			index++;
			if (enumerator.Current.CompareTo(max) > 0)
				max = enumerator.Current;
		}
		return index;
	}
	public static void Print<T>(this IEnumerable<T> items)
	{
		var enumerator = items.GetEnumerator();
		var count = -1;
		while (enumerator.MoveNext())
		{
			count++;
			if (count > 0)
				Console.Write(", ");
			Console.Write(enumerator.Current);
		}
		Console.WriteLine();
	}
}
public static class Tests
{
	static void Main()
	{
		var data = new List<int> { 23, 15, 16, 24, -9, 11, 32, 14 };
		Console.WriteLine("List of numbers:");
		data.Print();
		Console.WriteLine("Number of Items: " + data.Count());
		Console.WriteLine("Index of {0}: {1}", -9, data.IndexOf(-9));
		Console.WriteLine("Maximum: {0}", data.FindMax());

		Console.ReadKey();
	}
}