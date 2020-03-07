public class SettingCollection: IEnumerable<Setting>
{
	private List<Setting> settings;
	private int _iterator_protector;
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
		{
            settings.Add(new Setting(name, value, isEncrypted));
			_iterator_protector++;
		}
	}
	public void Remove(string name)
	{
		var index = settings.FindIndex(x => String.Compare(x.Name, name, true) == 0);
		if (index >= 0)
		{
			settings.RemoveAt(index);
			_iterator_protector++;
		}
		else
			throw new ArgumentException("setting not found");
	}
    public IEnumerator<Setting> GetEnumerator()
    {
        return new Enumerator(this, _iterator_protector);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
	public class Enumerator: IEnumerator<Setting>
	{
		private int index;
		private SettingCollection parent;
		private int protector;
		public Enumerator(SettingCollection parent, int protector)
		{
			this.parent = parent;
			this.protector = protector;
			index = -1;
		}
		public Setting Current
		{
			get
			{
				if (this.protector != this.parent._iterator_protector)
					throw new InvalidOperationException("Collection modified during enumeration");
				else
					return parent.settings[index];
			}
		}
		object System.Collections.IEnumerator.Current
		{
			get { return this.Current; }
		}
		public bool MoveNext()
		{
			if (this.protector != this.parent._iterator_protector)
				throw new InvalidOperationException("Collection modified during enumeration");
			else
			{
				bool result = false;
				if (index < parent.settings.Count - 1)
				{
					index++;
					result = true;
				}
				return result;
			}
		}
		public void Reset()
		{
			if (this.protector != this.parent._iterator_protector)
				throw new InvalidOperationException("Collection modified during enumeration");
			else
				index = -1;
		}
		public void Dispose() { }
    }
}