namespace _01_singleton;
// The Singleton class defines the `GetInstance` method that serves as an
// alternative to constructor and lets clients access the same instance of
// this class over and over.

// EN: The Singleton should always be a 'sealed' class to prevent class
// inheritance through external classes and also through nested classes.
public sealed class Singleton
{
    // The Singleton's constructor should always be private to prevent
    // direct construction calls with the `new` operator.
    private Singleton() { }

    // The Singleton's instance is stored in a static field. There are
    // multiple ways to initialize this field, all of them have various pros
    // and cons. In this example, we'll show the simplest of these ways,
    // which, however, doesn't work really well in multithreaded program.
    private static Singleton? _instance;

    // This is the static method that controls the access to the singleton
    // instance. On the first run, it creates a singleton object and places
    // it into the static field. On later runs, it returns the client
    // existing object stored in the static field.
    public static Singleton? GetInstance()
    {
        return _instance ??= new Singleton();
    }

    // Finally, any singleton should define some business logic, which can
    // be executed on its instance.
    public void SomeBusinessLogic()
    {
        // ...
    }
}

internal static class Program
{
    public static void Main(string[] args)
    {
        var s1 = Singleton.GetInstance();
        var s2 = Singleton.GetInstance();

        Console.WriteLine(s1 == s2
            ? "Singleton works, both variables contain the same instance."
            : "Singleton failed, variables contain different instances.");
    }
}