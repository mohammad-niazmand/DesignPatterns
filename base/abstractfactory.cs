abstract class AbstractFactory
{
	public abstract void Start();
	public abstract AbstractProductA CreateProductA();
	public abstract AbstractProductB CreateProductB();
}
abstract class AbstractProductA
{
	public abstract void Display();
}
abstract class AbstractProductB
{
	public abstract void Show();
}
class ConcreteFactory1: AbstractFactory
{
	public override void Start()
	{
		Console.WriteLine("ConcreteFactory1");
	}
	public override AbstractProductA CreateProductA()
	{
		return new ProductA1();
	}
	public override AbstractProductB CreateProductB()
	{
		return new ProductB1();
	}
}
class ConcreteFactory2: AbstractFactory
{
	public override void Start()
	{
		Console.WriteLine("ConcreteFactory2");
	}
	public override AbstractProductA CreateProductA()
	{
		return new ProductA2();
	}
	public override AbstractProductB CreateProductB()
	{
		return new ProductB2();
	}
}
class ProductA1: AbstractProductA
{
	public override void Display()
	{
		Console.WriteLine("ProductA1");
	}
}
class ProductA2: AbstractProductA
{
	public override void Display()
	{
		Console.WriteLine("ProductA2");
	}
}
class ProductB1: AbstractProductB
{
	public override void Show()
	{
		Console.WriteLine("ProductB1");
	}
}
class ProductB2: AbstractProductB
{
	public override void Show()
	{
		Console.WriteLine("ProductB2");
	}
}
class Client
{
	private AbstractFactory factory;
	public void SetFactory(AbstractFactory factory)
	{
		this.factory = factory;
	}
	public void Run()
	{
		factory.Start();
		var p1 = factory.CreateProductA();
		var p2 = factory.CreateProductB();
		p1.Display();
		p2.Show();
	}
}
class Test
{
	public static void Main()
	{
		var factory1 = new ConcreteFactory1();
		var client = new Client();
		
		client.SetFactory(factory1);
		client.Run();

		Console.WriteLine();
		
		var factory2 = new ConcreteFactory2();
		client.SetFactory(factory2);
		client.Run();

		Console.ReadKey();
	}
}

/* OUTPUT:
ConcreteFactory1
ProductA1
ProductB1

ConcreteFactory2
ProductA2
ProductB2
*/