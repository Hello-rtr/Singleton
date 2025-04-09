using System;
using System.Threading;

namespace SingletonPattern
{
    // 1. Базовая (непотокобезопасная) версия
    public class BasicSingleton
    {
        private static BasicSingleton _instance;

        // Приватный конструктор
        private BasicSingleton()
        {
            Console.WriteLine("BasicSingleton instance created");
        }

        // Публичное свойство для доступа к экземпляру
        public static BasicSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BasicSingleton();
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            Console.WriteLine($"[BasicSingleton] {message}");
        }
    }

    // 2. Потокобезопасная версия с lock
    public class ThreadSafeSingleton
    {
        private static ThreadSafeSingleton _instance;
        private static readonly object _lock = new object();

        private ThreadSafeSingleton()
        {
            Console.WriteLine("ThreadSafeSingleton instance created");
        }

        public static ThreadSafeSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThreadSafeSingleton();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            Console.WriteLine($"[ThreadSafeSingleton] {message}");
        }
    }

    // 3. Потокобезопасная версия с Lazy<T> (рекомендуемый способ)
    public class LazySingleton
    {
        private static readonly Lazy<LazySingleton> _lazy =
            new Lazy<LazySingleton>(() => new LazySingleton());

        private LazySingleton()
        {
            Console.WriteLine("LazySingleton instance created");
        }

        public static LazySingleton Instance => _lazy.Value;

        public void Log(string message)
        {
            Console.WriteLine($"[LazySingleton] {message}");
        }
    }

    // 4. Потокобезопасная версия со статическим конструктором
    public class StaticConstructorSingleton
    {
        private static readonly StaticConstructorSingleton _instance;

        // Статический конструктор выполняется только один раз
        static StaticConstructorSingleton()
        {
            _instance = new StaticConstructorSingleton();
            Console.WriteLine("StaticConstructorSingleton instance created");
        }

        private StaticConstructorSingleton() { }

        public static StaticConstructorSingleton Instance => _instance;

        public void Log(string message)
        {
            Console.WriteLine($"[StaticConstructorSingleton] {message}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing BasicSingleton ===");
            var basic1 = BasicSingleton.Instance;
            var basic2 = BasicSingleton.Instance;
            Console.WriteLine($"Same instance? {ReferenceEquals(basic1, basic2)}");
            basic1.Log("Hello from BasicSingleton");

            Console.WriteLine("\n=== Testing ThreadSafeSingleton ===");
            Parallel.For(0, 5, i => {
                var threadSafe = ThreadSafeSingleton.Instance;
                threadSafe.Log($"Message from thread {i}");
            });

            Console.WriteLine("\n=== Testing LazySingleton ===");
            var lazy1 = LazySingleton.Instance;
            var lazy2 = LazySingleton.Instance;
            Console.WriteLine($"Same instance? {ReferenceEquals(lazy1, lazy2)}");
            lazy1.Log("Hello from LazySingleton");

            Console.WriteLine("\n=== Testing StaticConstructorSingleton ===");
            var static1 = StaticConstructorSingleton.Instance;
            var static2 = StaticConstructorSingleton.Instance;
            Console.WriteLine($"Same instance? {ReferenceEquals(static1, static2)}");
            static1.Log("Hello from StaticConstructorSingleton");
        }
    }
}