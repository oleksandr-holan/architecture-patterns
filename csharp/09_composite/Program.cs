namespace _09_composite;

public abstract class Item
{
    public abstract float GetPrice();
    public abstract void Display(string indent = "");
}

internal class Composition(string name) : Item
{
    private readonly List<Item> _components = [];

    public virtual void Add(Item component)
    {
        _components.Add(component);
    }

    public virtual void Remove(Item component)
    {
        _components.Remove(component);
    }

    public override float GetPrice()
    {
        return _components.Sum(component => component.GetPrice());
    }

    public override void Display(string indent = "")
    {
        Console.WriteLine($"{indent}{name}:");
        foreach (var component in _components)
        {
            component.Display(indent + "  ");
        }
    }
}

internal class PcItem(string name, float price, string category) : Item
{
    public override float GetPrice()
    {
        return price;
    }

    public override void Display(string indent = "")
    {
        Console.WriteLine($"{indent}{name} ({category}): ${price:F2}");
    }
}

internal class PcBuild(string name) : Composition(name)
{
    public override void Add(Item component)
    {
        if (component is PcItem)
        {
            base.Add(component);
        }
        else
        {
            throw new ArgumentException("Only PCItems can be added to a PCBuild");
        }
    }
}

internal class Order() : Composition("Order");

internal static class Program
{
    private static void Main()
    {
        var order = new Order();

        // Add regular items
        order.Add(new PcItem("Keyboard", 49.99f, "Keyboards"));
        order.Add(new PcItem("Mouse", 29.99f, "Mice"));

        // Create a PC build
        var pcBuild = new PcBuild("Gaming PC");
        pcBuild.Add(new PcItem("CPU", 299.99f, "Processor"));
        pcBuild.Add(new PcItem("GPU", 499.99f, "Graphics Card"));
        pcBuild.Add(new PcItem("RAM", 89.99f, "Memory"));
        pcBuild.Add(new PcItem("SSD", 119.99f, "Storage"));

        // Add the PC build to the order
        order.Add(pcBuild);

        // Add more items
        order.Add(new PcItem("Monitor", 199.99f, "Monitors"));

        // Display the order
        order.Display();

        // Get the total price
        var totalPrice = order.GetPrice();
        Console.WriteLine($"\nTotal Price: ${totalPrice:F2}");
    }
}