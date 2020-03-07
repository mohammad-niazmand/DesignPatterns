public class Setting
{
	public string Name { get; private set; }
	public bool IsEncrypted { get; private set; }
	public string Value { get; private set; }
	
	public Setting(string name, string value, bool isEncrypted = false)
	{
		this.Name = name;
		this.Value = value;
		this.IsEncrypted = isEncrypted;
	}
}
public class SettingCollection: IEnumerable<Setting>
{
	private List<Setting> settings;
	
	public SettingCollection()
	{
		this.settings = new List<Setting>();
	}
	public int Count
	{
		get { return settings.Count; }
	}
	public void Add(string name, string value, bool isEncrypted = false)
	{
		if (settings.Exists(x => String.Compare(x.Name, name, true) == 0))
			throw new ArgumentException("setting already exists");
		else
            settings.Add(new Setting(name, value, isEncrypted));
	}
	public void Remove(string name)
	{
		var index = settings.FindIndex(x => String.Compare(x.Name, name, true) == 0);
		if (index >= 0)
			settings.RemoveAt(index);
		else
			throw new ArgumentException("setting not found");
	}
    public IEnumerator<Setting> GetEnumerator()
    {
        return new Enumerator(this);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
	public class Enumerator: IEnumerator<Setting>
	{
		private int index;
		private SettingCollection parent;
		public Enumerator(SettingCollection parent)
		{
			this.parent = parent;
			index = -1;
		}
		object System.Collections.IEnumerator.Current
		{
			get { return parent.settings[index]; }
		}
		public Setting Current
		{
			get { return parent.settings[index]; }
		}
		public bool MoveNext()
		{
			bool result = false;
			if (index < parent.settings.Count - 1)
			{
				index++;
				result = true;
			}
			return result;
		}
		public void Reset() { index = -1; }
		public void Dispose() { }
    }
}
class Test
{
	static void Main()
	{
        var settings = new SettingCollection();

        settings.Add("MembershipEnabled", "true");
        settings.Add("OrderProductsEnabled", "true");
        settings.Add("ConnectionTimeOut", "5");
        settings.Add("DefaultTheme", "green");
        settings.Add("MailPassword", "mailpass", true);

        Console.WriteLine("\tName\t\tValue\tEncrypted");
        Console.WriteLine(new String('-', 40));

        foreach (var setting in settings)
        {
            Console.WriteLine("{0,-25}{1,-10}{2,-5}", setting.Name, setting.Value, setting.IsEncrypted);
        }
        
		Console.ReadKey();
	}
}
/* OUTPUT:
         Name            Value   Encrypted
----------------------------------------
MembershipEnabled        true      False
OrderProductsEnabled     true      False
ConnectionTimeOut        5         False
DefaultTheme             green     False
MailPassword             mailpass  True
*/
