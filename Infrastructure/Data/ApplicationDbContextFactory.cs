using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<SqlDBContext>
    {
        public SqlDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlDBContext>();
            optionsBuilder.UseNpgsql("");
            return new SqlDBContext(optionsBuilder.Options);
        }
    }
}