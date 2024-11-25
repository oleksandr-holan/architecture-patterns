namespace _14_chain_of_responsibility;

public static class StringExtensions
{
    public static bool SharesAnyCharacter(this string? s1, string s2)
    {
        return s1 != null && s1.Any(s2.Contains);
    }
}

internal interface IValidate
{
    void Handle(string? s);
}

internal abstract class BaseValidator(IValidate? next = null) : IValidate
{
    private IValidate? Next { get; } = next;

    public virtual void Handle(string? s)
    {
        Next?.Handle(s);
    }
}

internal class NonEmpty(IValidate? next = null) : BaseValidator(next)
{
    public override void Handle(string? s)
    {
        if (string.IsNullOrEmpty(s))
        {
            throw new Exception("Can't be empty");
        }
        base.Handle(s);
    }
}

internal class HasLowerLetters(IValidate? next = null) : BaseValidator(next)
{
    public override void Handle(string? s)
    {
        if (!s.SharesAnyCharacter("abcdefghijklmnopqrstuvwxyz"))
        {
            throw new Exception("Must have lower letters");
        }
        base.Handle(s);
    }
}

internal class HasUpperLetters(IValidate? next = null) : BaseValidator(next)
{
    public override void Handle(string? s)
    {
        if (!s.SharesAnyCharacter("ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
        {
            throw new Exception("Must have upper letters");
        }
        base.Handle(s);
    }
}

internal class Input(string? text = null, string? error = null)
{
    public string? Text { get; } = text;
    public string? Error { get; set; } = error;
}

internal static class Program
{
    private static readonly string[] SourceArray = ["lower", "UPPER", "lowerUPPER"];

    private static void Main()
    {
        var validator = new NonEmpty(new HasLowerLetters(new HasUpperLetters()));
        var inputs = SourceArray.Select(s => new Input(s)).ToList();

        foreach (var inp in inputs)
        {
            try
            {
                validator.Handle(inp.Text);
                Console.WriteLine($"{inp.Text} is valid!");
            }
            catch (Exception e)
            {
                inp.Error = e.Message;
                Console.WriteLine($"{inp.Text} {inp.Error}");
            }
        }
    }
}