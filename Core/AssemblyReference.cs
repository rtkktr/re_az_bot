using System.Reflection;

namespace Bot.Core;

internal static class AssemblyReference
{
    public static Assembly Assembly { get; } = typeof(AssemblyReference).Assembly;
}
