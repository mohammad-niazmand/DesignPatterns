// Don't forget to add System.Configuration to the list of references in this program
// Also add an "Implementor" entry in the <appSettings> section in app.config file.

public abstract class Implementor
{
	public abstract void OperationImp();
}
public class ConcreteImplementorA: Implementor
{
	public override void OperationImp()
	{
		Console.WriteLine("ConcreteImplementorA.OperationImp()");
	}
}
public class ConcreteImplementorB: Implementor
{
	public override void OperationImp()
	{
		Console.WriteLine("ConcreteImplementorB.OperationImp()");
	}
}
public class DefaultImplementation: Implementor
{
	public override void OperationImp()
	{
		Console.WriteLine("DefaultImplementation.OperationImp()");
	}
}
public static class Implementation
{
	public static Implementor GetImplementation()
	{
		var config = ConfigurationManager.AppSettings;
		
		if (config["Implementor"] == "ConcreteImplementorA")
			return new ConcreteImplementorA();
		else if (config["Implementor"] == "ConcreteImplementorB")
			return new ConcreteImplementorB();
		else
			return new DefaultImplementation();
	}
}
public abstract class Abstraction
{
	private Implementor implementor;
	public Abstraction()
	{
		this.implementor = Implementation.GetImplementation();
	}
	public virtual void Operation()
	{
		Console.WriteLine("Abstraction.Operation()");
		Console.WriteLine("Calling implementation using implementor ...");
		implementor.OperationImp();
	}
}
public class RefinedAbstraction: Abstraction
{ }
public class Client
{
	public static void Main()
	{
		Abstraction ra = new RefinedAbstraction();
		ra.Operation();
		
		Console.ReadKey();
	}
}
/* OUTPUT:
Abstraction.Operation()
Calling implementation using implementor ...
ConcreteImplementorA.OperationImp()
*/