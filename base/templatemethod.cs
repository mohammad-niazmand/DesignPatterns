abstract class AbstractClass
{
	public void TemplateMethod()
	{
		Console.WriteLine("\nAbstractClass.TemplateMethod() started");
		PrimitiveOperation1();
		PrimitiveOperation2();
		Console.WriteLine("AbstractClass.TemplateMethod() ended");
	}
	public abstract void PrimitiveOperation1();
	public abstract void PrimitiveOperation2();
}
class ConcreteClassA: AbstractClass
{
	public override void PrimitiveOperation1()
	{
		Console.WriteLine("ConcreteClassA.PrimitiveOperation1()");
	}
	public override void PrimitiveOperation2()
	{
		Console.WriteLine("ConcreteClassA.PrimitiveOperation2()");
	}
}
class ConcreteClassB: AbstractClass
{
	public override void PrimitiveOperation1()
	{
		Console.WriteLine("ConcreteClassB.PrimitiveOperation1()");
	}
	public override void PrimitiveOperation2()
	{
		Console.WriteLine("ConcreteClassB.PrimitiveOperation2()");
	}
}
class Test
{
	public static void Main()
	{
		var c1 = new ConcreteClassA();
		c1.TemplateMethod();
		
		var c2 = new ConcreteClassB();
		c2.TemplateMethod();
		
		Console.ReadKey();
	}
}
/* OUTPUT:
AbstractClass.TemplateMethod() started
ConcreteClassA.PrimitiveOperation1()
ConcreteClassA.PrimitiveOperation2()
AbstractClass.TemplateMethod() ended

AbstractClass.TemplateMethod() started
ConcreteClassB.PrimitiveOperation1()
ConcreteClassB.PrimitiveOperation2()
AbstractClass.TemplateMethod() ended
*/