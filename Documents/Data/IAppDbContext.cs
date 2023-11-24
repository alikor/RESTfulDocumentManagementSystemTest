using Microsoft.EntityFrameworkCore;
using Documents.Models;

public interface IAppDbContext
{
    public DbSet<Document> Documents { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}