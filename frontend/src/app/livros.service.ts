import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Livro } from './livro.data';
import { Autor } from './autor.data';
import { Assunto } from './assunto.data';

@Injectable({
  providedIn: 'root'
})
export class LivrosService {
  apiUrlBase = 'https://localhost:7207';

  constructor(private http: HttpClient) { }

  obterUrlRelatorioConsolidado() {
    return `${this.apiUrlBase}/relatorios/consolidado`
  }

  obterTodosLivros(): Observable<Livro[]> {
    return this.http.get<Livro[]>(`${this.apiUrlBase}/livros`);
  }

  obterAutoresDoLivro(livroId: number): Observable<Autor[]> {
    return this.http.get<Autor[]>(`${this.apiUrlBase}/livros/${livroId}/autores`);
  }

  obterAssuntosDoLivro(livroId: number): Observable<Assunto[]> {
    return this.http.get<Assunto[]>(`${this.apiUrlBase}/livros/${livroId}/assuntos`);
  }

  excluirLivroPorId(livroId: number): Observable<undefined> {
    return this.http.delete<undefined>(`${this.apiUrlBase}/livros/${livroId}`);
  }

  incluirNovoLivro(livro: Livro): Observable<Livro> {
    return this.http.post<Livro>(`${this.apiUrlBase}/livros`, livro);
  }

  atualizarLivro(livro: Livro): Observable<Livro> {
    return this.http.post<Livro>(`${this.apiUrlBase}/livros/${livro.codigo}`, livro);
  }
}
