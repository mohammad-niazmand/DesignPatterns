/*
expression ::= exp
exp ::= parenthesis | binary | number
parenthesis ::= '(' exp ')'
binary ::= exp Operator exp
Operator ::= '+' | '-' | '*' | '/'
digit = '0' | '1' | ... '9'
number ::= digit | digit number
*/
abstract class Expression
{
	public abstract void Interpret(Context context);
	public static ParenthesisExpression Parenthesis(Expression expression)
	{
		return new ParenthesisExpression(expression);
	}
	public static BinaryExpression Binary(Expression left, OperatorExpression op, Expression right)
	{
		return new BinaryExpression(left, op, right);
	}
	public static NumberExpression Number(double value)
	{
		return new NumberExpression(value);
	}
	public static IdentifierExpression Identifier(string name)
	{
		return new IdentifierExpression(name);
	}
	public static OperatorExpression Operator(char op)
	{
		return new OperatorExpression(op);
	}
}
class ParenthesisExpression: Expression
{
	private Expression expression;

	public ParenthesisExpression(Expression expression)
	{
		this.expression = expression;
	}
	public override void Interpret(Context context)
	{
		expression.Interpret(context);
	}
}
class BinaryExpression: Expression
{
	public Expression Left { get; private set; }
	public OperatorExpression op { get; private set; }
	public Expression Right { get; private set; }

	public BinaryExpression(Expression left, OperatorExpression op, Expression right)
	{
		this.Left = left;
		this.op = op;
		this.Right = right;
	}
	public override void Interpret(Context context)
	{
		var tmp1 = context.Clone();
		var tmp2 = context.Clone();

		Left.Interpret(tmp1);
		Right.Interpret(tmp2);

		switch (op.Value)
		{
			case '+': context.Value = tmp1.Value + tmp2.Value; break;
			case '-': context.Value = tmp1.Value - tmp2.Value; break;
			case '*': context.Value = tmp1.Value * tmp2.Value; break;
			case '/': context.Value = tmp1.Value / tmp2.Value; break;
			default: throw new ApplicationException("Invalid operator");
		}
	}
}
class OperatorExpression: Expression
{
	public char Value { get; private set; }
	public OperatorExpression(char value)
	{
		if (value == '+' | value == '-' | value == '*' | value == '/')
			Value = value;
		else
			throw new ApplicationException("Invalid operator");
	}
	public override void Interpret(Context context)
	{ }
}
class IdentifierExpression: Expression
{
	public double Value { get; set; }
	public string Name { get; private set; }
	
	public IdentifierExpression(string name)
	{
		if (!Regex.IsMatch(name, @"[a-zA-Z]\w*"))
			throw new ApplicationException("Invalid identifier");
		Name = name;
	}
	public override void Interpret(Context context)
	{
		context.Value = context.Variables[this.Name];
		this.Value = context.Value;
	}
}
class NumberExpression: Expression
{
	public double Value { get; set; }
	public NumberExpression(double value)
	{
		Value = value;
	}
	public override void Interpret(Context context)
	{
		context.Value = this.Value;
	}
}
class Context
{
	public double Value { get; set; }
	public Dictionary<string, double> Variables { get; private set; }
	
	public Context()
	{
		Variables = new Dictionary<string, double>();
	}
	public Context Clone()
    {
        return (Context)this.MemberwiseClone();
    }
	public void MergeVariables(IEnumerable<string> variables, bool clearExisting = false)
	{
		foreach (var variable in variables)
		{
			if (!Variables.ContainsKey(variable))
			{
				Variables.Add(variable, 0);
			}
			else if (clearExisting)
			{
				Variables[variable] = 0;
			}
		}
	}
}
class Test
{
	static void Main()
	{
		// create abstract syntax tree for
		// the expression: a + b * 4 - 1
		
		var exp1 = Expression.Binary(Expression.Identifier("b"), Expression.Operator('*'), Expression.Number(4));
		var exp2 = Expression.Binary(Expression.Identifier("a"), Expression.Operator('+'), exp1);
		var exp3 = Expression.Binary(exp2, Expression.Operator('-'), Expression.Number(1));
		
		var context = new Context();
		context.Variables.Add("a", 2);
		context.Variables.Add("b", 3);
		
		exp3.Interpret(context);
		
		Console.WriteLine(context.Value);	// writes 13
		
		Console.ReadKey();
	}
}