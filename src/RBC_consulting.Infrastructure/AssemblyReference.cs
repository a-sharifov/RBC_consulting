using System.Reflection;

namespace RBC_consulting.Infrastructure;

public class AssemblyReference
{
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
