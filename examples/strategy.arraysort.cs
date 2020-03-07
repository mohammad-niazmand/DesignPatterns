public class Rectangle
{
	public int Width { get; set; }
	public int Height { get; set; }
	public Rectangle(int width, int height)
	{
		Width = width;
		Height = height;
	}
	public int Area()
	{
		return Width * Height;
	}
	public void Show()
	{
		Console.WriteLine("Rectangle {0}x{1}: Area = {2}", Width, Height, Area());
	}
}
public class RectangleComparer: IComparer<Rectangle>
{
	public int Compare(Rectangle x, Rectangle y)
	{
		var areaX = x.Area();
		var areaY = y.Area();

		if (areaX == areaY)
			return 0;
		else if (areaX > areaY)
			return 1;
		else
			return -1;
	}
}
public static class Test
{
	static void ForAll<T>(IEnumerable<T> items, Action<T> action)
	{
		foreach (var item in items)
		{
			action(item);
		}
	}
	static void Main()
	{
		var rectangles = new Rectangle[]
		{
			new Rectangle(21, 13),
			new Rectangle(14, 42),
			new Rectangle(16, 26),
			new Rectangle(35, 47),
			new Rectangle(30, 29),
			new Rectangle(46, 11),
			new Rectangle(29, 24),
			new Rectangle(19, 10),
			new Rectangle(35, 23)
		};

		Console.WriteLine("\nList of Rectangles before sort:\n");
		ForAll(rectangles, rect => rect.Show());

		Array.Sort<Rectangle>(rectangles, new RectangleComparer());

		Console.WriteLine("\nList of Rectangles After sort:\n");
		ForAll(rectangles, rect => rect.Show());

		Console.ReadKey();
	}
}
/*
* List of Rectangles before sort:

Rectangle 21x13: Area = 273
Rectangle 14x42: Area = 588
Rectangle 16x26: Area = 416
Rectangle 35x47: Area = 1645
Rectangle 30x29: Area = 870
Rectangle 46x11: Area = 506
Rectangle 29x24: Area = 696
Rectangle 19x10: Area = 190
Rectangle 35x23: Area = 805

List of Rectangles After sort:

Rectangle 19x10: Area = 190
Rectangle 21x13: Area = 273
Rectangle 16x26: Area = 416
Rectangle 46x11: Area = 506
Rectangle 14x42: Area = 588
Rectangle 29x24: Area = 696
Rectangle 35x23: Area = 805
Rectangle 30x29: Area = 870
Rectangle 35x47: Area = 1645
*/
