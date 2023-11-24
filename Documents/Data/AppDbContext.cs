

using Documents.Models;
using Microsoft.EntityFrameworkCore;

namespace Documents.Data
{

    // Step 2: Create a DbContext class
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Document> Documents { get; set; }

        public string DbPath { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "app.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }

}