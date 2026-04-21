using Amazon.S3;
using AwsApp.Application.Common.Interfaces;
using AwsApp.Infrastructure.Persistence;
using AwsApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwsApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("AwsAppDb")); // Using InMemory for demo, use UseNpgsql for RDS

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        // AWS Services
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IFileService, S3FileService>();

        return services;
    }
}
