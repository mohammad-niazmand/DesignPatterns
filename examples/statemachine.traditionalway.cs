enum MachineState { Start, Processing, End }
class Machine
{
	public MachineState State { get; set; }
	public Machine()
	{
		State = MachineState.End;
	}
	public void Start()
	{
		Console.WriteLine();
		if (this.State == MachineState.End)
		{
			Console.WriteLine("State Changed => START");
			Console.WriteLine("Machine started.");
			
			this.State = MachineState.Start;
		}
		else
		{
			Console.WriteLine("Machine is already started!!");
		}
	}
	public void Process(string data)
	{
		Console.WriteLine();
		if (this.State == MachineState.End)
		{
			Console.WriteLine("Machine has not started yet!!");
		}
		else
		{
			Console.WriteLine("State Changed => PROCESSING.");
			Console.WriteLine("The length of '{0}' is: {1}", data, data.Length);
			Console.WriteLine("data processed successfully.");
			
			this.State = MachineState.Processing;
		}
	}
	public void End()
	{
		Console.WriteLine();
		if (this.State != MachineState.End)
		{
			Console.WriteLine("State Changed => END.");
			Console.WriteLine("Machine ended.");
			
			this.State = MachineState.End;
		}
		else
		{
			Console.WriteLine("Machine is ended already!!");
		}
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