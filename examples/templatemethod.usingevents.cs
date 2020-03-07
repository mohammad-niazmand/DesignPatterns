class CancelableListIterationArgs: EventArgs
{
	public int Index { get; private set; }
	public bool Cancel { get; set; }
	public CancelableListIterationArgs(int index)
	{
		this.Index = index;
	}
}
class CancelableList<T> where T: IEquatable<T>
{
	private List<T> items = new List<T>();
	public event EventHandler<CancelableListIterationArgs> OnIterate = delegate { };
	public void Add(T item)
	{
		items.Add(item);
	}
	public void AddMultiple(params T[] items)
	{
		foreach(var item in items)
			this.items.Add(item);
	}
	public int IndexOf(T value)
	{
		var index = -1;
		var found = false;
		foreach(var item in items)
		{
			index++;
			if (item.Equals(value))
			{
				found = true;
				break;
			}
			
			var args = new CancelableListIterationArgs(index);
			OnIterate(this, args);
			if (args.Cancel)
				break;
			Thread.Sleep(300);		// intentioanlly impose a delay
		}
		if (found)
			return index;
		else
			return -1;
	}
}
public class Test
{
	static void OnIterate(object sender, CancelableListIterationArgs e)
	{
		Console.Write(" .");
		if (e.Index > 10)
		{
			Console.WriteLine("\nOperation was canceled at iteration {0}", e.Index);
			e.Cancel = true;
		}
	}
	static void Main()
	{
		var cl = new CancelableList<int>();
		cl.AddMultiple(12, 24, 32, 25, 18, 10, 19, 31, 26, 22, 12, 8, 38);
		cl.OnIterate += new EventHandler<CancelableListIterationArgs>(OnIterate);
		
		var item = 999;
		Console.WriteLine("Finding index of {0} in the list", item);
		
		var index = cl.IndexOf(item);
		Console.WriteLine("Index of {0} was {1}", item, index);
		
		Console.ReadKey();
	}
}
/* OUTUT:
Finding index of 999 in the list
 . . . . . . . . . . . .
Operation was canceled at iteration 11
Index of 999 was -1
*/