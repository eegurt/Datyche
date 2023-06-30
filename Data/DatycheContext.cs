using Microsoft.EntityFrameworkCore;
using Datyche.Models;

namespace Datyche.Data
{
    public class DatycheContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Datyche.Models.File> Files => Set<Datyche.Models.File>();

        public DatycheContext(DbContextOptions<DatycheContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}