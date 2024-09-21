namespace _01_singleton;

internal class Singleton
{
    private Singleton()
    {
    }

    private static Singleton? _instance;
    private static readonly object Lock = new();

    public static Singleton GetInstance(
        int uid,
        string name,
        string phone,
        string email,
        string address
    )
    {
        if (_instance != null) return _instance;
        lock (Lock)
        {
            _instance ??= new Singleton
            {
                Uid = uid,
                Name = name,
                Phone = phone,
                Email = email,
                Address = address
            };
        }

        return _instance;
    }

    public int Uid { get; init; }
    public string Name { get; init; }
    public string Phone { get; private init; }
    public string Email { get; private init; }
    public string Address { get; private init; }

    public override string ToString()
    {
        return
            $"Uid: {Uid}, Name: {Name}, Phone: {Phone}, Email: {Email}, Address: {Address}";
    }
}

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(
            "{0}\n{1}\n\n{2}\n",
            "If you see the same value, then singleton was reused (yay!)",
            "If you see different values, then 2 singletons were created (booo!!)",
            "RESULT:"
        );

        var process1 = new Thread(() =>
        {
            TestSingleton(
                5,
                "Foo",
                "23242423",
                "me@email.com",
                "address");
        });
        var process2 = new Thread(() =>
        {
            TestSingleton(
                5,
                "Bar",
                "46876654",
                "me@email.com",
                "address");
        });
        process1.Start();
        process2.Start();

        process1.Join();
        process2.Join();
    }

    private static void TestSingleton(
        int uid,
        string name,
        string phone,
        string email,
        string address
    )
    {
        var singleton = Singleton.GetInstance(
            uid, name, phone, email, address
        );
        Console.WriteLine(singleton.ToString());
    }
}