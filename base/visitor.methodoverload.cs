interface Element
{
	string Name { get; }
	void Accept(ConcreteVisitor visitor);
}
class ConcreteElementA: Element
{
	public string Name
	{
		get; private set;
	}
	public ConcreteElementA(string name)
	{
		Name = name;
	}
	public void Accept(ConcreteVisitor visitor)
	{
		visitor.Visit(this);
	}
}
class ConcreteElementB: Element
{
	public string Name
	{
		get; private set;
	}
	public ConcreteElementB(string name)
	{
		Name = name;
	}
	public void Accept(ConcreteVisitor visitor)
	{
		visitor.Visit(this);
	}
}
class ConcreteElementC: Element
{
	public string Name
	{
		get; private set;
	}
	public ConcreteElementC(string name)
	{
		Name = name;
	}
	public void Accept(ConcreteVisitor visitor)
	{
		visitor.Visit(this);
	}
}
class ConcreteVisitor
{
	public void Visit(ConcreteElementA element)
	{
		Console.WriteLine("Visiting " + element.Name + " ...");
	}
	public void Visit(ConcreteElementB element)
	{
		Console.WriteLine("Visiting " + element.Name + " ...");
	}
	public void Visit(ConcreteElementC element)
	{
		Console.WriteLine("Visiting " + element.Name + " ...");
	}
}
class Tests
{
	public static void Main()
	{
		var elements = new Element[]
			{
				new ConcreteElementA("EA1"),
				new ConcreteElementA("EA2"),
				new ConcreteElementB("EB1"),
				new ConcreteElementA("EA3"),
				new ConcreteElementC("EC1"),
				new ConcreteElementC("EC2"),
				new ConcreteElementA("EA5"),
				new ConcreteElementB("EB2"),
				new ConcreteElementC("EC3")
			};
		ConcreteVisitor visitor = new ConcreteVisitor();
		foreach(var element in elements)
		{
			element.Accept(visitor);
		}
		Console.ReadKey();
	}
}
/*
Visiting EA1 ....
Visiting EA2 ....
Visiting EB1 ....
Visiting EA3 ....
Visiting EC1 ....
Visiting EC2 ....
Visiting EA5 ....
Visiting EB2 ....
Visiting EC3 ....
*/