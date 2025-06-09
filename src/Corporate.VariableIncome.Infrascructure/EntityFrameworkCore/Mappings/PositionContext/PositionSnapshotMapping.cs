using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.PositionContext;

/*
 * Comentário de Intenção:
 * 
 * O mapeamento da entidade de domínio PositionSnapshot segue o mesmo padrão de rigidez e clareza
 * adotado em todo o projeto, utilizando objetos de valor para expressar as regras de negócio
 * com segurança e imutabilidade. 
 * A tabela representa a projeção (snapshot) da posição consolidada de um ativo
 * por um determinado usuário, e deve refletir precisamente o estado após cada operação de atualização.
 */
[ExcludeFromCodeCoverage]
public sealed class PositionSnapshotMapping : IEntityTypeConfiguration<PositionSnapshot>
{
    public void Configure(EntityTypeBuilder<PositionSnapshot> builder)
    {
        #region Table Configuration

        builder.ToTable("position_snapshots");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(p => p.Id)
            .HasName("pk_position_snapshot_id");

        #endregion

        #region Index Configuration

        builder.HasIndex(p => p.Id)
            .HasDatabaseName("uk_position_snapshot_id")
            .IsUnique(true);

        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("idx_position_snapshot_user_id");
        builder.HasIndex(p => p.AssetId)
            .HasDatabaseName("idx_position_snapshot_asset_id");

        builder.HasIndex(p => p.DateTime)
            .HasDatabaseName("idx_position_snapshot_datetime")
            .IsDescending(true);

        #endregion

        #region Properties Configuration

        builder.Property(p => p.Id)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("UUID")
            .HasMaxLength(IdValueObject.LENGTH)
            .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("idpositionsnapshot")
            .ValueGeneratedNever();

        builder.Property(p => p.AssetId)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("UUID")
            .HasMaxLength(IdValueObject.LENGTH)
            .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("asset_id")
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("UUID")
            .HasMaxLength(IdValueObject.LENGTH)
            .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder.Property(p => p.Quantity)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("INTEGER")
            .HasConversion(p => p.GetValue(), p => QuantityValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("quantity")
            .ValueGeneratedNever();

        builder.Property(p => p.AveragePrice)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("DECIMAL(16,2)")
            .HasConversion(p => p.GetValue(), p => PriceValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("average_price")
            .ValueGeneratedNever();

        builder.Property(p => p.DateTime)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("snapshot_datetime")
            .HasConversion(p => p.GetValue(), p => DateTimeValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .ValueGeneratedNever();

        builder.Property(p => p.ProftAndLoss)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("DECIMAL(20,6)")
            .HasConversion(p => p.GetValue(), p => ProftAndLossValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("proft_and_loss")
            .ValueGeneratedNever();

        #endregion

        #region Relationships

        builder.HasOne(p => p.Relationship1AssetNPositionSnapshots)
            .WithMany(p => p.PositionSnapshots)
            .HasForeignKey(p => p.AssetId)
            .HasPrincipalKey(p => p.Id)
            .HasConstraintName("fk_position_snapshot_asset_id")
            .IsRequired(true);

        builder.HasOne(p => p.Relationship1UserNPositionSnapshots)
            .WithMany(p => p.PositionSnapshots)
            .HasForeignKey(p => p.UserId)
            .HasPrincipalKey(p => p.Id)
            .HasConstraintName("fk_position_snapshot_user_id")
            .IsRequired(true);

        #endregion
    }
}
