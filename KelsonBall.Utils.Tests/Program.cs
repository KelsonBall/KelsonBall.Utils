using KelsonBall.TestRunner;
using System;
using System.Reflection;

namespace KelsonBall.Utils.Tests
{
    class Program
    {
        public static void Main()
        {
            Assembly.GetExecutingAssembly().Test();
            Console.ReadKey();
        }
    }
}
