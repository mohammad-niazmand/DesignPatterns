class SoftwareProduct
{
	public string Name { get; set; }
	public string StudyDocs { get; private set; }
	public void CreateStudyDocs(string docs)
	{
		this.StudyDocs = docs;
	}
	public string AnalysisDocs { get; private set; }
	public void CreateAnalysisDocs(string docs)
	{
		this.AnalysisDocs = docs;
	}
	public string DesignDocs { get; private set; }
	public void CreateDesignDocs(string docs)
	{
		this.DesignDocs = docs;
	}
	public string Implementation { get; private set; }
	public void CreateImplementation(string implementation)
	{
		this.Implementation = implementation;
	}
	public string TestDocs { get; private set; }
	public void CreateTestDocs(string docs)
	{
		this.TestDocs = docs;
	}
	public string DeliveryDocs { get; private set; }
	public void CreateDeliveryDocs(string docs)
	{
		this.DeliveryDocs = docs;
	}
	public void Show()
	{
		Console.WriteLine("Product Name: " + this.Name);
		Console.WriteLine(this.StudyDocs);
		Console.WriteLine(this.AnalysisDocs);
		Console.WriteLine(this.DesignDocs);
		Console.WriteLine(this.Implementation);
		Console.WriteLine(this.TestDocs);
		Console.WriteLine(this.DeliveryDocs);
	}
}
class ProjectManager
{
	private Team team;
	public void SetTeam(Team team)
	{
		this.team = team;
	}
	public void Run()
	{
		team.DefineProduct();
		team.Study();
		team.Analyze();
		team.Design();
		team.Implement();
		team.Test();
	}
}
abstract class Team
{
	protected SoftwareProduct product;
	
	public abstract void DefineProduct();
	public abstract void Study();
	public abstract void Analyze();
	public abstract void Design();
	public abstract void Implement();
	public abstract void Test();
	public abstract SoftwareProduct GetProduct();
	
	public Team()
	{
		product = new SoftwareProduct();
	}
}
class Team1: Team
{
	public override void DefineProduct()
	{
		product.Name = "Software Product I";
		Console.WriteLine("Product '{0}' Started", product.Name);
	}
	public override void Study()
	{
		product.CreateStudyDocs("Study Documentation");
		Console.WriteLine("\tStudy Phase finished.");
	}
	public override void Analyze()
	{
		product.CreateAnalysisDocs("Analysis Documentation");
		Console.WriteLine("\tAnalysis Phase finished.");
	}
	public override void Design()
	{
		product.CreateDesignDocs("Design Documentation");
		Console.WriteLine("\tDesign Phase finished.");
	}
	public override void Implement()
	{
		product.CreateImplementation("Implementation");
		Console.WriteLine("\tImplementation Phase finished.");
	}
	public override void Test()
	{
		product.CreateTestDocs("Test Documentation");
		Console.WriteLine("\tTest Phase finished.");
	}
	public override SoftwareProduct GetProduct()
	{
		Console.WriteLine("Product delivered.\n");
		
		return product;
	}
}
class Team2: Team
{
	public override void DefineProduct()
	{
		product.Name = "Software Product II";
		Console.WriteLine("Product '{0}' Started", product.Name);
	}
	public override void Study()
	{
		product.CreateStudyDocs("Study Documentation");
		Console.WriteLine("\tStudy Phase finished.");
	}
	public override void Analyze()
	{
		product.CreateAnalysisDocs("Analysis Documentation");
		Console.WriteLine("\tAnalysis Phase finished.");
	}
	public override void Design()
	{
		product.CreateDesignDocs("Design Documentation");
		Console.WriteLine("\tDesign Phase finished.");
	}
	public override void Implement()
	{
		product.CreateImplementation("Implementation");
		Console.WriteLine("\tImplementation Phase finished.");
	}
	public override void Test()
	{
		product.CreateTestDocs("Test Documentation");
		Console.WriteLine("\tTest Phase finished.");
	}
	public override SoftwareProduct GetProduct()
	{
		Console.WriteLine("Product delivered.\n");
		
		return product;
	}
}
class Test
{
	public static void Main()
	{
		var pm = new ProjectManager();
		var team1 = new Team1();
		pm.SetTeam(team1);
		pm.Run();
		var product1 = team1.GetProduct();
		product1.Show();
		
		var team2 = new Team2();
		pm.SetTeam(team2);
		pm.Run();
		var product2 = team2.GetProduct();
		product2.Show();
		
		Console.ReadKey();
	}
}
/* OUTPUT:
Product 'Software Product I' Started
        Study Phase finished.
        Analysis Phase finished.
        Design Phase finished.
        Implementation Phase finished.
        Test Phase finished.
Product delivered.

Product Name: Software Product I
Study Documentation
Analysis Documentation
Design Documentation
Implementation
Test Documentation
*/