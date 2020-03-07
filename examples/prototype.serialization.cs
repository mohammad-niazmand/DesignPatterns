public static class MyExtensionMethods
{
    public static T DeepClone<T>(this T a)
    {
        using (var stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
}
[Serializable]
class Engine { }

[Serializable]
class Car
{
    public string Model { get; set; }
    public Engine Engine { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
    public int Year { get; set; }
}

class CarPrototypeManager
{
    private Dictionary<string, Car> prototypes = new Dictionary<string, Car>();

    public void Add(string key, Car car)
    {
        prototypes.Add(key, car);
    }
    public Car CreateCar(string key)
    {
        return prototypes[key].DeepClone<Car>();
    }
}
class Test
{
    static void Main()
    {
        CarPrototypeManager manager = new CarPrototypeManager();

        manager.Add("c1", new Car { Model = "Honda Accord", Year = 2013, Engine = new Engine() });
        manager.Add("c2", new Car { Model = "Hyundai Sonata", Year = 2011, Engine = new Engine() });
        manager.Add("c3", new Car { Model = "Toyota Prius", Year = 2013, Engine = new Engine() });

        var c1 = manager.CreateCar("c3");
        var c2 = manager.CreateCar("c3");

        Console.WriteLine("c1.Model: {0}", c1.Model);
        Console.WriteLine("c2.Model: {0}", c2.Model);
        Console.WriteLine("c1 == c2 ? {0}", c1 == c2);
        Console.WriteLine("c1.Engine == c2.Engine ? {0}", c1.Engine == c2.Engine);

        Console.ReadKey();
    }
}
/* OUTPUT:
c1.Model: Toyota Prius
c2.Model: Toyota Prius
c1 == c2 ? False
c1.Engine == c2.Engine ? False
*/
