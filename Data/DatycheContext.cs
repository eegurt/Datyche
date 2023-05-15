using Microsoft.EntityFrameworkCore;
using Datyche.Models;

namespace Datyche.Data
{
    public class DatycheContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;

        public DatycheContext(DbContextOptions<DatycheContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}