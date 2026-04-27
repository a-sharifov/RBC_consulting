using System.Reflection;

namespace WebApp.Api;
public class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}