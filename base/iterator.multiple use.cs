class Test
{
    public static void Main()
    {
        var items = new int[]
			{
				12, 19, 10, 12, 8, 11, 12, 10, 12, 4, 4, 10, 4, 6, 8
			};
        var uniqueItems = new Dictionary<int, int>();
		
        Console.WriteLine("Item\tCount");
        Console.WriteLine("----\t----");
		
        var en1 = items.GetEnumerator();
		
        while (en1.MoveNext())
        {
            var item = (int)en1.Current;
            
            if (!uniqueItems.ContainsKey(item))
            {
                var en2 = items.GetEnumerator();
                var count = 0;
				
                while (en2.MoveNext())
                {
                    if ((int)en2.Current == item)
					{
                        count++;
					}
                }

                uniqueItems.Add(item, count);
            }
            
        }
		
        foreach (var x in uniqueItems)
        {
            Console.WriteLine("{0}\t{1}", x.Key, x.Value);
        }
		
        Console.ReadKey();
    }
}
/* OUTPUT:
Item    Count
----    ----
12      4
19      1
10      3
8       2
11      1
4       3
6       1
*/