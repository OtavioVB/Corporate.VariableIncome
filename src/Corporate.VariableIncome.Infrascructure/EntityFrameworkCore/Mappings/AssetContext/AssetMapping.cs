using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.AssetContext;

[ExcludeFromCodeCoverage]
public sealed class AssetMapping : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        #region Table Configuration

        builder.ToTable("assets");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(p => p.Id)
            .HasName("pk_asset_id");

        #endregion

        #region Indexes

        builder.HasIndex(p => p.Id)
            .HasDatabaseName("uk_asset_id")
            .IsUnique(true);

        builder.HasIndex(p => p.Code)
            .HasDatabaseName("uk_asset_code")
            .IsUnique(true);

        #endregion

        #region Properties Configuration

        builder.Property(p => p.Id)
            .IsRequired()
            .IsFixedLength(true)
            .HasMaxLength(IdValueObject.LENGTH)
            .HasColumnType("UUID")
            .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("idasset")
            .ValueGeneratedNever();

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(AssetCodeValueObject.MAX_LENGTH)
            .HasColumnType("VARCHAR")
            .HasConversion(p => p.GetValue(), p => AssetCodeValueObject.Build(p))
            .HasColumnName("code")
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(AssetNameValueObject.MAX_LENGTH)
            .HasColumnType("VARCHAR")
            .HasConversion(p => p.GetValue(), p => AssetNameValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("name")
            .ValueGeneratedNever();

        #endregion
    }
}