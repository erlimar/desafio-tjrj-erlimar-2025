IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE TABLE [Assunto] (
        [CodAssunto] int NOT NULL IDENTITY,
        [Descricao] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Assunto] PRIMARY KEY ([CodAssunto])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE TABLE [Autor] (
        [CodAutor] int NOT NULL IDENTITY,
        [Nome] nvarchar(40) NOT NULL,
        CONSTRAINT [PK_Autor] PRIMARY KEY ([CodAutor])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE TABLE [Livro] (
        [CodLivro] int NOT NULL IDENTITY,
        [Titulo] nvarchar(40) NOT NULL,
        [Editora] nvarchar(40) NOT NULL,
        [Edicao] int NOT NULL,
        [AnoPublicacao] int NOT NULL,
        CONSTRAINT [PK_Livro] PRIMARY KEY ([CodLivro])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE TABLE [Livro_Assunto] (
        [Assunto_CodAssunto] int NOT NULL,
        [Livro_CodLivro] int NOT NULL,
        CONSTRAINT [PK_Livro_Assunto] PRIMARY KEY ([Assunto_CodAssunto], [Livro_CodLivro]),
        CONSTRAINT [FK_Livro_Assunto_Assunto_Assunto_CodAssunto] FOREIGN KEY ([Assunto_CodAssunto]) REFERENCES [Assunto] ([CodAssunto]) ON DELETE CASCADE,
        CONSTRAINT [FK_Livro_Assunto_Livro_Livro_CodLivro] FOREIGN KEY ([Livro_CodLivro]) REFERENCES [Livro] ([CodLivro]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE TABLE [Livro_Autor] (
        [Autor_CodAutor] int NOT NULL,
        [Livro_CodLivro] int NOT NULL,
        CONSTRAINT [PK_Livro_Autor] PRIMARY KEY ([Autor_CodAutor], [Livro_CodLivro]),
        CONSTRAINT [FK_Livro_Autor_Autor_Autor_CodAutor] FOREIGN KEY ([Autor_CodAutor]) REFERENCES [Autor] ([CodAutor]) ON DELETE CASCADE,
        CONSTRAINT [FK_Livro_Autor_Livro_Livro_CodLivro] FOREIGN KEY ([Livro_CodLivro]) REFERENCES [Livro] ([CodLivro]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE INDEX [IX_Livro_Assunto_Livro_CodLivro] ON [Livro_Assunto] ([Livro_CodLivro]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    CREATE INDEX [IX_Livro_Autor_Livro_CodLivro] ON [Livro_Autor] ([Livro_CodLivro]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250302094038_CriaModeloInicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250302094038_CriaModeloInicial', N'9.0.2');
END;

COMMIT;
GO

