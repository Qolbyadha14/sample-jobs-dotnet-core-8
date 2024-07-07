using Microsoft.EntityFrameworkCore;

namespace hangfire_jobs.DatabaseContext
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options) { }
    }
}
