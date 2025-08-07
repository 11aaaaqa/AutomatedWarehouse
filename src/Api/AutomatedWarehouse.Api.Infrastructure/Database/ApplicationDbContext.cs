using AutomatedWarehouse.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AutomatedWarehouse.Api.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            try
            {
                Database.OpenConnection();
            }
            catch (PostgresException ex) when(ex.SqlState == "3D000")
            {
                var masterConnectionString = new NpgsqlConnectionStringBuilder(configuration["Database:ConnectionString"])
                {
                    Database = "postgres"
                }.ToString();

                using var conn = new NpgsqlConnection(masterConnectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand($"CREATE DATABASE \"{Database.GetDbConnection().Database}\" ENCODING 'UTF8' LC_COLLATE 'en_US.UTF-8' LC_CTYPE 'en_US.UTF-8' TEMPLATE template0", conn);
                cmd.ExecuteNonQuery();

                Database.Migrate();
            }
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
