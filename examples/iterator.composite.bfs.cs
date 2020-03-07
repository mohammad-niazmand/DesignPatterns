abstract class Component
{
	public string Name { get; protected set; }

	public Component(string name)
	{
		this.Name = name;
	}
	protected virtual void GetChildren(IList<Component> queue)
	{ }
	public static IEnumerable<Component> BFS(Component component)
	{
		var queue = new List<Component>();
		component.GetChildren(queue);

		var marker = 0;
		while (true)
		{
			var count = queue.Count;
			var index = marker;
			while (index < count)
			{
				queue[index].GetChildren(queue);

				index++;
				marker++;
			}
			if (queue.Count == count)
				break;
		}

		yield return component;

		foreach (var x in queue)
		{
			yield return x;
		}
	}
}
class Composite : Component
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
	protected override void GetChildren(IList<Component> queue)
	{
		foreach (var x in this.children)
		{
			queue.Add(x);
		}
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
				new Composite("X",
				new Component[]
				{
					new Composite("Q",
						new Component[]
						{
							new Leaf("QA")
						})
				}),
				new Composite("Y",
					new Component[]
					{
						new Leaf("YA")
					}),
				new Leaf("A"),
				new Composite("Z",
					new Component[]
					{
						new Leaf("ZA"),
						new Composite("R",
						new Component[]
							{
								new Leaf("RA"),
								new Leaf("RB")
							}
						),
						new Leaf("ZB")
					}
				),
				new Leaf("B")
			}
		);

		foreach (var x in Component.BFS(component))
		{
			Console.Write(x.Name + " ");
		}

		Console.ReadKey();
	}
}
/* OUTPUT:
root X Y A Z B Q YA ZA R ZB QA RA RB
*/