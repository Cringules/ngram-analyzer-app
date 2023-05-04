namespace Cringules.NGram.App.Model;

public class NamedItem<T>
{
    public string Name { get; }
    public T Value { get; }

    public NamedItem(string name, T value)
    {
        Name = name;
        Value = value;
    }
}
