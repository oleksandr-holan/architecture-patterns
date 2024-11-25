using System.Collections;
using System.Text;

namespace _16_iterator;

internal abstract class Component
{
    public abstract override string ToString();
}

internal class Composite(string tagName, params Component[] children)
    : Component, IEnumerable<Component>
{
    public List<Component> Children { get; } = [..children];
    private string TagName { get; } = tagName;

    public override string ToString()
    {
        return TagName;
    }

    public IEnumerator<Component> GetEnumerator()
    {
        return new CompositeEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal class CompositeEnumerator : IEnumerator<Component>
{
    private readonly Stack<IEnumerator<Component?>> _stack = new();

    public CompositeEnumerator(Composite root)
    {
        _stack.Push(new List<Component> { root }.GetEnumerator());
    }

    public Component? Current { get; private set; }

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        while (_stack.Count > 0)
        {
            if (_stack.Peek().MoveNext())
            {
                Current = _stack.Peek().Current;
                if (Current is Composite composite)
                {
                    _stack.Push(composite.Children.GetEnumerator());
                }
                return true;
            }
            _stack.Pop();
        }
        return false;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose() { }
}

internal static class Program
{
    private static Composite Div(params Component[] children) => new("div", children);
    private static Composite P(params Component[] children) => new("p", children);

    private static void Main()
    {
        var tags = Div(
            P(
                Div(),
                P()
            ),
            Div(
                P()
            )
        );

        Console.WriteLine(ToHtml(tags));
        Console.Write("Flattened: ");
        foreach (var tag in tags)
        {
            Console.Write(tag + " ");
        }
        Console.WriteLine();
    }

    private static string ToHtml(Component component, int indent = 0)
    {
        var result = new StringBuilder();
        result.AppendLine($"{new string(' ', indent * 4)}<{component}>");

        if (component is Composite composite)
        {
            foreach (var child in composite.Children)
            {
                result.Append(ToHtml(child, indent + 1));
            }
        }

        result.AppendLine($"{new string(' ', indent * 4)}</{component}>");
        return result.ToString();
    }
}