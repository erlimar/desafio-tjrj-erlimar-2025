import { Assunto } from "./assunto.data";
import { Autor } from "./autor.data";
import { ValorLivro } from "./valor-livro.data";

export interface Livro {
  codigo: number;
  titulo: string;
  editora: string;
  edicao: number | null;
  anoPublicacao: number | null;
  assuntos: Assunto[] | null;
  autores: Autor[] | null;
  valores: ValorLivro[] | null;
}
