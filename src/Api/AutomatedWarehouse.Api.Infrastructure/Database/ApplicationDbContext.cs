using AutomatedWarehouse.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AutomatedWarehouse.Api.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            if (!Database.CanConnect())
            {
                var masterConnectionString = new NpgsqlConnectionStringBuilder(configuration["Database:ConnectionString"])
                {
                    Database = "postgres" 
                }.ToString();

                using var conn = new NpgsqlConnection(masterConnectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand($"CREATE DATABASE \"{Database.GetDbConnection().Database}\" ENCODING 'UTF8' LC_COLLATE 'English_United States.1252' LC_CTYPE 'English_United States.1252' TEMPLATE template0", conn);
                cmd.ExecuteNonQuery();
            }
            Database.EnsureCreated();
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<ReceiptDocument> ReceiptDocuments { get; set; }
        public DbSet<ReceiptResource> ReceiptResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Resource>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<MeasurementUnit>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<ReceiptDocument>().HasIndex(x => x.ReceiptNumber).IsUnique();
        }
    }
}
