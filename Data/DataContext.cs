using Microsoft.EntityFrameworkCore;

namespace TRYWEBAPI.Data {
    public class DataContext : DbContext {

        public DataContext(DbContextOptions<DataContext> options) : base (options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Account>()
                    .HasIndex(p => new { p.Email, p.Username })
                    .IsUnique(true);
            }
     
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}