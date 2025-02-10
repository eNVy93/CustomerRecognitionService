using CustomerRecognitionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService
{
    public class AppDbContext: DbContext
    {
        public string DbPath { get; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MergedCustomerHistory> MergedCustomersHistory { get; set; }
        public DbSet<PendingMerge> PendingMerges { get; set; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "customer_recognition.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
