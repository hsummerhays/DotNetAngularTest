using AwsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwsApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
