public abstract class Subject
{
    public abstract void Request();
}
public class RealSubject: Subject
{
    public override void Request()
    {
        Console.WriteLine("RealSubject.Request()");
    }
}
public class SubjectProxy: Subject
{
    private RealSubject realSubject;
    public override void Request()
    {
        // lazy initialization
        if (realSubject == null)
        {
            realSubject = new RealSubject();
        }
        realSubject.Request();
    }
}
public class Client
{
    public static void UseSubject(Subject subject)
    {
        subject.Request();
    }
    public static void Main()
    {
        Subject proxy = new SubjectProxy();
        UseSubject(proxy);
        Console.ReadKey();
    }
}
/* OUTPUT:
RealSubject.Request()
*/
