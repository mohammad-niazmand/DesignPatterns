abstract class Subject
{
	protected List<Observer> observers = new List<Observer>();
	
	public virtual void Attach(Observer observer)
	{
		observers.Add(observer);
	}
	public virtual void Detach(Observer observer)
	{
		observers.Remove(observer);
	}
	public virtual void Notify()
	{
		foreach (Observer o in observers)
		{
			o.Update();
		}
	}
}
class ConcreteSubject: Subject
{
	protected int subjectState;
	
	public virtual int GetState()
	{
		return subjectState;
	}
	public virtual void SetState(int value)
	{
		subjectState = value;
		Notify();
	}
}
abstract class Observer
{
	public abstract void Update();
}
class ConcreteObserver: Observer
{
	private ConcreteSubject subject;
	private string name;
	private int observerState;
	private int factor;
	
	public ConcreteObserver(ConcreteSubject subject, string name, int factor)
	{
		this.subject = subject;
		this.name = name;
		this.factor = factor;
	}
	public override void Update()
	{
		observerState = subject.GetState() * factor;
		Console.WriteLine("{0}'s new state is {1}", name, observerState);
	}
}
class Test
{
	static void Main()
	{
		var s = new ConcreteSubject();

		s.Attach(new ConcreteObserver(s, "Observer-X", 10));
		s.Attach(new ConcreteObserver(s, "Observer-Y", 20));
		s.Attach(new ConcreteObserver(s, "Observer-Z", 30));

		s.SetState(12);
		
		Console.ReadKey();
	}
}
/* OUTPUT:
Observer-X's new state is 120
Observer-Y's new state is 240
Observer-Z's new state is 360
*/