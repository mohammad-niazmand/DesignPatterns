class Director
{
	private Builder builder;
	public void SetBuilder(Builder builder)
	{
		this.builder = builder;
	}
	public void Construct()
	{
		Console.WriteLine("Directing building of a product ...");
		
		builder.BuildPart1();
		builder.BuildPart2();
	}
}
abstract class Builder
{
	protected Product product;
	
	public abstract void BuildPart1();
	public abstract void BuildPart2();
	public virtual Product GetResult()
	{
		return product;
	}
	public Builder()
	{
		product = new Product();
	}
}
class Builder1 : Builder
{
	public override void BuildPart1()
	{
		product.Part1 = "Part-A";
		Console.WriteLine("Part1 created");
	}
	public override void BuildPart2()
	{
		product.Part2 = "Part-B";
		Console.WriteLine("Part2 created");
	}
	public override Product GetResult()
	{
		product.Name = "Product I";
		Console.WriteLine(product.Name + " created");
		
		return base.GetResult();
	}
}
class Builder2 : Builder
{
	public override void BuildPart1()
	{
		product.Part1 = "Part-X";
		Console.WriteLine("Part1 created");
	}
	public override void BuildPart2()
	{
		product.Part2 = "Part-Y";
		Console.WriteLine("Part2 created");
	}
	public override Product GetResult()
	{
		product.Name = "Product II";
		Console.WriteLine(product.Name + " created");
		
		return base.GetResult();
	}
}
class Product
{
	public string Name { get; set; }
	public string Part1 { get; set; }
	public string Part2 { get; set; }
	public void Display()
	{
		Console.WriteLine("\nProduct Name: " + this.Name);
		Console.WriteLine("Part1 : " + this.Part1);
		Console.WriteLine("Part2 : " + this.Part2);
		
		Console.WriteLine();
	}
}
class Client
{
	public static void Main()
	{
		Director director = new Director();

		var b1 = new Builder1();
		var b2 = new Builder2();

		director.SetBuilder(b1);
		director.Construct();
		var p1 = b1.GetResult();
		p1.Display();

		director.SetBuilder(b2);
		director.Construct();
		var p2 = b2.GetResult();
		p2.Display();
		
		Console.ReadKey();
	}
}
/* Output
Directing building of a product ...
Part1 created
Part2 created
Product I created

Product Name: Product I
Part1 : Part-A
Part2 : Part-B

Directing building of a product ...
Part1 created
Part2 created
Product II created

Product Name: Product II
Part1 : Part-X
Part2 : Part-Y
*/