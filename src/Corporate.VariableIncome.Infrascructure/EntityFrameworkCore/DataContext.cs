using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.AssetContext;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.OperationContext;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.PositionContext;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.QuotationContext;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.UserContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public sealed class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Asset> Assets { get; set; }

    public DbSet<Operation> Operations { get; set; }

    public DbSet<PositionSnapshot> PositionSnapshots { get; set; }

    public DbSet<Quotation> Quotations { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());

        modelBuilder.ApplyConfiguration(new AssetMapping());

        modelBuilder.ApplyConfiguration(new OperationMapping());

        modelBuilder.ApplyConfiguration(new PositionSnapshotMapping());

        modelBuilder.ApplyConfiguration(new QuotationMapping());

        base.OnModelCreating(modelBuilder);
    }
}
