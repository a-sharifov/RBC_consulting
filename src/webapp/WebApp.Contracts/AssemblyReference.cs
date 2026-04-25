using System.Reflection;

namespace WebApp.Contracts;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
