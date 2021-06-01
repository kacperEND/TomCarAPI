using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<SqlServerDBContext>
    {
        public SqlServerDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerDBContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TomCarDev;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new SqlServerDBContext(optionsBuilder.Options);
        }
    }
}