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
	public SettingCollection(): base()
	{
		this.settings = new List<Setting>();
	}
	public int Count
	{
		get { return settings.Count; }
	}
	public void Add(Setting setting)
	{
		if (setting == null)
			throw new ArgumentNullException();
		
		settings.Add(setting);
	}
	public void Add(string name, string value, bool isEncrypted = false)
	{
		if (settings.Exists(x => String.Compare(x.Name, name, true) == 0))
			throw new ArgumentException("setting already exists");
		else
            this.Add(new Setting(name, value, isEncrypted));
	}
	public void Remove(Setting setting)
	{
		settings.Remove(setting);
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
        foreach (var setting in settings)
		{
			yield return setting;
		}
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
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
        //Console.WriteLine("{0,20}", "hi");
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
