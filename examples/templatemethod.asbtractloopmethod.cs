abstract class AbstractLoop
{
	protected int Run()
	{
		int i = -1;
		if (Begin())
		{
			i = 0;
			Initialize();
			while (!Done(i))
			{
				if (!Process(i++))
					break;
			}
			Finalize(i);
		}
		return End(i);
	}
	protected virtual bool Begin() { return true; }
	protected virtual void Initialize() { }
	protected abstract bool Done(int index);
	protected virtual bool Process(int index) { return true; }
	protected virtual void Finalize(int index) { }
	protected virtual int End(int count) { return count; }
}
class EnumerableSearcher<T> : AbstractLoop where T : IEquatable<T>
{
	private IEnumerator<T> enumerator;
	private T item;
	private bool found;
	public EnumerableSearcher(IEnumerable<T> items)
	{
		this.enumerator = items.GetEnumerator();
	}
	protected override bool Done(int index)
	{
		return !enumerator.MoveNext();
	}
	protected override bool Process(int index)
	{
		if (enumerator.Current.Equals(item))
			found = true;
		
		return !found;
	}
	protected override int End(int count)
	{
		if (!found)
			return -1;
		else
			return count - 1;
	}
	public int IndexOf(T item)
	{
		this.item = item;
		
		enumerator.Reset();
		
		return Run();
	}
}
class EnumerableFindMax<T> : AbstractLoop where T : IComparable<T>
{
	private IEnumerator<T> enumerator;
	private T max;
	private bool found;
	public EnumerableFindMax(IEnumerable<T> items)
	{
		this.enumerator = items.GetEnumerator();
	}
	protected override bool Done(int index)
	{
		return !enumerator.MoveNext();
	}
	protected override bool Process(int index)
	{
		if (enumerator.Current.CompareTo(max) > 0)
			max = enumerator.Current;
		return true;
	}
	public T FindMax()
	{
		enumerator.Reset();
		max = default(T);
		Run();
		return max;
	}
}
class EnumerableCounter<T> : AbstractLoop
{
	private IEnumerator<T> enumerator;
	
	public EnumerableCounter(IEnumerable<T> items)
	{
		enumerator = items.GetEnumerator();
	}
	protected override bool Done(int index)
	{
		return !enumerator.MoveNext();
	}
	public int Count()
	{
		enumerator.Reset();
		
		return Run();
	}
}
class ForAll<T> : AbstractLoop
{
	private IEnumerator<T> enumerator;
	private Action<int, T> action;
	
	public ForAll(IEnumerable<T> items, Action<int, T> action)
	{
		this.enumerator = items.GetEnumerator();
		this.action = action;
	}
	protected override bool Done(int index)
	{
		return !enumerator.MoveNext();
	}
	protected override bool Process(int index)
	{
		action(index, enumerator.Current);

		return true;
	}
	public new void Run()
	{
		enumerator.Reset();
		
		Run();
	}
}
class EnumerablePrinter<T> : AbstractLoop
{
	private IEnumerator<T> enumerator;
	
	public EnumerablePrinter(IEnumerable<T> items)
	{
		this.enumerator = items.GetEnumerator();
	}
	protected override bool Done(int index)
	{
		return !enumerator.MoveNext();
	}
	protected override bool Process(int index)
	{
		if (index > 0)
			Console.Write(", ");
		
		Console.Write(enumerator.Current);

		return true;
	}
	public void Print()
	{
		enumerator.Reset();
		
		Run();
		
		Console.WriteLine();
	}
}
public class Tests
{
	static void Main()
	{
		var data = new List<int> { 23, 15, 16, 24, -9, 11, 32, 14 };

		var item = -9;
		var es = new EnumerableSearcher<int>(data);
		var i = es.IndexOf(item);
		
		var ec = new EnumerableCounter<int>(data);
		var count = ec.Count();
		
		var em = new EnumerableFindMax<int>(data);
		var max = em.FindMax();
		
		var ep = new EnumerablePrinter<int>(data);
		ep.Print();

		Console.WriteLine("Number of Items: " + count);
		Console.WriteLine("Index of {0}: {1}", item, i);
		Console.WriteLine("Maximum: {0}", max);
		Console.ReadKey();
	}
}