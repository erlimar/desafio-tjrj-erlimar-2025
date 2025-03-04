import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Livro } from './livro.data';

@Injectable({
  providedIn: 'root'
})
export class LivrosService {
  apiUrlBase = 'https://localhost:7207';

  constructor(private http: HttpClient) { }

  obterUrlRelatorioLivros() {
    return `${this.apiUrlBase}/relatorios/livros`
  }

  obterTodosLivros(): Observable<Livro[]> {
    return this.http.get<Livro[]>(`${this.apiUrlBase}/livros`);
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
