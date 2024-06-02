using System.Reflection;

namespace Domain;

public class AssemblyReference
{
    public static Assembly Assembly = typeof(AssemblyReference).Assembly;
}