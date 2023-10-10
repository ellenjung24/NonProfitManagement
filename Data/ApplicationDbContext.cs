using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NonProfitManagement.Models;

namespace NonProfitManagement.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<ContactList>().Property(m => m.AccountNo).IsRequired();
        builder.Entity<PaymentMethod>().Property(m => m.PaymentMethodId).IsRequired();
        builder.Entity<TransactionType>().Property(m => m.TransactionTypeId).IsRequired();

        builder.Entity<ContactList>().ToTable("ContactList");
        builder.Entity<Donation>().ToTable("Donation");
        builder.Entity<PaymentMethod>().ToTable("PaymentMethod");
        builder.Entity<TransactionType>().ToTable("TransactionType");
        // Use seed method here
        builder.Seed();
    }

    public DbSet<ContactList>? ContactLists { get; set; }
    public DbSet<Donation>? Donations { get; set; }
    public DbSet<PaymentMethod>? PaymentMethods { get; set; }
    public DbSet<TransactionType>? TransactionTypes { get; set; }
}
