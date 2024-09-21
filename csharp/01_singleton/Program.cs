namespace _01_singleton
{
    // This Singleton implementation is called "double check lock". It is safe
    // in multithreaded environment and provides lazy initialization for the
    // Singleton object.
    internal class Singleton
    {
        private Singleton() { }

        private static Singleton? _instance;

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object Lock = new object();

        public static Singleton? GetInstance(string? value)
        {
            // This conditional is necessary to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance != null) return _instance;
            // Now, imagine that the program has just been launched. Since
            // there's no Singleton instance yet, multiple threads can
            // simultaneously pass the previous conditional and reach this
            // point almost at the same time. The first of them will acquire
            // lock and will proceed further, while the rest will wait here.
            lock (Lock)
            {
                // The first thread to acquire the lock, reaches this
                // conditional, goes inside and creates the Singleton
                // instance. Once it leaves the lock block, a thread that
                // might have been waiting for the lock release may then
                // enter this section. But since the Singleton field is
                // already initialized, the thread won't create a new
                // object.
                _instance ??= new Singleton
                {
                    Value = value
                };
            }
            return _instance;
        }

        // We'll use this property to prove that our Singleton really works.
        public string? Value { get; private init; }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            // The client code.
            
            Console.WriteLine(
                "{0}\n{1}\n\n{2}\n",
                "If you see the same value, then singleton was reused (yay!)",
                "If you see different values, then 2 singletons were created (booo!!)",
                "RESULT:"
            );
            
            var process1 = new Thread(() =>
            {
                TestSingleton("FOO");
            });
            var process2 = new Thread(() =>
            {
                TestSingleton("BAR");
            });
            
            process1.Start();
            process2.Start();
            
            process1.Join();
            process2.Join();
        }

        private static void TestSingleton(string? value)
        {
            var singleton = Singleton.GetInstance(value);
            Console.WriteLine(singleton?.Value);
        } 
    }
}