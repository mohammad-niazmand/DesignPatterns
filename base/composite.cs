public abstract class Component
{
    protected string name;
    public Component(string name)
    {
        this.name = name;
    }
    public abstract void Add(Component c);
    public abstract void Remove(Component c);
    public abstract void Display(int depth);
}
class Composite : Component
{
    private List<Component> components = new List<Component>();
    public Composite(string name): base(name)
    { }
    public Composite(string name, Component[] components): base(name)
    {
        foreach (Component component in components)
        {
            Add(component);
        }
    }
    public override void Add(Component component)
    {
        components.Add(component);
    }
    public override void Remove(Component component)
    {
        components.Remove(component);
    }
    public override void Display(int depth)
    {
        Console.WriteLine(new String('-', depth) + name);
        foreach (Component component in components)
        {
            component.Display(depth + 2);
        }
    }
}
class Leaf : Component
{
    public Leaf(string name): base(name)
    { }
    public override void Add(Component c)
    {
        Console.WriteLine("Cannot add to a leaf");
    }
    public override void Remove(Component c)
    {
        Console.WriteLine("Cannot remove from a leaf");
    }
    public override void Display(int depth)
    {
        Console.WriteLine(new String('-', depth) + name);
    }
}
public class Test
{
    public static void Main()
    {
        Component c = new Composite("root",
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
		
        c.Display(1);
		
		Console.ReadKey();
    }
}
/* Output
-root
---Leaf A
---Leaf B
---Composite X
-----Leaf XA
-----Leaf XB
---Leaf C
*/
