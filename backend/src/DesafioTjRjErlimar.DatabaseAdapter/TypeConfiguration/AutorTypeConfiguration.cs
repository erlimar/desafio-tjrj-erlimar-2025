using DesafioTjRjErlimar.Application.ManutencaoAutor;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.DatabaseAdapter.TypeConfiguration;

public class AutorTypeConfiguration : IEntityTypeConfiguration<Autor>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Autor> builder)
    {
        builder
            .ToTable(nameof(Autor))
            .HasKey(pk => pk.AutorId);

        builder.Property(p => p.AutorId).HasColumnName("CodAutor").IsRequired();
        builder.Property(p => p.Nome).HasColumnName("Nome").HasMaxLength(40).IsRequired();
    }
}