using System;

namespace ConsoleApp_Pokemon;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Pokemon Console App - Debug Tests\n");
        DebugTest.TestCase3();
        DebugTest.TestCase4();
        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
    }
}
