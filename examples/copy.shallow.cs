class Engine {}
class Car
{
    public string Model { get; set; }
    public Engine Engine { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
    public int Year { get; set; }
    
    public Car Clone()
    {
        return (Car)this.MemberwiseClone();
    }
}
class Test
{
    static void Main()
    {
        var engine = new Engine();
        var model = "Nissan Versa";
        var prototype = new Car
            {
                Model = model,
                Engine = engine,
                Width = 2.3,
                Length = 3.6,
                Year = 2011
            };
        var clone = prototype.Clone();
        
        if (clone.Engine == engine)
            Console.WriteLine("clone and prototype are using the same engine");
        else
            Console.WriteLine("clone and prototype have distinct engines");
            
        model = "Toyota Yaris";
        
        Console.WriteLine("prototype model: {0}", prototype. Model);
        Console.WriteLine("clone model: {0}", clone. Model);

        Console.ReadKey();
    }
}
/* OUTPUT:
clone and prototype are using the same engine
prototype model: Nissan Versa
clone model: Nissan Versa
*/
