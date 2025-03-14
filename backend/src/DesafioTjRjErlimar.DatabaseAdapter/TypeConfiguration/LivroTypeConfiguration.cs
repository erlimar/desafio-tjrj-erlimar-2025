using DesafioTjRjErlimar.Application;
using DesafioTjRjErlimar.Application.ManutencaoAutor;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioTjRjErlimar.DatabaseAdapter.TypeConfiguration;

public class LivroTypeConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder
            .ToTable(nameof(Livro))
            .HasKey(pk => pk.LivroId);

        builder.Property(p => p.LivroId).HasColumnName("CodLivro").IsRequired();
        builder.Property(p => p.Titulo).HasColumnName("Titulo").HasMaxLength(40).IsRequired();
        builder.Property(p => p.Editora).HasColumnName("Editora").HasMaxLength(40).IsRequired();
        builder.Property(p => p.Edicao).HasColumnName("Edicao").IsRequired();
        builder.Property(p => p.AnoPublicacao).HasColumnName("AnoPublicacao").IsRequired();

        builder
            .HasMany(r => r.Assuntos)
            .WithMany(r => r.Livros)
            .UsingEntity(
                "Livro_Assunto",
                right => right.HasOne(typeof(Assunto)).WithMany().HasForeignKey("Assunto_CodAssunto").HasPrincipalKey(nameof(Assunto.AssuntoId)),
                left => left.HasOne(typeof(Livro)).WithMany().HasForeignKey("Livro_CodLivro").HasPrincipalKey(nameof(Livro.LivroId)),
                fk => fk.HasKey("Assunto_CodAssunto", "Livro_CodLivro"));

        builder
            .HasMany(r => r.Autores)
            .WithMany(r => r.Livros)
            .UsingEntity(
                "Livro_Autor",
                right => right.HasOne(typeof(Autor)).WithMany().HasForeignKey("Autor_CodAutor").HasPrincipalKey(nameof(Autor.AutorId)),
                left => left.HasOne(typeof(Livro)).WithMany().HasForeignKey("Livro_CodLivro").HasPrincipalKey(nameof(Livro.LivroId)),
                fk => fk.HasKey("Autor_CodAutor", "Livro_CodLivro"));
    }
}