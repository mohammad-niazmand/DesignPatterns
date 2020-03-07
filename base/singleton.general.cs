public class Singleton <T> where T : class, new()
{
    private static readonly T uniqueInstance = new T();
    private Singleton()
    { }
    public static T Instance()
    {
        return uniqueInstance;
    }
}
class MyClass
{ }
class HisClass
{ }
class Test
{
    public static void Main ( )
    {
        MyClass mc1 = Singleton<MyClass>.Instance();
        MyClass mc2 = Singleton<MyClass>.Instance();
		
        HisClass hc1 = Singleton<HisClass>.Instance();
        HisClass hc2 = Singleton<HisClass>.Instance();

        if (mc1 == mc2)
            Console.WriteLine("mc1 and mc2 refer to the same instance.");
        else    // this is never executed
            Console.WriteLine("mc1 and mc2 refer to distinct instances.");
		
        if (hc1 == hc2)
            Console.WriteLine("hc1 and hc2 refer to the same instance.");
        else    // this is never executed
            Console.WriteLine("hc1 and hc2 refer to distinct instances.");
		
		Console.ReadKey();
    }
}
/* OUTPUT:
mc1 and mc2 refer to the same instance.
hc1 and hc2 refer to the same instance.
*/