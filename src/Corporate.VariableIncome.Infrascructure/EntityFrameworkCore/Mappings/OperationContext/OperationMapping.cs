using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.OperationContext;

/*
* Comentário de Intenção:
* 
* Mapeamento da entidade Operation. A utilização de objetos de valor visa garantir consistência e integridade
* dos dados desde a entrada até a persistência. Todas as propriedades com valor semântico de negócio
* estão fortemente tipadas e convertidas corretamente para tipos primitivos aceitos pelo SGBD.
*/
[ExcludeFromCodeCoverage]
public sealed class OperationMapping : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        #region Table Configuration

        builder.ToTable("operations");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(p => p.Id)
               .HasName("pk_operation_id");

        #endregion

        #region Index Configuration

        builder.HasIndex(p => p.Id)
               .HasDatabaseName("uk_operation_id")
               .IsUnique(true);

        builder.HasIndex(p => p.AssetId)
            .HasDatabaseName("idx_operation_asset_id");
        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("idx_operation_user_id");
        builder.HasIndex(p => p.DateTime)
            .HasDatabaseName("idx_operation_date_time")
            .IsDescending(true);

        #endregion

        #region Properties Configuration

        builder.Property(p => p.Id)
               .IsRequired(true)
               .IsFixedLength(true)
               .HasColumnType("UUID")
               .HasMaxLength(IdValueObject.LENGTH)
               .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("idoperation")
               .ValueGeneratedNever();
        builder.Property(p => p.UnitaryPrice)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("DECIMAL(16,2)")
               .HasConversion(p => p.GetValue(), p => PriceValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("unitary_price")
               .ValueGeneratedNever();
        builder.Property(p => p.Quantity)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("INTEGER")
               .HasConversion(p => p.GetValue(), p => QuantityValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("quantity")
               .ValueGeneratedNever();
        builder.Property(p => p.BrokerageFee)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("DECIMAL(20,6)")
               .HasConversion(p => p.GetValue(), p => BrokerageFeeValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("brokerage_fee")
               .ValueGeneratedNever();
        builder.Property(p => p.DateTime)
               .IsRequired(true)
               .IsFixedLength(true)
               .HasColumnType("TIMESTAMPTZ")
               .HasConversion(p => p.GetValue(), p => DateTimeValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("operation_date")
               .ValueGeneratedNever();
        builder.Property(p => p.AssetId)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("UUID")
               .HasMaxLength(IdValueObject.LENGTH)
               .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("asset_id")
               .ValueGeneratedNever();
        builder.Property(p => p.UserId)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("UUID")
               .HasMaxLength(IdValueObject.LENGTH)
               .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
               .HasColumnName("user_id")
               .ValueGeneratedNever();
        builder.Property(p => p.Type)
               .IsRequired(true)
               .IsFixedLength(false)
               .HasColumnType("VARCHAR")
               .HasMaxLength(255)
               .HasConversion(p => p.GetValueAsString(), p => TypeOperationValueObject.UnsafeBuildFromTrustedDatabaseRecord(Enum.Parse<EnumTypeOperation>(p)))
               .HasColumnName("type")
               .ValueGeneratedNever();

        #endregion

        #region Relationships

        builder.HasOne(p => p.Relationship1AssetNOperations)
               .WithMany(p => p.Operations)
               .HasForeignKey(p => p.AssetId)
               .HasPrincipalKey(p => p.Id)
               .HasConstraintName("fk_operation_asset_id")
               .IsRequired(true);

        builder.HasOne(p => p.Relationship1UserNOperations)
               .WithMany(p => p.Operations)
               .HasForeignKey(p => p.UserId)
               .HasPrincipalKey(p => p.Id)
               .HasConstraintName("fk_operation_user_id")
               .IsRequired(true);

        #endregion
    }
}
