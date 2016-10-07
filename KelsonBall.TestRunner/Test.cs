using System;
using System.Linq;
using System.Reflection;

namespace KelsonBall.TestRunner
{
    public static class TestExtensions
    {
        private static readonly object testMutext = new object();

        public static void Test(this Assembly assembly)
        {
            lock(testMutext)
                foreach (object testObject in assembly
                                                    .GetTypes()
                                                    .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null)
                                                    .Select(t => Activator.CreateInstance(t)))
                    foreach (MethodInfo testMethod in testObject.GetType()
                                                                .GetMethods()
                                                                .Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null))
                        RunTestMethod(testMethod, testObject);
        }

        private static void RunTestMethod(MethodInfo method, object testObject)
        {
            bool result = true;
            Exception exception = null;
            Console.Write($"Testing {testObject.GetType().Name}, {method.Name} : ");
            try
            {
                method.Invoke(testObject, null);
            }
            catch(Exception excp)
            {
                result = false;
                exception = excp.InnerException ?? excp;
            }
            if (result)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(result ? "Pass" : "Fail");
            if (exception != null)
                Console.WriteLine(exception.ToString());
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
