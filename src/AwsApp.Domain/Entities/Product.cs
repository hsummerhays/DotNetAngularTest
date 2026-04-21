using AwsApp.Domain.Common;

namespace AwsApp.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? S3ImageUrl { get; set; } // Reference to an AWS S3 asset
}
