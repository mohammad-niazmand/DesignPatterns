using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

abstract class MathCommand
{
	protected AbstractBasicMath math;
	protected double operand;
	protected double currentValue;
	public abstract void Execute();
    public abstract void Unexecute();
    public abstract string Info();
	public MathCommand(AbstractBasicMath math, double operand)
	{
		this.math = math;
		this.operand = operand;
	}
	public MathCommand(AbstractBasicMath math)
	{
		this.math = math;
	}
}
class StartByCommand: MathCommand
{
	public StartByCommand(AbstractBasicMath math, double operand): base(math, operand)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.StartBy(operand);
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("startby {0}", operand);
    }
}
class AddCommand: MathCommand
{
	public AddCommand(AbstractBasicMath math, double operand): base(math, operand)
	{ }
	public override void Execute()
	{
		math.Add(operand);
	}
	public override void Unexecute()
	{
		math.Sub(operand);
	}
    public override string Info()
    {
        return String.Format("add {0}", operand);
    }
}
class SubCommand: MathCommand
{
	public SubCommand(AbstractBasicMath math, double operand): base(math, operand)
	{ }
	public override void Execute()
	{
		math.Sub(operand);
	}
	public override void Unexecute()
	{
		math.Add(operand);
	}
    public override string Info()
    {
        return String.Format("subtract {0}", operand);
    }
}
class MultCommand: MathCommand
{
	public MultCommand(AbstractBasicMath math, double operand): base(math, operand)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.Mult(operand);
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("multiply by {0}", operand);
    }
}
class DivCommand: MathCommand
{
	public DivCommand(AbstractBasicMath math, double operand): base(math, operand)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.Div(operand);
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("divide by {0}", operand);
    }
}
class SqrCommand: MathCommand
{
	public SqrCommand(AbstractBasicMath math): base(math)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.Sqr();
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("make square");
    }
}
class SqrtCommand: MathCommand
{
	public SqrtCommand(AbstractBasicMath math): base(math)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.Sqrt();
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("make square root");
    }
}
class LogCommand: MathCommand
{
	public LogCommand(AbstractBasicMath math): base(math)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.Log();
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("calculate logarithm 2");
    }
}
class Log10Command: MathCommand
{
	public Log10Command(AbstractBasicMath math): base(math)
	{ }
	public override void Execute()
	{
		currentValue = math.CurrentValue;
		math.Log10();
	}
	public override void Unexecute()
	{
		math.StartBy(currentValue);
	}
    public override string Info()
    {
        return String.Format("calculate logarithm 10");
    }
}
abstract class AbstractBasicMath
{
	public double CurrentValue { get; private set; }
	public virtual void StartBy(double value)
	{
		this.CurrentValue = value;
	}
	public virtual void Add(double operand)
	{
		this.CurrentValue += operand;
	}
	public virtual void Sub(double operand)
	{
		this.CurrentValue -= operand;
	}
	public virtual void Mult(double operand)
	{
		this.CurrentValue *= operand;
	}
	public virtual void Div(double operand)
	{
		this.CurrentValue /= operand;
	}
	public virtual void Sqr()
	{
		this.CurrentValue *= this.CurrentValue;
	}
	public virtual void Sqrt()
	{
		this.CurrentValue = Math.Sqrt(this.CurrentValue);
	}
	public virtual void Log()
	{
		this.CurrentValue = Math.Log(this.CurrentValue);
	}
	public virtual void Log10()
	{
		this.CurrentValue = Math.Log10(this.CurrentValue);
	}
}
class BasicMath: AbstractBasicMath
{ }
interface IMathLogger
{
	int Level { get; }
	int UndoLevels { get; }
	int RedoLevels { get; }
	void Append(MathCommand command);
	void ShowUndos();
	void ShowRedos();
	void Undo();
	void UndoAll();
	void UndoTo(int level);
	void Redo();
	void RedoAll();
	void RedoTo(int level);
}
class MathLogger: IMathLogger
{
	private List<MathCommand> commands;
	public int Level { get; private set; }
	public int UndoLevels
	{
		get { return Level; }
	}
	public int RedoLevels
	{
        get { return commands.Count; }
	}
	public MathLogger()
	{
		commands = new List<MathCommand>();
		Level = 0;
	}
	public void Append(MathCommand command)
	{
        if (Level < commands.Count)
        {
            commands[Level] = command;
            while (commands.Count > Level + 1)
            {
                commands.RemoveAt(Level + 1);
            }
        }
        else
            commands.Add(command);
		Level++;
	}
	public void Undo()
	{
		if (Level > 0)
		{
			commands[Level - 1].Unexecute();
			Level--;
		}
	}
	public void UndoAll()
	{
		for(var i = Level; i > 0; i--)
		{
			commands[i - 1].Unexecute();
		}
		Level = 0;
	}
	public void UndoTo(int level)
	{
		for(var i = Level; i > level; i--)
		{
			commands[i - 1].Unexecute();
		}
		Level = level;
	}
	public void Redo()
	{
		if (Level < commands.Count)
		{
			commands[Level].Execute();
			Level++;
		}
	}
	public void RedoAll()
	{
		for(var i = Level; i < commands.Count; i++)
		{
			commands[i].Execute();
		}
		Level = commands.Count;
	}
	public void RedoTo(int level)
	{
		for(var i = Level; i < level; i++)
		{
			commands[i].Execute();
		}
		Level = level;
	}
    public void ShowUndos()
    {
        for (int i = Level; i > 0; i--)
        {
            Console.WriteLine("{0}: {1}", i, commands[i - 1].Info());
        }
    }
    public void ShowRedos()
    {
        for (int i = Level; i < commands.Count; i++)
        {
            Console.WriteLine("{0}: {1}", i, commands[i].Info());
        }
    }
}
interface IMathInvoker
{
	AbstractBasicMath Math { get; }
	IMathLogger Logger { get; }
	double CurrentValue { get; }
    void StartBy(double operand);
	void Add(double operand);
	void Sub(double operand);
	void Mult(double operand);
	void Div(double operand);
	void Sqr();
	void Sqrt();
	void Log();
	void Log10();
}
class MathInvoker: IMathInvoker
{
	private AbstractBasicMath math;
	private IMathLogger logger;
	public AbstractBasicMath Math
	{
		get { return math; }
	}
	public IMathLogger Logger
	{
		get { return logger; }
	}
	public MathInvoker(AbstractBasicMath math, IMathLogger logger)
	{
		this.math = math;
		this.logger = logger;
	}
	public double CurrentValue
	{
		get { return math.CurrentValue; }
	}
	private void Calculate(MathCommand command)
	{
		logger.Append(command);
		command.Execute();
	}
	public virtual void StartBy(double operand)
	{
		var cmd = new StartByCommand(math, operand);
		Calculate(cmd);
	}
	public virtual void Add(double operand)
	{
		var cmd = new AddCommand(math, operand);
		Calculate(cmd);
	}
	public virtual void Sub(double operand)
	{
		var cmd = new SubCommand(math, operand);
		Calculate(cmd);
	}
	public virtual void Mult(double operand)
	{
		var cmd = new MultCommand(math, operand);
		Calculate(cmd);
	}
	public virtual void Div(double operand)
	{
		var cmd = new DivCommand(math, operand);
		Calculate(cmd);
	}
	public virtual void Sqr()
	{
		var cmd = new SqrCommand(math);
		Calculate(cmd);
	}
	public virtual void Sqrt()
	{
		var cmd = new SqrtCommand(math);
		Calculate(cmd);
	}
	public virtual void Log()
	{
		var cmd = new LogCommand(math);
		Calculate(cmd);
	}
	public virtual void Log10()
	{
		var cmd = new Log10Command(math);
		Calculate(cmd);
	}
}
interface IConsoleMath
{
	void Run();
}
class ConsoleCommand
{
	public string Line { get; private set; }
	public string Command { get; private set; }
	public string Argument { get; private set; }
	public double Operand { get; private set; }
	public bool HasNumberArgument { get; private set; }
	
