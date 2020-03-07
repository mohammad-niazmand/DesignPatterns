abstract class Component
{
	public string Name { get; protected set; }

	public Component(string name)
	{
		this.Name = name;
	}
	public abstract void Accept(Visitor visitor);
}
class Composite: Component
{
	public List<Component> children { get; protected set; }

	public Composite(string name): base(name)
	{
		this.children = new List<Component>();
	}
	public Composite(string name, Component[] children): base(name)
	{
		this.children = new List<Component>();
		
		foreach (var component in children)
		{
			this.children.Add(component);
		}
	}
	public override void Accept(Visitor visitor)
	{
		visitor.Visit(this);
		
		foreach (var component in children)
		{
			component.Accept(visitor);
		}
	}
}
class Leaf : Component
{
    public Leaf(string name): base(name)
    { }
	public override void Accept(Visitor visitor)
	{
		visitor.Visit(this);
	}
}
abstract class Visitor
{
	public abstract void Visit(Composite c);
	public abstract void Visit(Leaf c);
}
class DisplayVisitor: Visitor
{
	public override void Visit(Composite c)
	{
		Console.WriteLine("{0, -10}{1, -16}{2, -5}", c.GetType().ToString(), c.Name, c.children.Count);
	}
	public override void Visit(Leaf c)
	{
		Console.WriteLine("{0, -10}{1, -16}{2, -5}", c.GetType().ToString(), c.Name, "-");
	}
}
public class Test
{
	public static void Main()
	{
		Component component = new Composite("root",
		new Component[]
			{
				new Leaf("Leaf A"),
				new Leaf("Leaf B"),
				new Composite("Composite X",
				new Component[]
				{
					new Leaf("Leaf XA"),
					new Leaf("Leaf XB")
				}
				),
				new Leaf("Leaf C")
			}
		);
		
		var visitor = new DisplayVisitor();
		Console.WriteLine("{0, -10}{1, -14}{2, -5}", "Type", "Name", "children Count");
		Console.WriteLine(new String('-', 40));
		component.Accept(visitor);
		
		Console.ReadKey();
    }
}
/* OUTPUT:
Type      Name          children Count
----------------------------------------
Composite root            4
Leaf      Leaf A          -
Leaf      Leaf B          -
Composite Composite X     2
Leaf      Leaf XA         -
Leaf      Leaf XB         -
Leaf      Leaf C          -
*/