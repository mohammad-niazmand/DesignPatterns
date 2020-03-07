public abstract class State
{
	public abstract void Handle(Context context);
}
public class ConcreteStateA: State
{
	public override void Handle(Context context)
	{
		Console.WriteLine("\nRequest handled by ConcreteStateA");
		Console.WriteLine("State changed to ConcreteStateB");
		
		context.State = new ConcreteStateB();
	}
}
public class ConcreteStateB: State
{
	public override void Handle(Context context)
	{
		Console.WriteLine("\nRequest handled by ConcreteStateB");
		Console.WriteLine("State changed to ConcreteStateC");
		
		context.State = new ConcreteStateC();
	}
}
public class ConcreteStateC: State
{
	public override void Handle(Context context)
	{
		Console.WriteLine("\nRequest handled by ConcreteStateC");
		Console.WriteLine("State changed to ConcreteStateA");
		
		context.State = new ConcreteStateA();
	}
}
public class Context
{
	public State State { get; set; }
	
	public Context()
	{
		this.State = new ConcreteStateA();
	}
	public void Request()
	{
		State.Handle(this);
	}
}
class Test
{
	public static void Main()
	{
		var c = new Context();
		
		c.Request();
		c.Request();
		c.Request();
		c.Request();
		c.Request();
		c.Request();
		
		Console.ReadKey();
	}
}
/* OUTPUT:
Request handled by ConcreteStateA
State changed to ConcreteStateB

Request handled by ConcreteStateB
State changed to ConcreteStateC

Request handled by ConcreteStateC
State changed to ConcreteStateA

Request handled by ConcreteStateA
State changed to ConcreteStateB

Request handled by ConcreteStateB
State changed to ConcreteStateC

Request handled by ConcreteStateC
State changed to ConcreteStateA
*/