using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Mappings.UserContext;

/*
 * Comentário de Intenção:
 * 
 * A criação do mapeamento da entidade de domínio com o banco de dados foi criado de maneira intencional os tipos fortemente tipados
 * dos objetos de valor são convertidos no tipo primitivo persistido no banco de dados e oferecido pelo SGBD.
 */
[ExcludeFromCodeCoverage]
public sealed class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region Table Configuration

        /*
         * Comentário de Intenção:
         * 
         * Nome das tabelas sempre no plural, relacionado a entidade de negócio.
         */
        builder.ToTable("users");

        #endregion

        #region Primary Key Configuration

        /*
         * Comentário de Intenção:
         * 
         * Nome da chave primário como "pk", "nome da entidade de negócio" e "id"
         */
        builder.HasKey(p => p.Id)
            .HasName("pk_user_id");

        #endregion

        #region Index Key Configuration

        /*
         * Comentário de Intenção:
         * 
         * Os index foram criados a seguir como forma de corroborar nas operações de consulta realizadas no banco de dados. 
         * Com a criação de index key's, a consulta é realizada por meio de arvore binária, o que aumenta a performance e não realiza
         * uma operação de consulta linha a linha (o que é muito lento) no banco de dados.
         */
        builder.HasIndex(p => p.Id)
            .HasDatabaseName("uk_user_id")
            .IsUnique(true);

        builder.HasIndex(p => p.Email)
            .HasDatabaseName("uk_user_email")
            .IsUnique(true);

        #endregion

        #region Properties Configuration

        /*
        * Comentário de Intenção:
        * 
        * As propriedades foram definidas e configuradas conforme a regra de negócio manda e sua intenção.
        */
        builder.Property(p => p.Id)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasMaxLength(IdValueObject.LENGTH)
            .HasColumnType("UUID")
            .HasConversion(p => p.GetValue(), p => IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("iduser")
            .ValueGeneratedNever();
        builder.Property(p => p.Name)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasMaxLength(UserNameValueObject.MAX_LENGTH)
            .HasColumnType("VARCHAR")
            .HasConversion(p => p.GetValue(), p => UserNameValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
            .HasColumnName("name")
            .ValueGeneratedNever();
        builder.Property(p => p.Email)
           .IsRequired(true)
           .IsFixedLength(false)
           .HasMaxLength(EmailValueObject.MAX_LENGTH)
           .HasColumnType("VARCHAR")
           .HasConversion(p => p.GetValue(), p => EmailValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
           .HasColumnName("email")
           .ValueGeneratedNever();
        builder.Property(p => p.BrokerageFee)
           .IsRequired(true)
           .IsFixedLength(false)
           .HasColumnType($"DECIMAL(9, 8)")
           .HasConversion(p => p.GetValue(), p => BrokerageFeeValueObject.UnsafeBuildFromTrustedDatabaseRecord(p))
           .HasColumnName("brokerage_fee")
           .ValueGeneratedNever();

        #endregion
    }
}
