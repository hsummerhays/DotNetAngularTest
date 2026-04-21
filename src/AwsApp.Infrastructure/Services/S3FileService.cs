using Amazon.S3;
using Amazon.S3.Transfer;
using AwsApp.Application.Common.Interfaces;

namespace AwsApp.Infrastructure.Services;

public class S3FileService(IAmazonS3 s3Client) : IFileService
{
    private const string BucketName = "my-aws-app-bucket"; // Should be in config

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = BucketName,
            CannedACL = S3CannedACL.PublicRead
        };

        var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{BucketName}.s3.amazonaws.com/{fileName}";
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var response = await s3Client.GetObjectAsync(BucketName, fileName);
        return response.ResponseStream;
    }
}
