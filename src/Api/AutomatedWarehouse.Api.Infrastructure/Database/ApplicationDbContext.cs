using AutomatedWarehouse.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<ReceiptDocument> ReceiptDocuments { get; set; }
        public DbSet<ReceiptResource> ReceiptResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ReceiptDocument>()
                .HasMany<ReceiptResource>()
                .WithOne()
                .HasForeignKey(x => x.ReceiptDocumentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MeasurementUnit>()
                .HasMany<ReceiptResource>()
                .WithOne()
                .HasForeignKey(x => x.MeasurementUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Resource>()
                .HasMany<ReceiptResource>()
                .WithOne()
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Resource>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<MeasurementUnit>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<ReceiptDocument>().HasIndex(x => x.ReceiptNumber).IsUnique();
        }
    }
}
