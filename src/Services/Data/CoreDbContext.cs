using Domain.Employees;
using Domain.Inquiries;
using Domain.Offers;
using Microsoft.EntityFrameworkCore;

namespace Services.Data;

public class CoreDbContext : DbContext
{
    public DbSet<Inquire> Inquiries { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<OfferTemplate> OfferTemplates { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureInquiries(modelBuilder);
        ConfigureOfferTemplates(modelBuilder);
        ConfigureOffers(modelBuilder);
        ConfigureEmployees(modelBuilder);
    }

    private static void ConfigureInquiries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inquire>(cfg =>
        {
            cfg.HasKey(e => e.Id);
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

    private static void ConfigureEmployees(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.UserName).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.PasswordHash).HasMaxLength(StringLengths.MediumString);
        });
    }
}
