using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Serializer
{
    public static byte[] Serialize(object o)
    {
        var stream = new MemoryStream();
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, o);
        return stream.ToArray();
    }
    public static object Deserialize(byte[] bytes)
    {
        var stream = new MemoryStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        var formatter = new BinaryFormatter();
        var o = formatter.Deserialize(stream);
        stream.Close();
        return o;
    }
}
public class TripleDESCryptographyConfig
{
    private byte[] key;
    private byte[] iv;
    public byte[] Key
    {
        get { return key; }
        set { key = value; }
    }
    public byte[] IV
    {
        get { return iv; }
        set { iv = value; }
    }

    public TripleDESCryptographyConfig(string key, string iv)
    {
        this.key = UTF8Encoding.UTF8.GetBytes(key);
        this.iv = UTF8Encoding.UTF8.GetBytes(iv);
    }
    public TripleDESCryptographyConfig(byte[] key, byte[] iv)
    {
        this.key = key;
        this.iv = iv;
    }
}
public class TripleDESCryptography
{
    public TripleDESCryptographyConfig Config { get; private set; }

    public TripleDESCryptography(TripleDESCryptographyConfig config)
    {
        this.Config = config;
    }
    public byte[] Encrypt(byte[] data)
    {
        byte[] result;

        var mStream = new MemoryStream();
        var encyptor = new TripleDESCryptoServiceProvider().CreateEncryptor(Config.Key, Config.IV);
        var cStream = new CryptoStream(mStream, encyptor, CryptoStreamMode.Write);
        cStream.Write(data, 0, data.Length);
        cStream.FlushFinalBlock();

        result = mStream.ToArray();

        cStream.Close();
        mStream.Close();

        return result;
    }
    public byte[] Decrypt(byte[] data)
    {
        byte[] result;

        var mStream = new MemoryStream(data);
        var decryptor = new TripleDESCryptoServiceProvider().CreateDecryptor(Config.Key, Config.IV);
        var cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);

        result = new byte[data.Length];
        cStream.Read(result, 0, result.Length);

        cStream.Close();
        mStream.Close();

        return result;
    }
}
public class Originator
{
    [Serializable]
    public class OriginatorState
    {
        public List<string> List { get; private set; }
        public string Name { get; private set; }

        public OriginatorState(List<string> list, string name)
        {
            this.List = list;
            this.Name = name;
        }
    }

    private TripleDESCryptography crypt;

    public string Name { get; private set; }
    private List<string> list = new List<string>();

    private void Init(string name)
    {
        var config = new TripleDESCryptographyConfig("abcdefghijklmnopqrstuvwx", "12345678");
        this.crypt = new TripleDESCryptography(config);
        this.Name = name;
    }
    public Originator()
    {
        Init("");
    }
    public Originator(string name)
    {
        Init(name);
    }
    public void Append(string s)
    {
        list.Add(s);
    }
    public Memento CreateMemento()
    {
        var state = new OriginatorState(list, Name);
        var data = Serializer.Serialize(state);
        var encryptedData = crypt.Encrypt(data);

        return new Memento(encryptedData);
    }
    public void SetMemento(Memento m)
    {
        var decryptedData = crypt.Decrypt(m.Data);
        var data = (OriginatorState)Serializer.Deserialize(decryptedData);
        list = data.List;
        Name = data.Name;
    }
    public void Print()
    {
        foreach (var s in list)
        {
            Console.WriteLine(s);
        }
        Console.WriteLine();
    }
}
public class Memento
{
    public byte[] Data { get; private set; }
    public Memento(byte[] data)
    {
        this.Data = data;
    }
    public byte[] GetState()
    {
        return this.Data;
    }
}
public class Test
{
    public static void Main()
    {
        var originator1 = new Originator("Originator");

        originator1.Append("List of Programming Languages\n");
        originator1.Append("C++");
        originator1.Append("Java");
        originator1.Append("C#");
        originator1.Append("Python");
        originator1.Append("Groovy");
        originator1.Append("Ruby");

        var memento = originator1.CreateMemento();

        var originator2 = new Originator();
        originator2.SetMemento(memento);

        originator2.Print();

        Console.ReadKey();
    }
}
/* OUTPUT:
List of Programming Languages

C++
Java
C#
Python
Groovy
Ruby
*/