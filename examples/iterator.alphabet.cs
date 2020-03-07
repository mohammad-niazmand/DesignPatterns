struct AlphabetSetting
{
	public bool Upper { get; set; }
	public char Skip { get; set; }
}
class Alphabet : IEnumerable<char>
{
	public IEnumerable<char> FirstToLast(AlphabetSetting setting)
	{
		var mark = 0;
		for (var i = 0; i < 26; i++)
		{
			if (mark == setting.Skip)
			{
				yield return (char)(((setting.Upper) ? 65 : 97) + i);
				mark = 0;
			}
			else
				mark++;
		}
	}
	public IEnumerable<char> FirstToLast()
	{
		return this.FirstToLast(new AlphabetSetting());
	}
	public IEnumerable<char> LastToFirst(AlphabetSetting setting)
	{
		var mark = 0;
		for (var i = 25; i >= 0; i--)
		{
			if (mark == setting.Skip)
			{
				yield return (char)(((setting.Upper) ? 65 : 97) + i);
				mark = 0;
			}
			else
				mark++;
		}
	}
	public IEnumerable<char> LastToFirst()
	{
		return this.LastToFirst(new AlphabetSetting());
	}
	public IEnumerator<char> GetEnumerator()
	{
		var x = this.FirstToLast();
		return x.GetEnumerator();
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}
class Test
{
	static void Main()
	{
		var alphabet = new Alphabet();
		foreach (var ch in alphabet)
		{
			Console.Write(ch + " ");
		}
		Console.WriteLine();

		foreach (var ch in alphabet.LastToFirst())
		{
			Console.Write(ch + " ");
		}
		Console.WriteLine();

		foreach (var ch in alphabet.FirstToLast(new AlphabetSetting { Upper = true }))
		{
			Console.Write(ch + " ");
		}
		Console.ReadKey();
	}
}
/* OUTPUT:
a b c d e f g h i j k l m n o p q r s t u v w x y z
z y x w v u t s r q p o n m l k j i h g f e d c b a
B D F H J L N P R T V X Z
*/