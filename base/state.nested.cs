public abstract class State
{
	public abstract void Handle(Context context);
}
public class Context
{
	public State State { get; private set; }

	// declare concrete state classes here
	public class ConcreteStateA: State
	{
		public override void Handle(Context context)
		{
			// context.State is private, but is accessible and writable for ConcreteStateA.
			context.State = new ConcreteStateB();
		}
	}
	public class ConcreteStateB: State
	{
		public override void Handle(Context context)
		{
			// context.State is private, but is accessible and writable for ConcreteStateB.
			context.State = new ConcreteStateA();
		}
	}
	public Context()
	{
		this.State = new ConcreteStateA();
	}
	public void Request()
	{
		State.Handle(this);
	}
}
