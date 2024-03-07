using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HubFile.Models;

namespace HubFile.Data
{
    public class HubFileContext : DbContext
    {
        public HubFileContext (DbContextOptions<HubFileContext> options)
            : base(options)
        {
        }

        public DbSet<HubFile.Models.FileModel> FileModel { get; set; } = default!;
    }
}
