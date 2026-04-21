namespace AwsApp.Infrastructure.Configuration;

public class S3Options
{
    public const string SectionName = "AWS:S3";

    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1";
}
