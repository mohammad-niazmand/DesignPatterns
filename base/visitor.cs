abstract class Visitor
{
	public abstract void VisitConcreteElementA(ConcreteElementA element);
	public abstract void VisitConcreteElementB(ConcreteElementB element);
}
class ConcreteVisitor1 : Visitor
{
	public override void VisitConcreteElementA(ConcreteElementA element)
	{
		Console.WriteLine("{0}: {1} visiting by {2} ...",
													element.GetType().Name,
													element.Name,
													this.GetType().Name);
		element.OperationA();
	}
	public override void VisitConcreteElementB(ConcreteElementB element)
	{
		Console.WriteLine("{0}: {1} visiting by {2} ...",
													element.GetType().Name,
													element.ID,
													this.GetType().Name);
		element.OperationB();
	}
}
abstract class Element
{
	public abstract void Accept(Visitor visitor);
}
class ConcreteElementA : Element
{
	private string name;
	public string Name
	{
		get { return name; }
	}
	public ConcreteElementA(string name)
	{
		this.name = name;
	}
	public override void Accept(Visitor visitor)
	{
		visitor.VisitConcreteElementA(this);
	}
	public void OperationA()
	{
		Console.WriteLine("\tOperationA()");
	}
}
class ConcreteElementB: Element
{
	private string id;
	public string ID
	{
		get { return id; }
	}
	public ConcreteElementB(string id)
	{
		this.id = id;
	}
	public override void Accept(Visitor visitor)
	{
		visitor.VisitConcreteElementB(this);
	}
	public void OperationB()
	{
		Console.WriteLine("\tOperationB()");
	}
}
class ObjectStructure
{
	private List<Element> elements = new List<Element>();
	public void Add(Element element)
	{
		elements.Add(element);
	}
	public void Accept(Visitor visitor)
	{
		foreach (var element in elements)
		{
			element.Accept(visitor);
		}
	}
}
class Test
{
	static void Main()
	{
		var o = new ObjectStructure();
		
		o.Add(new ConcreteElementA("A1"));
		o.Add(new ConcreteElementA("A2"));
		o.Add(new ConcreteElementB("B1"));
		o.Add(new ConcreteElementA("A3"));
		o.Add(new ConcreteElementB("B2"));

		var visitor1 = new ConcreteVisitor1();
		o.Accept(visitor1);
		
		Console.ReadKey();
	}
}
/* OUTPUT:
ConcreteElementA: A1 visiting by ConcreteVisitor1 ...
        OperationA()
ConcreteElementA: A2 visiting by ConcreteVisitor1 ...
        OperationA()
ConcreteElementB: B1 visiting by ConcreteVisitor1 ...
        OperationB()
ConcreteElementA: A3 visiting by ConcreteVisitor1 ...
        OperationA()
ConcreteElementB: B2 visiting by ConcreteVisitor1 ...
        OperationB()
*/