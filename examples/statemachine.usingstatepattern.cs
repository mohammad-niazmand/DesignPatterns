abstract class MachineState
{
	public abstract void Start(Machine machine);
	public abstract void Process(Machine machine, string data);
	public abstract void End(Machine machine);
}
class MachineEndState: MachineState
{
	public override void Start(Machine machine)
	{
		Console.WriteLine("State Changed => START");
		Console.WriteLine("Machine started.");
		machine.State = new MachineStartState();
	}
	public override void Process(Machine machine, string data)
	{
		Console.WriteLine("Machine has not started yet!!");
	}
	public override void End(Machine machine)
	{
		Console.WriteLine("Machine is ended already!!");
	}
}
class MachineStartState: MachineState
{
	public override void Start(Machine machine)
	{
		Console.WriteLine("Machine is already started!!");
	}
	public override void Process(Machine machine, string data)
	{
		machine.State = new MachineProcessingState();
		machine.State.Process(machine, data);
	}
	public override void End(Machine machine)
	{
		Console.WriteLine("State Changed => END.");
		Console.WriteLine("Machine ended.");
		machine.State = new MachineEndState();
	}
}
class MachineProcessingState: MachineState
{
	public override void Start(Machine machine)
	{
		Console.WriteLine("Machine is already started!!");
	}
	public override void Process(Machine machine, string data)
	{
		Console.WriteLine("State Changed => PROCESSING.");
		Console.WriteLine("The length of '{0}' is: {1}", data, data.Length);
		Console.WriteLine("data processed successfully.");
	}
	public override void End(Machine machine)
	{
		Console.WriteLine("State Changed => END.");
		Console.WriteLine("Machine ended.");
		machine.State = new MachineEndState();
	}
}
class Machine
{
	public MachineState State { get; set; }
	public Machine()
	{
		State = new MachineEndState();
	}
	public void Start()
	{
		Console.WriteLine();
		State.Start(this);
	}
	public void Process(string data)
	{
		Console.WriteLine();
		State.Process(this, data);
	}
	public void End()
	{
		Console.WriteLine();
		State.End(this);
	}
}
class Test
{
	public static void Main()
	{
		var c = new Machine();
		
		c.Start();
		c.Process("Hello World");
		c.Process("This is a test");
		c.Process("Another test");
		c.End();
		
		Console.ReadKey();
	}
}

/* OUTPUT:
State Changed => START
Machine started.

State Changed => PROCESSING.
The length of 'Hello World' is: 11
data processed successfully.

State Changed => PROCESSING.
The length of 'This is a test' is: 14
data processed successfully.

State Changed => PROCESSING.
The length of 'Another test' is: 12
data processed successfully.

State Changed => END.
Machine ended.
*/