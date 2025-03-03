using DesafioTjRjErlimar.Application;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioTjRjErlimar.DatabaseAdapter.TypeConfiguration;

public class AssuntoTypeConfiguration : IEntityTypeConfiguration<Assunto>
{
    public void Configure(EntityTypeBuilder<Assunto> builder)
    {
        builder
            .ToTable(nameof(Assunto))
            .HasKey(pk => pk.AssuntoId);

        builder.Property(p => p.AssuntoId).HasColumnName("CodAssunto").IsRequired();
        builder.Property(p => p.Descricao).HasColumnName("Descricao").HasMaxLength(20).IsRequired();
    }
}