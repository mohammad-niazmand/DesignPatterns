public sealed class Singleton
{
    private static readonly Lazy<Singleton> uniqueInstance = new Lazy<Singleton>(() => new Singleton(), true);
    Singleton( )
    {    }
    public static Singleton Instance()
    {
        return uniqueInstance.Value;
    }
}
class Test
{
	static void Main()
	{
		var s1 = Singleton.Instance();
		var s2 = Singleton.Instance();
		var s3 = Singleton.Instance();
		
		Console.WriteLine("s1 == s2 ? {0}", (s1 == s2));
		Console.WriteLine("s2 == s3 ? {0}", (s2 == s3));
		
		Console.ReadKey();
	}
}
/* OUTPUT:
s1 == s2 ? True
s2 == s3 ? True
*/