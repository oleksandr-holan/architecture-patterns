namespace _10_decorator;

internal interface ICoffee
{
    double GetCost();
    string GetDescription();
}

internal class Coffee : ICoffee
{
    public double GetCost() => 2.0;
    public string GetDescription() => "Simple coffee";
}

internal abstract class CoffeeDecorator(ICoffee coffee) : ICoffee
{
    public virtual double GetCost() => coffee.GetCost();
    public virtual string GetDescription() => coffee.GetDescription();
}

internal class Milk(ICoffee coffee) : CoffeeDecorator(coffee)
{
    public override double GetCost() => base.GetCost() + 0.5;
    public override string GetDescription() => $"{base.GetDescription()}, milk";
}

internal class Sugar(ICoffee coffee) : CoffeeDecorator(coffee)
{
    public override double GetCost() => base.GetCost() + 0.2;
    public override string GetDescription() => $"{base.GetDescription()}, sugar";
}

internal class Vanilla(ICoffee coffee) : CoffeeDecorator(coffee)
{
    public override double GetCost() => base.GetCost() + 0.7;
    public override string GetDescription() => $"{base.GetDescription()}, vanilla";
}

internal static class Program
{
    private static void Main()
    {
        var coffee = new Coffee();
        Console.WriteLine($"Cost: ${coffee.GetCost():F2}, Description: {coffee.GetDescription()}");

        var coffeeWithMilk = new Milk(coffee);
        Console.WriteLine($"Cost: ${coffeeWithMilk.GetCost():F2}, Description: {coffeeWithMilk.GetDescription()}");

        var coffeeWithMilkAndSugar = new Sugar(coffeeWithMilk);
        Console.WriteLine($"Cost: ${coffeeWithMilkAndSugar.GetCost():F2}, Description: {coffeeWithMilkAndSugar.GetDescription()}");

        var fancyCoffee = new Vanilla(new Milk(new Sugar(new Coffee())));
        Console.WriteLine($"Cost: ${fancyCoffee.GetCost():F2}, Description: {fancyCoffee.GetDescription()}");
    }
}