	public ConsoleCommand(string line)
	{
		this.Line = line;
		var parts = line.Split(' ');
		this.Command = parts[0].Trim();
		this.Argument = "";
        if (parts.Length > 1)
        {
            for (var i = 1; i < parts.Length; i++)
            {
                if (parts[i].Trim() != "")
                {
                    this.Argument = parts[i].Trim();
                    var operand = 0.0;
                    this.HasNumberArgument = double.TryParse(this.Argument, out operand);
                    this.Operand = operand;
                    break;
                }
            }
        }
	}
}
class ConsoleMath: IConsoleMath
{
	private string about;
	private string prompt;
	private IMathInvoker invoker;
	public ConsoleMath(IMathInvoker invoker)
	{
        this.about = new String('-', 60);
		this.about += "\n\tSimple Console Math Operations version 1.0.0";
		this.about += "\n\t\tWritten by S.Mansoor Omrani";
		this.about += "\n\t\t\t\t2013-2014\n";
        this.about += new String('-', 60);
        this.about += "\n\nStart by 'startby' command. Type 'help' or '?' to see the list of commands.\n";
		this.prompt = "\n>";
		this.invoker = invoker;
	}
    private bool Add(ConsoleCommand cc)
    {
        var result = false;

        if (cc.HasNumberArgument)
        {
            invoker.Add(cc.Operand);
            result = true;
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: add 'num'");
            Console.WriteLine("\twhere 'num' can be a floating-point or integer number");
            Console.WriteLine("example: add 23");
            Console.WriteLine("example: add 4.5");
        }
        else
            Console.WriteLine("Invalid argument");
        return result;
    }
    private bool StartBy(ConsoleCommand cc)
    {
        var result = false;
        if (cc.HasNumberArgument)
        {
            invoker.StartBy(cc.Operand);
            result = true;
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: startby 'num'");
            Console.WriteLine("\twhere 'num' can be a floating-point or integer number");
            Console.WriteLine("example: startby 5");
            Console.WriteLine("example: startby 2.34");
        }
        else
            Console.WriteLine("Please specify a number.");
        return result;
    }
    private bool Sub(ConsoleCommand cc)
    {
        var result = false;
        if (cc.HasNumberArgument)
        {
            invoker.Sub(cc.Operand);
            result = true;
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: sub 'num'");
            Console.WriteLine("\twhere 'num' can be a floating-point or integer number");
            Console.WriteLine("example: sub 23");
            Console.WriteLine("example: sub 4.5");
        }
        else
            Console.WriteLine("Invalid argument");
        return result;
    }
    private bool Mult(ConsoleCommand cc)
    {
        var result = false;
        if (cc.HasNumberArgument)
        {
            invoker.Mult(cc.Operand);
            result = true;
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: mult 'num'");
            Console.WriteLine("\twhere 'num' can be a floating-point or integer number");
            Console.WriteLine("example: mult 5");
            Console.WriteLine("example: mult 2.34");
        }
        else
            Console.WriteLine("Invalid argument");
        return result;
    }
    private bool Div(ConsoleCommand cc)
    {
        var result = false;
        if (cc.HasNumberArgument)
        {
            if (cc.Operand == 0)
                Console.WriteLine("Division by Zero!");
            else
            {
                invoker.Div(cc.Operand);
                result = true;
            }
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: div 'num'");
            Console.WriteLine("\twhere 'num' can be a floating-point or integer number");
            Console.WriteLine("example: div 4");
            Console.WriteLine("example: add 3.42");
        }
        else
            Console.WriteLine("Invalid argument");
        return result;
    }
    private bool Sqr(ConsoleCommand cc)
    {
        var result = false;
        if (String.IsNullOrEmpty(cc.Argument))
        {
            invoker.Sqr();
            result = true;
        }
        else if (cc.Argument == "/?")
            Console.WriteLine("Syntax: sqr");
        return result;
    }
    private bool Sqrt(ConsoleCommand cc)
    {
        var result = false;
        if (String.IsNullOrEmpty(cc.Argument))
        {
            invoker.Sqrt();
            result = true;
        }
        else if (cc.Argument == "/?")
            Console.WriteLine("Syntax: sqrt");
        return result;
    }
    private bool Log(ConsoleCommand cc)
    {
        var result = false;
        if (String.IsNullOrEmpty(cc.Argument))
        {
            invoker.Log();
            result = true;
        }
        else if (cc.Argument == "/?")
            Console.WriteLine("Syntax: log");
        return result;
    }
    private bool Log10(ConsoleCommand cc)
    {
        var result = false;
        if (String.IsNullOrEmpty(cc.Argument))
        {
            invoker.Log10();
            result = true;
        }
        else if (cc.Argument == "/?")
            Console.WriteLine("Syntax: log10");
        return result;
    }
    private bool Show(ConsoleCommand cc)
    {
        var result = false;
        if (cc.Argument == "undos")
            invoker.Logger.ShowUndos();
        else if (cc.Argument == "redos")
            invoker.Logger.ShowRedos();
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: show {undos | redos}");
            Console.WriteLine("\ta single 'show' command without any argument shows current value");
        }
        else if (String.IsNullOrEmpty(cc.Argument))
            result = true;
        else
            Console.WriteLine("Invalid argument");
        return result;
    }
    private bool Undo(ConsoleCommand cc)
    {
        var result = false;
        if (String.IsNullOrEmpty(cc.Argument))
        {
            invoker.Logger.Undo();
            result = true;
        }
        else if (cc.Argument == "all")
        {
            invoker.Logger.UndoAll();
            result = true;
        }
        else if (cc.HasNumberArgument)
        {
            int level = 0;
            if (Int32.TryParse(cc.Operand.ToString(), out level))
            {
                if (level >= 0)
                {
                    if (level <= invoker.Logger.UndoLevels)
                    {
                        invoker.Logger.UndoTo(level);
                        result = true;
                    }
                    else
                        Console.WriteLine("Invalid level. Level must be between 0 and {0}", invoker.Logger.UndoLevels);
                }
                else
                    Console.WriteLine("Invalid level. Level must be between 0 and {0}", invoker.Logger.UndoLevels);
            }
            else
                Console.WriteLine("Invalid level");
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: undo ['level' | all]");
            Console.WriteLine("\twhere 'level' is an undo level to which the operations must be undone.");
            Console.WriteLine("\twhere 'undo' performs a single undo level");
            Console.WriteLine("\t'undo all' undos all the operations");
        }
        return result;
    }
    private bool Redo(ConsoleCommand cc)
    {
        var result = false;
        if (String.IsNullOrEmpty(cc.Argument))
        {
            invoker.Logger.Redo();
            result = true;
        }
        else if (cc.Argument == "all")
        {
            invoker.Logger.RedoAll();
            result = true;
        }
        else if (cc.HasNumberArgument)
        {
            int level = 0;
            if (Int32.TryParse(cc.Operand.ToString(), out level))
            {
                if (level >= 0)
                {
                    if (level <= invoker.Logger.RedoLevels)
                    {
                        invoker.Logger.RedoTo(level);
                        result = true;
                    }
                    else
                        Console.WriteLine("Invalid level. Level must be between 0 and {0}", invoker.Logger.RedoLevels);
                }
                else
                    Console.WriteLine("Invalid level. Level must be between 0 and {0}", invoker.Logger.RedoLevels);
            }
            else
                Console.WriteLine("Invalid level");
        }
        else if (cc.Argument == "/?")
        {
            Console.WriteLine("Syntax: redo ['level' | all]");
            Console.WriteLine("\twhere 'level' is a redo level to which the operations must be repeated.");
            Console.WriteLine("\twhere 'redo' performs a single redo level");
            Console.WriteLine("\t'redo all' redos all the operations");
        }
        return result;
    }
    private void Help()
    {
        var commands = "startby\tadd\tsub\tmult\tdiv\tsqr\tsqrt\tlog\tlog10\tshow\tclear\tundo\tredo\tundoall\tredoall\tundoto\tredoto\tcls\thelp\texit";

        Console.WriteLine(commands);
    }
	public void Run()
	{
		Console.WriteLine(about);
		
		while (true)
		{
			Console.Write(prompt);
			var line = Console.ReadLine().ToLower().Trim();
			if (!String.IsNullOrEmpty(line))
			{
				var cc = new ConsoleCommand(line);
				var showValue = false;
				
				switch(cc.Command)
				{
                    case "startby":
                        showValue = StartBy(cc);
                        break;
                    case "clear":
                        showValue = StartBy(new ConsoleCommand("startby 0"));
                        break;
					case "add":
                        showValue = Add(cc);
						break;
					case "sub":
                        showValue = Sub(cc);
						break;
					case "mult":
                        showValue = Mult(cc);
						break;
					case "div":
                        showValue = Div(cc);
						break;
					case "sqr":
                        showValue = Sqr(cc);
						break;
					case "sqrt":
                        showValue = Sqrt(cc);
						break;
					case "log":
                        showValue = Log(cc);
						break;
					case "log10":
                        showValue = Log10(cc);
						break;
					case "help":
                        Help();
						break;
                    case "?":
                        Help();
                        break;
					case "show":
                        showValue = Show(cc);
						break;
					case "undo":
                        showValue = Undo(cc);
						break;
					case "redo":
                        showValue = Redo(cc);
						break;
                    case "undoall":
                        showValue = Undo(new ConsoleCommand("undo all"));
                        break;
                    case "redoall":
                        showValue = Redo(new ConsoleCommand("redo all"));
                        break;
                    case "undoto":
                        showValue = Undo(new ConsoleCommand("undo " + cc.Argument));
                        break;
                    case "redoto":
                        showValue = Redo(new ConsoleCommand("redo " + cc.Argument));
                        break;
                    case "cls":
                        Console.Clear();
                        break;
					case "exit":
						Console.WriteLine("Thank you for using this program.");
						goto Finish;
					default:
						Console.WriteLine("Invalid command. type 'help' to get the list of commands.");
                        break;
				}
				if (showValue)
				{
					Console.WriteLine("Current Value = {0}", invoker.CurrentValue);
				}
			}
		}
    Finish:
        return;
	}
}
class Client
{
	static void Main()
	{
        var bm = new BasicMath();
        var ml = new MathLogger();
        var mi = new MathInvoker(bm, ml);
        var cm = new ConsoleMath(mi);

        cm.Run();
        
        //Console.ReadKey();
	}
}