abstract class Colleague
{
	public abstract void Send(string message);
}
class ConcreteColleague1: Colleague
{
	private ConcreteColleague2 colleague;
	
	public void SetColleague(ConcreteColleague2 colleague)
	{
		this.colleague = colleague;
	}
	public override void Send(string message)
	{
		Console.WriteLine("Colleague1: sending '" + message + "' to colleague2");
		colleague.HerReceive(message);
	}
	public void HisReceive(string message)
	{
		Console.WriteLine("Colleague1 received: " + message);
	}
}
class ConcreteColleague2: Colleague
{
	private ConcreteColleague1 colleague;
	
	public void SetColleague(ConcreteColleague1 colleague)
	{
		this.colleague = colleague;
	}
	public override void Send(string message)
	{
		Console.WriteLine("Colleague2: sending '" + message + "' to colleague1");
		colleague.HisReceive(message);
	}
	public void HerReceive(string message)
	{
		Console.WriteLine("Colleague2 received: " + message);
	}
}
class Test
{
	static void Main()
	{
		ConcreteColleague1 c1 = new ConcreteColleague1();
		ConcreteColleague2 c2 = new ConcreteColleague2();

		c1.SetColleague(c2);
		c2.SetColleague(c1);
		
		c1.Send("How are you?");
		c2.Send("Fine, thanks");

		Console.ReadKey();
	}
}
/* OUTPUT:
Colleague1: sending 'How are you?' to colleague2
Colleague2 received: How are you?
Colleague2: sending 'Fine, thanks' to colleague1
Colleague1 received: Fine, thanks
*/