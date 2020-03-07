public abstract class Target
{
    public abstract void Request();
}
public class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("Adaptee.SpecificRequest()");
    }
}
public class Adapter: Target
{
    private Adaptee adaptee = new Adaptee();
    public override void Request()
    {
        Console.WriteLine("Adapter.Request()");
        Console.WriteLine("redirecting the request " +
                                           "to the adaptee ...");
       adaptee.SpecificRequest();
    }
}
class Test
{
    public static void Main()
    {
        Target target = new Adapter();
        target.Request();
		
		Console.ReadKey();
    }
}
/* OUTPUT:
Adapter.Request()
redirecting the request to the adaptee ...
Adaptee.SpecificRequest()
*/
