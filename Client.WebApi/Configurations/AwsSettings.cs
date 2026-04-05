namespace Client.WebApi.Configurations;

public class AwsSettings
{
    public const string SectionName = "AWS";

    public string Region { get; set; } = string.Empty;
    public string? Profile { get; set; }
    public string? ServiceURL { get; set; }
    public string BucketName { get; set; } = string.Empty;
}
