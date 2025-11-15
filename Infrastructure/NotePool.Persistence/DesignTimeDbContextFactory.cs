using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NotePool.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NotePoolDbContext>
    {
        public NotePoolDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NotePoolDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
