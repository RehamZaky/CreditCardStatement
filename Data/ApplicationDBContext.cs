using CreditCardStatementApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CreditCardStatementApi.Data
{
    public class ApplicationDBContext : IdentityDbContext<UserModel,IdentityRole,string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Statement> statements { get; set; }
    }
}
