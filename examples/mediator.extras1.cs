abstract class Mediator
{
	protected ConcreteColleague1 colleague1;
	protected ConcreteColleague2 colleague2;
	
	public Mediator(ConcreteColleague1 colleague1, ConcreteColleague2 colleague2)
	{
		this.colleague1 = colleague1;
		this.colleague2 = colleague2;
	}
	public abstract void NotifyChange(string message, Colleague colleague);
}
class ConcreteMediator: Mediator
{
	public ConcreteMediator(ConcreteColleague1 colleague1, ConcreteColleague2 colleague2): base(colleague1, colleague2)
	{ }
	public override void NotifyChange(string message, Colleague colleague)
	{
		if (colleague == colleague1)
		{
			colleague2.HerReceive(message);
		}
		else
		{
			colleague1.HisReceive(message);
		}
	}
}
class ShouterMediator: ConcreteMediator
{
	public ShouterMediator(ConcreteColleague1 colleague1, ConcreteColleague2 colleague2): base(colleague1, colleague2)
	{ }
	public override void NotifyChange(string message, Colleague colleague)
	{
		var msg = message.ToUpper();
		base.NotifyChange(msg, colleague);
	}
}
abstract class Colleague
{
	protected Mediator mediator;
	
	public void SetMediator(Mediator mediator)
	{
		this.mediator = mediator;
	}
	public virtual void Send(string message)
	{
		mediator.NotifyChange(message, this);
	}
}
class ConcreteColleague1: Colleague
{
	public override void Send(string message)
	{
		Console.WriteLine("Colleague1: sending '" + message + "' to colleague2");
		base.Send(message);
	}
	public void HisReceive(string message)
	{
		Console.WriteLine("Colleague1 received: " + message);
	}
}
class ConcreteColleague2: Colleague
{
	public override void Send(string message)
	{
		Console.WriteLine("Colleague2: sending '" + message + "' to colleague1");
		base.Send(message);
	}
	public void HerReceive(string message)
	{
		Console.WriteLine("Colleague2 received: " + message);
	}
}
class MainApp
{
	static void Main()
	{
		var c1 = new ConcreteColleague1();
		var c2 = new ConcreteColleague2();

		var m = new ShouterMediator(c1, c2);
		
		c1.SetMediator(m);
		c2.SetMediator(m);
		
		c1.Send("How are you?");
		c2.Send("Fine, thanks");

		Console.ReadKey();
	}
}
/* OUTPUT:
Colleague1: sending 'How are you?' to colleague2
Colleague2 received: HOW ARE YOU?
Colleague2: sending 'Fine, thanks' to colleague1
Colleague1 received: FINE, THANKS
*/