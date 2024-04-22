using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyBankAccount.Core.Models;

namespace MyBankAccount.SQLiteAdapter
{
    public class BankAccountDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public BankAccountDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}