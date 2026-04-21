using AwsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AwsApp.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // For real DBs, we would use _context.Database.MigrateAsync();
            // For InMemory, we just ensure it's created
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Products.Any())
        {
            _context.Products.AddRange(
                new Product { Name = "AWS Cloud Practitioner Guide", Description = "A comprehensive guide to AWS fundamentals.", Price = 24.99m },
                new Product { Name = "AWS DeepRacer Pro", Description = "Autonomous racing car for machine learning enthusiasts.", Price = 399.00m },
                new Product { Name = "AWS Snowball Edge", Description = "Data migration and edge computing device.", Price = 250.00m },
                new Product { Name = "S3 Buckets (Premium)", Description = "Infinite storage for your application assets.", Price = 1.99m },
                new Product { Name = "Lambda Rocket", Description = "High-speed serverless execution for your APIs.", Price = 0.01m }
            );

            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
