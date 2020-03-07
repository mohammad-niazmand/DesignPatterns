enum TraverseType { PreOrder, InOrder, PostOrder }

class BinaryTree<T> : IEnumerable<T> where T : struct
{
    public Nullable<T> Data;
    protected BinaryTree<T> Left;
    protected BinaryTree<T> Right;

    public BinaryTree()
    {
        Data = null;
    }
    public void Add(params T[] items)
    {
        foreach (var item in items)
        {
            this.Add(item);
        }
    }
    public void Add(T item)
    {
        if (!Data.HasValue)
            this.Data = item;
        else
        {
            var x = (IComparable<T>)Data.Value;
            var compare = x.CompareTo(item);

            if (compare < 0)
            {
                if (this.Right != null)
                    this.Right.Add(item);
                else
                {
                    this.Right = new BinaryTree<T>();
                    this.Right.Add(item);
                }
            }
            else if (compare > 0)
            {
                if (this.Left != null)
                    this.Left.Add(item);
                else
                {
                    this.Left = new BinaryTree<T>();
                    this.Left.Add(item);
                }
            }
        }
    }
    private IEnumerable<T> PreOrder()
    {
        BinaryTree<T> current = this;

        yield return current.Data.Value;

        if (current.Left != null)
            foreach (T data in current.Left.PreOrder())
                yield return data;

        if (current.Right != null)
            foreach (T data in current.Right.PreOrder())
                yield return data;
    }
    private IEnumerable<T> InOrder()
    {
        BinaryTree<T> current = this;

        if (current.Left != null)
            foreach (T data in current.Left.InOrder())
                yield return data;

        yield return current.Data.Value;

        if (current.Right != null)
            foreach (T data in current.Right.InOrder())
                yield return data;
    }
    private IEnumerable<T> PostOrder()
    {
        BinaryTree<T> current = this;

        if (current.Left != null)
            foreach (T data in current.Left.PostOrder())
                yield return data;

        if (current.Right != null)
            foreach (T data in current.Right.PostOrder())
                yield return data;

        yield return current.Data.Value;
    }
    public IEnumerable<T> Traverse(TraverseType t)
    {
        IEnumerable<T> structure = null;
        switch (t)
        {
            case TraverseType.InOrder: structure = InOrder(); break;
            case TraverseType.PreOrder: structure = PreOrder(); break;
            case TraverseType.PostOrder: structure = PostOrder(); break;
        }

        foreach (T data in structure)
            yield return data;
    }
    public IEnumerator<T> GetEnumerator()
    {
        var x = this.Traverse(TraverseType.InOrder);
        return x.GetEnumerator();
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
class Test
{
    static void Print(string title, IEnumerable<int> structure)
    {
        Console.Write(title + ": ");

        foreach (var x in structure)
        {
            Console.Write("{0} ", x);
        }
        Console.WriteLine();
    }
    static void Main()
    {
        var tree = new BinaryTree<int>();

        tree.Add(20, 30, 10, 15, 5, 18, 7, 4);

        Print("InOrder: ", tree);	// use default traverse: InOrder
        Print("PreOrder: ", tree.Traverse(TraverseType.PreOrder));
        Print("PostOrder: ", tree.Traverse(TraverseType.PostOrder));

        Console.ReadKey();
    }
}
/* OUTPUT:
InOrder: : 4 5 7 10 15 18 20 30
PreOrder: : 20 10 5 4 7 15 18 30
PostOrder: : 4 7 5 18 15 10 30 20
*/
