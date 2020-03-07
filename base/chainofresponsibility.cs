abstract class Handler
{
	protected Handler successor;
	protected int from;
	protected int to;
	
	public Handler(int from, int to, Handler successor)
	{
		this.successor = successor;
		this.from = from;
		this.to = to;
	}
	public virtual void HandleRequest(int request)
	{
		if (request >= this.from && request < this.to)
		{
			Console.WriteLine("{0} handled request for {1}", this.GetType().Name, request);
		}
		else if (successor != null)
		{
			successor.HandleRequest(request);
		}
		else
			Console.WriteLine("Request for {0} can not be handled", request);
	}
}
class ConcreteHandler1: Handler
{
	public ConcreteHandler1(Handler successor = null): base(1, 10, successor)
	{   }
}
class ConcreteHandler2: Handler
{
	public ConcreteHandler2(Handler successor = null): base(11, 20, successor)
	{   }
}
class ConcreteHandler3: Handler
{
	public ConcreteHandler3(Handler successor = null): base(21, 30, successor)
	{   }
}
class Test
{
	static void Main()
	{
		var h1 = new ConcreteHandler1();
		var h2 = new ConcreteHandler2(h1);
		var h3 = new ConcreteHandler3(h2);

		h3.HandleRequest(8);
		h3.HandleRequest(12);
		h3.HandleRequest(3);
		h3.HandleRequest(24);
		h3.HandleRequest(19);
		h3.HandleRequest(33);
		h3.HandleRequest(17);
		h3.HandleRequest(1);
		h3.HandleRequest(15);
		h3.HandleRequest(28);
		
		Console.ReadKey();
	}
}
/* OUTPUT:
ConcreteHandler1 handled request for 8
ConcreteHandler2 handled request for 12
ConcreteHandler1 handled request for 3
ConcreteHandler3 handled request for 24
ConcreteHandler2 handled request for 19
Request for 33 can not be handled
ConcreteHandler2 handled request for 17
ConcreteHandler1 handled request for 1
ConcreteHandler2 handled request for 15
ConcreteHandler3 handled request for 28
*/