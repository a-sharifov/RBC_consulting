using System.Reflection;
using WebApp.Domain.Common.Enumerations.Interfaces;

namespace WebApp.Domain.Common.Enumerations;

public abstract class Enumeration<TEnum>(int index, string name) : IEnumeration
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> _enumerations = GetEnumerations();

    public int Index { get; private set; } = index;
    public string Name { get; private set; } = name;

    public static TEnum? FromValueOrDefault(int index) =>
        _enumerations.TryGetValue(index, out var enumeration) ? enumeration : null;

    public static TEnum FromName(string name) =>
        _enumerations.Values.Single(x => x.Name == name);

    public static TEnum? FromNameOrDefault(string? name) =>
        _enumerations.Values.SingleOrDefault(x => x.Name == name);

    public static IEnumerable<string> GetNames() =>
        _enumerations.Values.Select(x => x.Name);

    public static bool NameExists(string name) =>
        _enumerations.Values.Any(x => x.Name == name);

    public static bool ValueExists(int index) =>
        _enumerations.ContainsKey(index);

    private static Dictionary<int, TEnum> GetEnumerations()
    {
        var enumerationType = typeof(TEnum);

        return enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!)
            .ToDictionary(field => field.Index);
    }

    public static implicit operator int(Enumeration<TEnum> @enum) => @enum.Index;
    public static implicit operator string(Enumeration<TEnum> @enum) => @enum.Name;

    public static bool operator ==(Enumeration<TEnum>? left, Enumeration<TEnum>? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Enumeration<TEnum>? left, Enumeration<TEnum>? right) =>
        !(left == right);

    public override bool Equals(object? obj) =>
        obj is Enumeration<TEnum> other && Index.Equals(other.Index);

    public override int GetHashCode() => Index.GetHashCode();
}
