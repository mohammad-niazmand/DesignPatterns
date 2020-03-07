using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
	Main Ambiguous Grammer without operator priority
expression  --> exp
exp  --> parenthesis | binary | number | identifier
parenthesis  --> '(' exp ')'
binary  --> exp operator exp
operator  --> '+' | '-' | '*' | '/'
number  --> digit | digit number
identifier  --> alpha | alpha identifier
digit --> '0' | '1' | ... '9'
alpha  --> 'a' | 'b' | ... 'z' | 'A' | 'B' | ... 'Z'

	Non-Ambiguous LL(1) Grammer with operator priority
S  --> E $
E  --> T E'
E' --> '+' TE' | '-' TE' | empty
T  --> FT'
T' --> '*' FT' | '/' FT' | empty
F  --> identifier | number | '(' E ')'
identifier --> alpha | alpha identifier
number --> digit | digit number
digit  --> '0' | '1' | '2' | ... | '9'
alpha  --> 'a' ... 'z' | 'A' ... 'Z'
*/

enum LexicalToken
{ Plus, Minus, Star, Slash, Id, Num, OpenPar, ClosePar, Start, End, Error }
class Lexer
{
    private string sentence;
    private int index;
    private int oldIndex;
    private char oldChar;

    private void Init(string sentence)
    {
        this.sentence = sentence;
        this.Current = LexicalToken.Start;
        this.index = 0;
        this.oldIndex = -1;
        this.oldChar = new Char();
    }
    public string Sentence
    {
        get { return sentence; }
        set
        {
            Init(value);
        }
    }
    public Lexer()
    {
        Init("");
    }
    public Lexer(string sentence)
    {
        Init(sentence);
    }
    public void Reset()
    {
        Init(sentence);
    }
    public LexicalToken Current
    {
        get;
        private set;
    }
    public string Value
    {
        get;
        private set;
    }
    public void Next()
    {
        GetNext();
    }
    public LexicalToken GetNext()
    {
        var state = 1;
        var result = LexicalToken.Error;
        var ch = new Char();
        var done = false;

        while (!done)
        {
            if (oldChar != 0)
            {
                ch = oldChar;
                oldChar = new Char();
            }
            else
            {
                if (this.index < sentence.Length)
                {
                    ch = sentence[this.index];
                    if (this.oldIndex < 0)
                        this.oldIndex = index;
                    this.index++;
                }
                else
                {
                    switch (state)
                    {
                        case 1:
                            result = LexicalToken.End;
                            this.Value = "";
                            break;
                        case 2:
                            result = LexicalToken.Id;
                            this.Value = sentence.Substring(this.oldIndex);
                            this.oldIndex = -1;
                            break;
                        case 3:
                            result = LexicalToken.Num;
                            this.Value = sentence.Substring(this.oldIndex);
                            this.oldIndex = -1;
                            break;
                    }
                    oldChar = new Char();
                    done = true;
                    continue;
                }
            }
            switch (state)
            {
                case 1:
                    if (ch == '+')
                    {
                        result = LexicalToken.Plus;
                        this.Value = "+";
                        this.oldIndex = -1;
                        done = true;
                    }
                    else if (ch == '-')
                    {
                        result = LexicalToken.Minus;
                        this.Value = "-";
                        this.oldIndex = -1;
                        done = true;
                    }
                    else if (ch == '*')
                    {
                        result = LexicalToken.Star;
                        this.Value = "*";
                        this.oldIndex = -1;
                        done = true;
                    }
                    else if (ch == '/')
                    {
                        result = LexicalToken.Slash;
                        this.Value = "/";
                        this.oldIndex = -1;
                        done = true;
                    }
                    else if (ch == '(')
                    {
                        result = LexicalToken.OpenPar;
                        this.Value = "(";
                        this.oldIndex = -1;
                        done = true;
                    }
                    else if (ch == ')')
                    {
                        result = LexicalToken.ClosePar;
                        this.Value = ")";
                        this.oldIndex = -1;
                        done = true;
                    }
                    else if (Char.IsLetter(ch))
                        state = 2;
                    else if (Char.IsDigit(ch))
                        state = 3;
                    else if (Char.IsWhiteSpace(ch))
                    {
                        state = 1;
                        this.oldIndex = -1;
                    }
                    else
                        throw new ApplicationException(String.Format("Lexer error: Invalid character {0} at position {1}", ch, (this.index - 1)));
                    break;
                case 2:	// Id
                    if (Char.IsLetterOrDigit(ch))
                        state = 2;
                    else
                    {
                        result = LexicalToken.Id;
                        oldChar = ch;
                        this.Value = sentence.Substring(this.oldIndex, this.index - this.oldIndex - 1);
                        this.oldIndex = -1;
                        done = true;
                    }
                    break;
                case 3:	// number
                    if (Char.IsDigit(ch))
                        state = 3;
                    else
                    {
                        result = LexicalToken.Num;
                        oldChar = ch;
                        this.Value = sentence.Substring(this.oldIndex, this.index - this.oldIndex - 1);
                        this.oldIndex = -1;
                        done = true;
                    }
                    break;
            }
        }

        this.Current = result;

        return result;
    }
    public void Expect(LexicalToken token)
    {
        if (this.Current == token)
            Next();
        else
            throw new ApplicationException(String.Format("Lexer error: Incorrent token '{0}' encountered. Expected '{1}'.", this.Value, token.ToString()));
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
    public static BinaryExpression Binary(double left, OperatorExpression op, Expression right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(Expression left, OperatorExpression op, double right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(double left, OperatorExpression op, double right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(Expression left, char op, Expression right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(double left, char op, Expression right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(Expression left, char op, double right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(double left, char op, double right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(string left, OperatorExpression op, Expression right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(Expression left, OperatorExpression op, string right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(string left, OperatorExpression op, string right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(string left, char op, Expression right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(Expression left, char op, string right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static BinaryExpression Binary(string left, char op, string right)
    {
        return new BinaryExpression(left, op, right);
    }
    public static NumberExpression Number(double value)
    {
        return new NumberExpression(value);
    }
    public static NumberExpression Number(string value)
    {
        return new NumberExpression(value);
    }
    public static IdentifierExpression Identifier(string name, double value)
    {
        return new IdentifierExpression(name, value);
    }
    public static IdentifierExpression Identifier(string name, string value)
    {
        return new IdentifierExpression(value);
    }
    public static IdentifierExpression Identifier(string name)
    {
        return new IdentifierExpression(name, 0);
    }
    public static OperatorExpression Operator(char op)
    {
        return new OperatorExpression(op);
    }
}
class ParenthesisExpression : Expression
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
class BinaryExpression : Expression
{
    public Expression Left { get; private set; }
    public OperatorExpression op { get; private set; }
    public Expression Right { get; private set; }

    private void Init(Expression left, OperatorExpression op, Expression right)
    {
        this.Left = left;
        this.op = op;
        this.Right = right;
    }
    public BinaryExpression(Expression left, OperatorExpression op, Expression right)
    {
        Init(left, op, right);
    }
    public BinaryExpression(double left, OperatorExpression op, Expression right)
    {
        Init(Expression.Number(left), op, right);
    }
    public BinaryExpression(Expression left, OperatorExpression op, double right)
    {
        Init(left, op, Expression.Number(right));
    }
    public BinaryExpression(double left, OperatorExpression op, double right)
    {
        Init(Expression.Number(left), op, Expression.Number(right));
    }
    public BinaryExpression(Expression left, char op, Expression right)
    {
        Init(left, Expression.Operator(op), right);
    }
    public BinaryExpression(double left, char op, Expression right)
    {
        Init(Expression.Number(left), Expression.Operator(op), right);
    }
    public BinaryExpression(Expression left, char op, double right)
    {
        Init(left, Expression.Operator(op), Expression.Number(right));
    }
    public BinaryExpression(double left, char op, double right)
    {
        Init(Expression.Number(left), Expression.Operator(op), Expression.Number(right));
    }
    public BinaryExpression(string left, OperatorExpression op, Expression right)
    {
        int temp;

        if (Int32.TryParse(left, out temp))
            Init(Expression.Number(temp), op, right);
        else
            Init(Expression.Identifier(left), op, right);
    }
    public BinaryExpression(Expression left, OperatorExpression op, string right)
    {
        int temp;

        if (Int32.TryParse(right, out temp))
            Init(left, op, Expression.Number(temp));
        else
            Init(left, op, Expression.Identifier(right));
    }
    public BinaryExpression(string left, OperatorExpression op, string right)
    {
        int temp1;

        if (Int32.TryParse(left, out temp1))
        {
            int temp2;

            if (Int32.TryParse(right, out temp2))
                Init(Expression.Number(temp1), op, Expression.Number(temp2));
            else
                Init(Expression.Number(temp1), op, Expression.Identifier(right));
        }
        else
        {
            int temp2;

            if (Int32.TryParse(right, out temp2))
                Init(Expression.Identifier(left), op, Expression.Number(temp2));
            else
                Init(Expression.Identifier(left), op, Expression.Identifier(right));
        }
    }
    public BinaryExpression(string left, char op, Expression right)
    {
        int temp;

        if (Int32.TryParse(left, out temp))
            Init(Expression.Number(temp), Expression.Operator(op), right);
        else
            Init(Expression.Identifier(left), Expression.Operator(op), right);
    }
    public BinaryExpression(Expression left, char op, string right)
    {
        int temp;

        if (Int32.TryParse(right, out temp))
            Init(left, Expression.Operator(op), Expression.Number(temp));
        else
            Init(left, Expression.Operator(op), Expression.Identifier(right));
    }
    public BinaryExpression(string left, char op, string right)
    {
        int temp1;

        if (Int32.TryParse(left, out temp1))
        {
            int temp2;

            if (Int32.TryParse(right, out temp2))
                Init(Expression.Number(temp1), Expression.Operator(op), Expression.Number(temp2));
            else
                Init(Expression.Number(temp1), Expression.Operator(op), Expression.Identifier(right));
        }
        else
        {
            int temp2;

            if (Int32.TryParse(right, out temp2))
                Init(Expression.Identifier(left), Expression.Operator(op), Expression.Number(temp2));
            else
                Init(Expression.Identifier(left), Expression.Operator(op), Expression.Identifier(right));
        }
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
class OperatorExpression : Expression
{
    public char Value { get; private set; }
    public OperatorExpression(char value)
    {
        if (value == '+' | value == '-' | value == '*' | value == '/')
            Value = value;
        else
            throw new ApplicationException("Invalid operator");
    }
    public static OperatorExpression FromToken(LexicalToken token)
    {
        if (token == LexicalToken.Plus)
            return new OperatorExpression('+');
        else if (token == LexicalToken.Minus)
            return new OperatorExpression('-');
        else if (token == LexicalToken.Star)
            return new OperatorExpression('*');
        else if (token == LexicalToken.Slash)
            return new OperatorExpression('/');
        else
            throw new ApplicationException("Invalid operator");
    }
    public override void Interpret(Context context)
    { }
}
class IdentifierExpression : Expression
{
    public double Value { get; set; }
    public string Name { get; private set; }

    private void Init(string name, double value)
    {
        if (!Regex.IsMatch(name, @"[a-zA-Z]\w*"))
            throw new ApplicationException("Invalid identifier");
        Name = name;
        Value = value;
    }
    public IdentifierExpression(string name, double value)
    {
        Init(name, value);
    }
    public IdentifierExpression(string name, string value)
    {
        Init(name, double.Parse(value));
    }
    public IdentifierExpression(string name)
    {
        Init(name, 0);
    }
    public override void Interpret(Context context)
    {
        context.Value = context.Variables[this.Name];
        this.Value = context.Value;
    }
}
class NumberExpression : Expression
{
    public double Value { get; set; }
    public NumberExpression(double value)
    {
        Value = value;
    }
    public NumberExpression(string value)
    {
        Value = double.Parse(value);
    }
    public override void Interpret(Context context)
    {
        context.Value = this.Value;
    }
}
class Parser
{
    private Lexer lexer;
    public Dictionary<string, double> Variables { get; private set; }

    public Parser()
    {
        lexer = new Lexer();
        Variables = new Dictionary<string, double>();
    }
    private void AddVariable(string name)
    {
        if (!Variables.ContainsKey(name))
            Variables.Add(name, 0);
    }
    public Expression Parse(string sentence)
    {
        Variables.Clear();
        lexer.Sentence = sentence;
        lexer.Next();

        return S();
    }
    private Expression S()
    {
        var e2 = E();
        return e2;
    }
    private Expression E()
    {
        var e2 = T();
        var e3 = E1(e2);
        return e3;
    }
    private Expression E1(Expression e1)
    {
        if (lexer.Current == LexicalToken.Plus || lexer.Current == LexicalToken.Minus)
        {
            var op = OperatorExpression.FromToken(lexer.Current);
            lexer.Next();
            var e2 = T();
            var e3 = Expression.Binary(e1, op, e2);
            var e4 = E1(e3);

            return e4;
        }
        else
            return e1;
    }
    private Expression T()
    {
        var e2 = F();
        var e3 = T1(e2);

        return e3;
    }
    private Expression T1(Expression e1)
    {
        if (lexer.Current == LexicalToken.Star || lexer.Current == LexicalToken.Slash)
        {
            var op = OperatorExpression.FromToken(lexer.Current);
            lexer.Next();
            var e2 = F();
            var e3 = Expression.Binary(e1, op, e2);
            var e4 = T1(e3);

            return e4;
        }
        else
            return e1;
    }
    private Expression F()
    {
        if (lexer.Current == LexicalToken.Id)
        {
            var result = Expression.Identifier(lexer.Value);
            AddVariable(lexer.Value);
            lexer.Next();

            return result;
        }
        else if (lexer.Current == LexicalToken.Num)
        {
            var result = Expression.Number(lexer.Value);
            lexer.Next();

            return result;
        }
        else if (lexer.Current == LexicalToken.OpenPar)
        {
            lexer.Next();
            var e = E();
            lexer.Expect(LexicalToken.ClosePar);
            return Expression.Parenthesis(e);
        }
        else
            throw new ApplicationException(String.Format("Parse error: Incorrent token '{0}' encountered. Expected ')'.", lexer.Value));
    }
}
class Test
{
    static void testLexer()
    {
        var lexer = new Lexer();
        lexer.Sentence = "a + b * 4 - 1";

        while (lexer.GetNext() != LexicalToken.End)
        {
            Console.WriteLine("{0}\t{1}", lexer.Current, lexer.Value);
        }
    }
    static void testParser()
    {
        var parser = new Parser();
        try
        {
            var e = parser.Parse("a + 2 * (b - 1)");
            Console.WriteLine("Parse successfull");

            var context = new Context();

            context.MergeVariables(parser.Variables.Keys);
            context.Variables["a"] = 4;
            context.Variables["b"] = 3;

            e.Interpret(context);

            Console.WriteLine(context.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    static void Main()
    {
        testParser();
        //testLexer();
        Console.ReadKey();
    }
}