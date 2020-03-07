public abstract class Component
{
	public abstract void Operation();
}
public class ConcreteComponent: Component
{
	public override void Operation()
	{
		Console.WriteLine("ConcreteComponent.Operation()");
	}
}
public abstract class Decorator: Component
{
	protected Component component;
	public Decorator(Component component)
	{
		this.component = component;
	}
	public override void Operation()
	{
		component.Operation();
	}
}
public class ConcreteDecoratorA: Decorator
{
	public ConcreteDecoratorA(Component component): base(component)	{ }
	public override void Operation()
	{
		base.Operation();
		Console.WriteLine("ConcreteDecoratorA.Operation()");
	}
}
public class ConcreteDecoratorB: Decorator
{
	public ConcreteDecoratorB(Component component): base(component)	{ }
	public override void Operation()
	{
		base.Operation();
		Console.WriteLine("ConcreteDecoratorB.Operation()");
	}
}
public class test
{
	public static void Main()
	{
		ConcreteComponent cc = new ConcreteComponent();
		cc.Operation();
		Console.WriteLine();
		
		ConcreteDecoratorA cd1 = new ConcreteDecoratorA(cc);
		cd1.Operation();
		Console.WriteLine();
		
		ConcreteDecoratorB cd2 = new ConcreteDecoratorB(cd1);
		cd2.Operation();
		Console.WriteLine();
		
		Console.ReadKey();
	}
}
/*	OUTPUT:
ConcreteComponent.Operation()

ConcreteComponent.Operation()
ConcreteDecoratorA.Operation()

ConcreteComponent.Operation()
ConcreteDecoratorA.Operation()
ConcreteDecoratorB.Operation()
*/