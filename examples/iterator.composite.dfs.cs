abstract class Component : IEnumerable<Component>
{
	public string Name { get; protected set; }

	public Component(string name)
	{
		this.Name = name;
	}
	public virtual IEnumerator<Component> GetEnumerator()
	{
		yield return this;
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
	public virtual IEnumerable<Component> Enumerate()
	{
		yield return this;
	}
}
class Composite: Component, IEnumerable<Component>
{
	private List<Component> children { get; set; }

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
	public override IEnumerable<Component> Enumerate()
	{
		yield return this;

		if (this.children != null)
			foreach (var x in this.children)
			{
				foreach (var y in x.Enumerate())
					yield return y;
			}
	}
	public override IEnumerator<Component> GetEnumerator()
	{
		var structure = Enumerate();

		foreach (var x in structure)
			yield return x;
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}
class Leaf : Component
{
    public Leaf(string name): base(name)
    { }
}
public class Test
{
	public static void Main()
	{
		var component = new Composite("root",
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
		
		foreach (var c in component)
		{
			Console.WriteLine(c.Name);
		}
		
		Console.ReadKey();
    }
}
/* OUTPUT:
root
Leaf A
Leaf B
Composite X
Leaf XA
Leaf XB
Leaf C
*/