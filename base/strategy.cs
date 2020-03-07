abstract class Strategy
{
	public abstract void AlgorithmInterface();
}
class ConcreteStrategyA : Strategy
{
	public override void AlgorithmInterface()
	{
		Console.WriteLine("ConcreteStrategyA.AlgorithmInterface()");
	}
}
class ConcreteStrategyB : Strategy
{
	public override void AlgorithmInterface()
	{
		Console.WriteLine("ConcreteStrategyB.AlgorithmInterface()");
	}
}
class ConcreteStrategyC : Strategy
{
	public override void AlgorithmInterface()
	{
		Console.WriteLine("ConcreteStrategyC.AlgorithmInterface()");
	}
}
class Context
{
	private Strategy strategry;

	public Context(Strategy strategy)
	{
		this.strategry = strategy;
	}
	public void ContextInterface()
	{
		strategry.AlgorithmInterface();
	}
}
class Test
{
	static void Main()
	{
		Context context;

		context = new Context(new ConcreteStrategyA());
		context.ContextInterface();
		
		context = new Context(new ConcreteStrategyB());
		context.ContextInterface();
		
		context = new Context(new ConcreteStrategyC());
		context.ContextInterface();

		Console.ReadKey();
	}
}
/* OUTPUT:
ConcreteStrategyA.AlgorithmInterface()
ConcreteStrategyB.AlgorithmInterface()
ConcreteStrategyC.AlgorithmInterface()
*/