using Amazon.S3;
using Amazon.S3.Transfer;
using AwsApp.Application.Common.Interfaces;
using AwsApp.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace AwsApp.Infrastructure.Services;

public class S3FileService(IAmazonS3 s3Client, IOptions<S3Options> options) : IFileService
{
    private readonly S3Options _options = options.Value;

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _options.BucketName
        };

        using var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{_options.BucketName}.s3.{_options.Region}.amazonaws.com/{fileName}";
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var response = await s3Client.GetObjectAsync(_options.BucketName, fileName);
        using (response)
        {
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
