public class Program
{
    public static async Task Main( string[] args )
    {
        Console.WriteLine("Program started...");
        Console.ReadKey();

        await Method1();
        await Method2();

        Console.WriteLine("Program is running...");
        var first = Method1();
        var second = Method2();

        await first;
        await second;

        Console.ReadKey();
    }

    public static async Task Method1()
    {
        await Task.Delay(2000);
        Console.WriteLine("Method 1");
    }

    public static async Task Method2()
    {
        Console.WriteLine("Method 2");
    }
}
