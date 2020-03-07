public sealed class Singleton
{
    private static Singleton uniqueInstance;
    Singleton( )
    {    }
    public static Singleton Instance()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = new Singleton( );
        }
        return uniqueInstance;
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