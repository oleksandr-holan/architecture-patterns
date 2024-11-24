namespace _12_flyweight;

internal class CharacterStyle(string font, int size, bool bold, bool italic)
{
    public string Font { get; } = font;
    public int Size { get; } = size;
    public bool Bold { get; } = bold;
    public bool Italic { get; } = italic;
}

internal class Character(char ch, CharacterStyle style)
{
    private char Char { get; } = ch;
    private CharacterStyle Style { get; } = style;

    public void Render()
    {
        var bold = Style.Bold ? "bold" : "normal";
        var italic = Style.Italic ? "italic" : "normal";
        Console.WriteLine($"Rendering '{Char}' in {Style.Font}, size {Style.Size}, {bold}, {italic}");
    }
}

internal class CharacterFactory
{
    private readonly Dictionary<(char, string, int, bool, bool), Character> _characters = new();

    public Character GetCharacter(char ch, CharacterStyle style)
    {
        var key = (ch, style.Font, style.Size, style.Bold, style.Italic);
        if (_characters.TryGetValue(key, out var value)) return value;
        value = new Character(ch, style);
        _characters[key] = value;
        return value;
    }

    public int Count => _characters.Count;
}

internal class Document
{
    private readonly List<Character> _characters = [];
    private readonly CharacterFactory _factory = new();

    public void AddCharacter(char ch, string font, int size, bool bold = false, bool italic = false)
    {
        var style = new CharacterStyle(font, size, bold, italic);
        var character = _factory.GetCharacter(ch, style);
        _characters.Add(character);
    }

    public void Render()
    {
        foreach (var character in _characters)
        {
            character.Render();
        }
    }

    public int CharacterCount => _characters.Count;
    public int UniqueCharacterCount => _factory.Count;
}

internal static class Program
{
    private static void Main()
    {
        var document = new Document();

        const string text = "Some text with repeating characters!";
        foreach (var ch in text)
        {
            if (char.IsUpper(ch))
            {
                document.AddCharacter(ch, "Arial", 12, bold: true);
            }
            else if (char.IsLower(ch))
            {
                document.AddCharacter(ch, "Times New Roman", 10);
            }
            else
            {
                document.AddCharacter(ch, "Courier", 11, italic: true);
            }
        }

        document.Render();

        Console.WriteLine($"\nNumber of unique character objects: {document.UniqueCharacterCount}");
        Console.WriteLine($"Number of characters: {document.CharacterCount}");
    }
}