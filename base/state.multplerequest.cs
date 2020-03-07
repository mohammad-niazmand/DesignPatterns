abstract class State
{
	public abstract void Handle1(Context context);
	public abstract void Handle2(Context context);
	public abstract void Handle3(Context context);
}
class ConcreteStateA: State
{
	public override void Handle1(Context context)
	{
		Console.WriteLine("ConcreteStateA.Handle1()");
		Console.WriteLine("State Changed to => ConcreteStateB");
		
		context.State = new ConcreteStateB();
	}
	public override void Handle2(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateA: Context doesn't support this method at this state.");
	}
	public override void Handle3(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateA: Context doesn't support this method at this state.");
	}
}
class ConcreteStateB: State
{
	public override void Handle1(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateB: Context doesn't support this method at this state.");
	}
	public override void Handle2(Context context)
	{
		Console.WriteLine("ConcreteStateB.Handle2()");
		Console.WriteLine("State Changed to => ConcreteStateC");
		
		context.State = new ConcreteStateC();
	}
	public override void Handle3(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateB: Context doesn't support this method at this state.");
	}
}
class ConcreteStateC: State
{
	public override void Handle1(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateC: Context doesn't support this method at this state.");
	}
	public override void Handle2(Context context)
	{
		Console.WriteLine("CurrentState=ConcreteStateC: Context doesn't support this method at this state.");
	}
	public override void Handle3(Context context)
	{
		Console.WriteLine("ConcreteStateB.Handle3()");
		Console.WriteLine("State Changed to => ConcreteStateA");
		
		context.State = new ConcreteStateA();
	}
}
class Context
{
	public State State { get; set; }
	public Context()
	{
		State = new ConcreteStateA();
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
	public void Request3()
	{
		Console.WriteLine();
		State.Handle3(this);
	}
}
class Test
{
	public static void Main()
	{
		var c = new Context();
		
		// using Context properly
		c.Request1();
		c.Request2();
		c.Request3();
		
		c.Request1();
		c.Request2();
		c.Request3();
		
		// using Context improperly
		c.Request3();
		c.Request2();
		
		Console.ReadKey();
	}
}
/* OUTPUT:
ConcreteStateA.Handle1()
State Changed to => ConcreteStateB

ConcreteStateB.Handle2()
State Changed to => ConcreteStateC

ConcreteStateB.Handle3()
State Changed to => ConcreteStateA

ConcreteStateA.Handle1()
State Changed to => ConcreteStateB

ConcreteStateB.Handle2()
State Changed to => ConcreteStateC

ConcreteStateB.Handle3()
State Changed to => ConcreteStateA

CurrentState=ConcreteStateA: Context doesn't support this method at this state.

CurrentState=ConcreteStateA: Context doesn't support this method at this state.
*/