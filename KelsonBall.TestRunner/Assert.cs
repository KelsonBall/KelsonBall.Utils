using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KelsonBall.TestRunner
{
    internal class AssertFailedException : Exception
    {
        public AssertFailedException() : base() { }
        public AssertFailedException(string message) : base(message) { }
    }

    [DebuggerStepThrough]
    public static class Assert
    {
        public static void True(bool condition, string message = null, [CallerMemberName] string caller = null)
        {
            if (!condition)
                throw new AssertFailedException(message ?? $"{caller ?? ""} Assertion failed.");
        }
    }
}