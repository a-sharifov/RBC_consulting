namespace RBC_consulting.Infrastructure.Persistense;

public sealed class DatabaseSettings
{
    public const string SectionName = "ConnectionStrings";
    public string DefaultConnection { get; set; } = string.Empty;
}
