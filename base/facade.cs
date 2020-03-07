public class SubSystemA
{
	public void OperationA()
	{
		Console.WriteLine("\tSubSystemA.OperationA()");
	}
}
public class SubSystemB
{
	public void OperationB1()
	{
		Console.WriteLine("\tSubSystemB.OperationB1()");
	}
	public void OperationB2()
	{
		Console.WriteLine("\tSubSystemB.OperationB2()");
	}
}
public class SubSystemC
{
	public void OperationC1()
	{
		Console.WriteLine("\tSubSystemC.OperationC1()");
	}
	public void OperationC2()
	{
		Console.WriteLine("\tSubSystemC.OperationC2()");
	}
	public void OperationC3()
	{
		Console.WriteLine("\tSubSystemC.OperationC3()");
	}
}
public class Facade
{
	private SubSystemA ssa;
	private SubSystemB ssb;
	private SubSystemC ssc;

	public Facade()
	{
		ssa = new SubSystemA();
		ssb = new SubSystemB();
		ssc = new SubSystemC();
	}
	public void ServiceX()
	{
		Console.WriteLine("\nFacade.ServiceX() ... ");
		
		ssa.OperationA();
		ssb.OperationB2();
	}
	public void ServiceY()
	{
		Console.WriteLine("\nFacade.ServiceY() ... ");
		
		ssc.OperationC3();
		ssb.OperationB1();
		ssb.OperationB2();
	}
	public void ServiceZ()
	{
		Console.WriteLine("\nFacade.ServiceZ() ... ");
		ssb.OperationB2();
		ssa.OperationA();
		ssc.OperationC2();
		ssc.OperationC1();
	}
}
public class Test
{
	public static void Main()
	{
		Facade facade = new Facade();

		facade.ServiceX();
		facade.ServiceY();
		facade.ServiceZ();

		Console.ReadKey();
	}
}
/* Output
Facade.ServiceX() ...
        SubSystemA.OperationA()
        SubSystemB.OperationB2()

Facade.ServiceY() ...
        SubSystemC.OperationC3()
        SubSystemB.OperationB1()
        SubSystemB.OperationB2()

Facade.ServiceZ() ...
        SubSystemB.OperationB2()
        SubSystemA.OperationA()
        SubSystemC.OperationC2()
        SubSystemC.OperationC1()
*/