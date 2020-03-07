public interface IPrototype
{
	IPrototype Clone();
}
public class ConcretePrototype: IPrototype
{
	public int SomeProperty { get; set; }
	public ConcretePrototype(int someProperty)
	{
		this.SomeProperty = someProperty;
	}
	public IPrototype Clone()
	{
		return (IPrototype)this.MemberwiseClone();
	}
}
public class ComplexClass: IPrototype
{
	public ConcretePrototype SomeProperty { get; set; }
	public ComplexClass(ConcretePrototype someProperty)
	{
		this.SomeProperty = someProperty;
	}
	public IPrototype Clone()
	{
		var result = (ComplexClass)this.MemberwiseClone();
		result.SomeProperty = (ConcretePrototype)this.SomeProperty.Clone();
		return (IPrototype)result;
	}
}
public class Test
{
	public static void Main()
	{
		ConcretePrototype cp1 = new ConcretePrototype(25);
		ConcretePrototype cp2 = (ConcretePrototype)cp1.Clone();
		
		Console.WriteLine("cp1.SomeProperty == cp2.SomeProperty? {0}", (cp1.SomeProperty == cp2.SomeProperty));
		Console.WriteLine("cp1 == cp2? {0}", (cp1 == cp2));
		
		Console.WriteLine();
		
		ComplexClass cc1 = new ComplexClass(cp1);
		ComplexClass cc2 = (ComplexClass)cc1.Clone();
		
		Console.WriteLine("cc1.SomeProperty == cc2.SomeProperty? {0}", (cc1.SomeProperty == cc2.SomeProperty));
		Console.WriteLine("cc1 == cp2? {0}", (cc1 == cc2));
		
		Console.ReadKey();
	}
}
/* Output
cp1.SomeProperty == cp2.SomeProperty? True
cp1 == cp2? False

cc1.SomeProperty == cc2.SomeProperty? False
cc1 == cp2? False
*/
