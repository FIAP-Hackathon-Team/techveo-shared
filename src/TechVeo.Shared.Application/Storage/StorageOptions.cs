namespace TechVeo.Shared.Application.Storage;

public class StorageOptions
{
    public const string SectionName = "Storage";

    public S3Options? S3 { get; set; }
}

public class S3Options
{
    public string BucketName { get; set; } = string.Empty;

    public string Region { get; set; } = "us-east-1";

    public string? AccessKey { get; set; }

    public string? SecretKey { get; set; }

    public string? ServiceUrl { get; set; }

    public bool ForcePathStyle { get; set; }
}
