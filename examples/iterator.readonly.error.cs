class Test
{
	static void Main()
	{
		var data = new List<int>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
		};
		Action action1 = () =>
		{
			try
			{
				foreach (var item in data)
				{
					Console.Write(item + " ");
					Thread.Sleep(50);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("\naction1 error: " + e.Message);
			}
			Console.WriteLine();
		};
		Action action2 = () =>
		{
			try
			{
				data.Add(50);
			}
			catch (Exception e)
			{
				Console.WriteLine("\naction2 error: " + e.Message);
			}
		};
		try
		{
			Parallel.Invoke(action1, action2);
		}
		catch (AggregateException e)
		{
			Console.WriteLine("\n" + e.Message);
		}
		Console.WriteLine("Total Items: " + data.Count);
		Console.ReadKey();
	}
}
/* OUTPUT:
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15
action1 error: Collection was modified; enumeration operation may not execute.
Total Items: 21
*/