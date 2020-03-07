public class FlyweightFactory
{
	private Dictionary<char, Flyweight> flyweights = new Dictionary<char, Flyweight>();
	public FlyweightFactory()
	{
		flyweights.Add('a', new ConcreteFlyweight('a'));
		flyweights.Add('b', new ConcreteFlyweight('b'));
		flyweights.Add('c', new ConcreteFlyweight('c'));
	}
	public Flyweight GetFlyweight(char key)
	{
		return flyweights[key];
	}
}
public abstract class Flyweight
{
	public abstract void Operation(string extrinsicstate);
}
internal class ConcreteFlyweight : Flyweight
{
	private char intrinsicState;
	public ConcreteFlyweight(char intrinsicState)
	{
		this.intrinsicState = intrinsicState;
	}
	public override void Operation(string extrinsicstate)
	{
		Console.WriteLine("ConcreteFlyweight('" + intrinsicState + "'): \"" + extrinsicstate + "\"");
	}
}
public class UnsharedConcreteFlyweight : Flyweight
{
	private IEnumerable<Flyweight> flyweights;
	public UnsharedConcreteFlyweight(IEnumerable<Flyweight> flyweights)
	{
		this.flyweights = flyweights;
	}
	public override void Operation(string extrinsicstate)
	{
		Console.WriteLine("UnsharedConcreteFlyweight: ");
		foreach (var flyweight in flyweights)
		{
			flyweight.Operation(extrinsicstate);
		}
		Console.WriteLine();
	}
}
public class Test
{
	public static void Main()
	{
		string extrinsicstate = "THIS IS A BIG EXTRINSIC STATE";
		FlyweightFactory factory = new FlyweightFactory();
		
		UnsharedConcreteFlyweight flyweights1 = new UnsharedConcreteFlyweight(new Flyweight[]
			{
				factory.GetFlyweight('a'),
				factory.GetFlyweight('a'),
				factory.GetFlyweight('b'),
				factory.GetFlyweight('c'),
				factory.GetFlyweight('a'),
				factory.GetFlyweight('c')
			});
		
		UnsharedConcreteFlyweight flyweights2 = new UnsharedConcreteFlyweight(new Flyweight[]
			{
				factory.GetFlyweight('c'),
				factory.GetFlyweight('b'),
				factory.GetFlyweight('b'),
				factory.GetFlyweight('a'),
				factory.GetFlyweight('c'),
				factory.GetFlyweight('b')
			});
		
		flyweights1.Operation(extrinsicstate);
		flyweights2.Operation(extrinsicstate);
		
		Console.ReadKey();
	}
}
/*
Output
UnsharedConcreteFlyweight:
ConcreteFlyweight('a'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('a'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('b'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('c'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('a'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('c'): "THIS IS A BIG EXTRINSIC STATE"

UnsharedConcreteFlyweight:
ConcreteFlyweight('c'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('b'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('b'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('a'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('c'): "THIS IS A BIG EXTRINSIC STATE"
ConcreteFlyweight('b'): "THIS IS A BIG EXTRINSIC STATE"
*/
