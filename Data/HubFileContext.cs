using Microsoft.EntityFrameworkCore;

namespace HubFile.Data
{
    public class HubFileContext : DbContext
    {
        public HubFileContext(DbContextOptions<HubFileContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<HubFile.Models.FileModel> FileModel { get; set; } = default!;
    }
}
