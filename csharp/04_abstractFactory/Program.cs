namespace _04_abstractFactory;

public interface ICloth
{
    IModern CreateModern();
    IClassic CreateClassic();
}

internal class Pants : ICloth
{
    public IModern CreateModern()
    {
        return new ModernPants();
    }

    public IClassic CreateClassic()
    {
        return new ClassicPants();
    }
}

internal class Shirt : ICloth
{
    public IModern CreateModern()
    {
        return new ModernShirt();
    }

    public IClassic CreateClassic()
    {
        return new ClassicShirt();
    }
}

public interface IModern
{
    string GetStyle();
}

internal class ModernPants : IModern
{
    public string GetStyle()
    {
        return "Slim fit, low-rise pants";
    }
}

internal class ModernShirt : IModern
{
    public string GetStyle()
    {
        return "Fitted, short-sleeved shirt";
    }
}

public interface IClassic
{
    string GetStyle();
    string MatchWith(IModern modernPiece);
}

internal class ClassicPants : IClassic
{
    public string GetStyle()
    {
        return "Straight-leg, high-waisted pants";
    }

    public string MatchWith(IModern modernPiece)
    {
        var modernStyle = modernPiece.GetStyle();
        return $"Classic pants paired with {modernStyle}";
    }
}

internal class ClassicShirt : IClassic
{
    public string GetStyle()
    {
        return "Button-down, long-sleeved shirt";
    }

    public string MatchWith(IModern modernPiece)
    {
        var modernStyle = modernPiece.GetStyle();
        return $"Classic shirt paired with {modernStyle}";
    }
}

internal static class ClothingStore
{
    public static void Start()
    {
        Console.WriteLine("Clothing Store: Showcasing pants...");
        ShowcaseClothing(new Pants());
        Console.WriteLine();

        Console.WriteLine("Clothing Store: Showcasing shirts...");
        ShowcaseClothing(new Shirt());
    }

    private static void ShowcaseClothing(ICloth factory)
    {
        var modernPiece = factory.CreateModern();
        var classicPiece = factory.CreateClassic();

        Console.WriteLine($"Modern style: {modernPiece.GetStyle()}");
        Console.WriteLine($"Classic style: {classicPiece.GetStyle()}");
        Console.WriteLine(classicPiece.MatchWith(modernPiece));
    }
}

internal static class Program
{
    private static void Main()
    {
        ClothingStore.Start();
    }
}