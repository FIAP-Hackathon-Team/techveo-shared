namespace TechVeo.Shared.Application.Aws;

public class AwsOptions
{
    public const string SectionName = "Aws";

    public string? Region { get; set; } = "us-east-1";

    public string? AccessKey { get; set; }

    public string? SecretKey { get; set; }

    public string? SessionToken { get; set; }
}
