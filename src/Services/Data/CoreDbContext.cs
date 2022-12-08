using Domain.Examples;
using Domain.Inquiries;
using Domain.Offers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Services.Data;

public class CoreDbContext : DbContext
{
    public DbSet<Example> Examples { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Inquire> Inquiries { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<OfferTemplate> OfferTemplates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "CoreDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureExamples(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureInquiries(modelBuilder);
        ConfigureOfferTemplates(modelBuilder);
        ConfigureOffers(modelBuilder);
    }

    private static void ConfigureExamples(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Example>().HasKey(e => e.Id);
        modelBuilder.Entity<Example>().Property(e => e.Name).HasMaxLength(StringLengths.ShortString);
    }
    
    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.OwnsOne(e => e.PersonalData, inner =>
            {
                inner.Property(e => e.FirstName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.LastName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.GovernmentId).HasMaxLength(StringLengths.MediumString);
                inner.Property(e => e.GovernmentIdType);
                inner.Property(e => e.JobType);
            });
        });
    }
    
    private static void ConfigureInquiries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inquire>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.UserId);
            cfg.Property(e => e.MoneyInSmallestUnit);
            cfg.Property(e => e.NumberOfInstallments);
            cfg.Property(e => e.CreationTime);
            cfg.OwnsOne(e => e.PersonalData, inner =>
            {
                inner.Property(e => e.FirstName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.LastName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.GovernmentId).HasMaxLength(StringLengths.MediumString);
                inner.Property(e => e.GovernmentIdType);
                inner.Property(e => e.JobType);
            });
        });
    }
    
    private static void ConfigureOfferTemplates(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OfferTemplate>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.MinimumMoneyInSmallestUnit);
            cfg.Property(e => e.MaximumMoneyInSmallestUnit);
            cfg.Property(e => e.MinimumNumberOfInstallments);
            cfg.Property(e => e.MaximumNumberOfInstallments);
            cfg.Property(e => e.CreationTime);
        });
    }
    private static void ConfigureOffers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.InquireId);
            cfg.Property(e => e.NumberOfInstallments);
            cfg.Property(e => e.MoneyInSmallestUnit);
            cfg.Property(e => e.InterestRate);
            cfg.Property(e => e.CreationTime);
            cfg.Property(e => e.Status);
        });
    }
}
