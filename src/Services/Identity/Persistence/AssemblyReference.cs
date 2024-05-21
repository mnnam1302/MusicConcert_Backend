using System.Reflection;

namespace Persistence;

public static class AssemblyReference
{
    public static readonly Assembly Identity = typeof(AssemblyReference).Assembly;
}