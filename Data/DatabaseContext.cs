using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Server
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<ValidationToken> ValidationTokens { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options){ }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserWallet>()
                .Property(p => p.UserId)
                .ValueGeneratedNever();
            builder.Entity<Transaction>()
                .Property(p => p.Id)
                .ValueGeneratedNever();
            base.OnModelCreating(builder);
        }
    }

    public class DatabaseContextFactory
    {
        private readonly DbContextOptions<DatabaseContext> options;

        public DatabaseContextFactory(DbContextOptionsBuilder<DatabaseContext> builder)
        {
            options = builder.Options;
        }
        public DatabaseContext CreateContext()
        {
            return new DatabaseContext(options);
        }
    }
}