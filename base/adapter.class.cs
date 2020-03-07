public interface ITarget
{
    void Request();
}
public class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("Adaptee.SpecificRequest()");
    }
}
public class Adapter: Adaptee, ITarget
{
    public virtual void Request()
    {
        Console.WriteLine("Adapter.Request()");
        Console.WriteLine("redirecting the request " +
                                           "to the adaptee ...");
        SpecificRequest();
    }
}
class Test
{
    public static void Main()
    {
        ITarget target = new Adapter();
        target.Request();
		
		Console.ReadKey();
    }
}
/* OUTPUT:
Adapter.Request()
redirecting the request to the adaptee ...
Adaptee.SpecificRequest()
*/
