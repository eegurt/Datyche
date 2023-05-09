using Microsoft.EntityFrameworkCore;

namespace Datyche.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            
        }
    }
}