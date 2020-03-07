public abstract class Product
{
	public abstract void Show();
}
public class DefaultProduct: Product
{
	public override void Show()
	{
		Console.WriteLine("I'm DefaultProduct");
	}
}
public class ConcreteProductA: Product
{
	public override void Show()
	{
		Console.WriteLine("I'm ConcreteProductA");
	}
}
public class ConcreteProductB: Product
{
	public override void Show()
	{
		Console.WriteLine("I'm ConcreteProductB");
	}
}
public abstract class Creator
{
	public abstract Product FactoryMethod(int a);
}
public class ConcreteCreator: Creator
{
	public override Product FactoryMethod(int a)
	{
		if (a < 0)
			return new ConcreteProductA();
		else if (a > 0)
			return new ConcreteProductB();
		else
			return new DefaultProduct();
	}
}
public class Test
{
	public static void Main()
	{
		var creator = new ConcreteCreator();
		Product p;
		
		p = creator.FactoryMethod(-1);
		p.Show();
		
		p = creator.FactoryMethod(1);
		p.Show();
		
		p = creator.FactoryMethod(0);
		p.Show();
		
		Console.ReadKey();
	}
}
/* Output
I'm ConcreteProductA
I'm ConcreteProductB
I'm DefaultProduct
*/
