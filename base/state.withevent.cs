abstract class State
{
	public string Name { get; protected set; }
	public abstract void Handle1(Context context);
	public abstract void Handle2(Context context);
}
class ConcreteStateA: State
{
	public ConcreteStateA()
	{
		Name = "ConcreteStateA";
	}
	public override void Handle1(Context context)
	{
		context.SetState(new ConcreteStateB());
	}
	public override void Handle2(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateA: Context doesn't support this method at this state.");
	}
}
class ConcreteStateB: State
{
	public ConcreteStateB()
	{
		Name = "ConcreteStateB";
	}
	public override void Handle1(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateB: Context doesn't support this method at this state.");
	}
	public override void Handle2(Context context)
	{
		context.SetState(new ConcreteStateA());
	}
}
class Context
{
	public State State { get; set; }
	public event EventHandler OnBeforeStateChanged = delegate { };
	public event EventHandler OnAfterStateChanged = delegate { };
	public Context()
	{
		State = new ConcreteStateA();
	}
	public void SetState(State state)
	{
		OnBeforeStateChanged(this, EventArgs.Empty);
		this.State = state;
		OnAfterStateChanged(this, EventArgs.Empty);
	}
	public void Request1()
	{
		Console.WriteLine();
		State.Handle1(this);
	}
	public void Request2()
	{
		Console.WriteLine();
		State.Handle2(this);
	}
}
class Test
{
	public static void ContextStateChangedHandler(object sender, EventArgs args)
	{
		Console.WriteLine("ContextState changed.");
		Console.WriteLine("Current State is: {0}.", ((Context)sender).State.Name);
	}
	public static void Main()
	{
		var c = new Context();
		c.OnAfterStateChanged += new EventHandler(ContextStateChangedHandler);
		
		c.Request1();
		c.Request2();
		
		Console.ReadKey();
	}
}
/* OUTPUT:
ContextState changed.
Current State is: ConcreteStateB.

ContextState changed.
Current State is: ConcreteStateA.
*/