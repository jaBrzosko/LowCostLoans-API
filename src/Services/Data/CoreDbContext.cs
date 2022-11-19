using Domain.Examples;
using Domain.Inquires;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Services.Data;

public class CoreDbContext : DbContext
{
    public DbSet<Example> Examples { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Inquire> Inquiries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "CoreDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureExamples(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureInquires(modelBuilder);
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
    
    private static void ConfigureInquires(ModelBuilder modelBuilder)
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
}
