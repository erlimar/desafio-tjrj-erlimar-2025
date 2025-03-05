CREATE OR ALTER VIEW dbo.RelatorioConsolidadoPorAutor
AS
SELECT
  A1.CodAutor         AutorPrincipalCodAutor,
  A1.Nome             AutorPrincipalNome,
  L1.CodLivro         LivroCodLivro,
  L1.Titulo           LivroTitulo,
  L1.Editora          LivroEditora,
  L1.Edicao           LivroEdicao,
  L1.AnoPublicacao    LivroAnoPublicacao,
  A2.CodAutor         AutorSecundarioCodAutor,
  A2.Nome             AutorSecundarioNome,
  S1.CodAssunto       AssuntoCodAssunto,
  S1.Descricao        AssuntoDescricao
  
FROM dbo.Autor A1
	INNER JOIN dbo.Livro_Autor LA1    ON LA1.Autor_CodAutor = A1.CodAutor
	INNER JOIN dbo.Livro L1           ON L1.CodLivro = LA1.Livro_CodLivro
	INNER JOIN dbo.Livro_Autor LA2    ON LA2.Livro_CodLivro = L1.CodLivro
	INNER JOIN dbo.Autor A2           ON A2.CodAutor = LA2.Autor_CodAutor
	INNER JOIN dbo.Livro_Assunto LS1  ON LS1.Livro_CodLivro = LA2.Livro_CodLivro
	INNER JOIN dbo.Assunto S1         ON S1.CodAssunto = LS1.Assunto_CodAssunto

WHERE A1.CodAutor <> A2.CodAutor;

--  (  A1 )                                 (    LA1    )                                 (  L1 )                                 (    LA2    )                                 ( A2  ) 
-- [(Autor).CodAutor] <---> [Autor_CodAutor.(Livro_Autor).Livro_CodLivro] <---> [CodLivro.(Livro).CodLivro] <---> [Livro_CodLivro.(Livro_Autor).Autor_CodAutor] <---> [CodAutor.(Autor)]
		 