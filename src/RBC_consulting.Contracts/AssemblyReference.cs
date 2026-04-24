using System.Reflection;

namespace RBC_consulting.Contracts;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
