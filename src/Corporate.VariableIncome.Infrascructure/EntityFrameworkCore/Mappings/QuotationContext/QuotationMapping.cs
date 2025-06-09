using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.QuotationContext;

/*
 * Comentário de Intenção:
 * 
 * Este mapeamento traduz a entidade Quotation, que representa a cotação unitária
 * de um ativo em um dado momento no tempo. Toda cotação está associada a um ativo (Asset).
 * As regras de domínio são aplicadas via objetos de valor, garantindo imutabilidade,
 * consistência e segurança de persistência.
 */
[ExcludeFromCodeCoverage]
public sealed class QuotationMapping : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        #region Table Configuration

        builder.ToTable("quotations");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(q => q.Id)
               .HasName("pk_quotation_id");

        #endregion

        #region Index Configuration

        builder.HasIndex(q => q.Id)
               .HasDatabaseName("uk_quotation_id")
               .IsUnique();

        builder.HasIndex(q => new { q.AssetId, q.DateTime })
               .HasDatabaseName("idx_quotation_asset_datetime");

        builder.HasIndex(q => q.DateTime)
            .HasDatabaseName("idx_quotation_datetime")
            .IsDescending([true]);

        #endregion

        #region Properties Configuration

        builder.Property(q => q.Id)
               .IsRequired(true)
               .IsFixedLength(true)
               .HasColumnType("UUID")
               .HasMaxLength(IdValueObject.LENGTH)
               .HasConversion(q => q.GetValue(), q => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(q))
               .HasColumnName("idquotation")
               .ValueGeneratedNever();
        builder.Property(q => q.AssetId)
               .IsRequired(true)
               .IsFixedLength(true)
               .HasColumnType("UUID")
               .HasMaxLength(IdValueObject.LENGTH)
               .HasConversion(q => q.GetValue(), q => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(q))
               .HasColumnName("asset_id")
               .ValueGeneratedNever();

        builder.Property(q => q.UnitaryPrice)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("DECIMAL(18,2)")
               .HasConversion(q => q.GetValue(), q => PriceValueObject.UnsafeBuildFromTrustedDatabaseRecord(q))
               .HasColumnName("unitary_price")
               .ValueGeneratedNever();

        builder.Property(q => q.DateTime)
               .IsRequired(true)
               .IsFixedLength(true)
               .HasColumnType("TIMESTAMPTZ")
               .HasConversion(q => q.GetValue(), q => DateTimeValueObject.UnsafeBuildFromTrustedDatabaseRecord(q))
               .HasColumnName("quotation_date")
               .ValueGeneratedNever();

        #endregion

        #region Relationships

        builder.HasOne(q => q.Relationship1AssetNQuotations)
               .WithMany(p => p.Quotations)
               .HasPrincipalKey(p => p.Id)
               .HasForeignKey(q => q.AssetId)
               .HasConstraintName("fk_quotation_asset_id")
               .IsRequired(true);

        #endregion
    }
}