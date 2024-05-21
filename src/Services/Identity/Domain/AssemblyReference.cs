using System.Reflection;

namespace Domain;

public static class AssemblyReference
{
    public static readonly Assembly Identity = typeof(AssemblyReference).Assembly;
}
