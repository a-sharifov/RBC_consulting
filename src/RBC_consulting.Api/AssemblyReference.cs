using System.Reflection;

namespace RBC_consulting;
public class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}