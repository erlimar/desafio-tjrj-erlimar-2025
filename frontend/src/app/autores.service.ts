import { Injectable } from '@angular/core';
import { Autor } from './autor.data';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AutoresService {

  apiUrlBase = 'https://localhost:7207';

  constructor(private http: HttpClient) { }

  obterUrlRelatorioAutores() {
    return `${this.apiUrlBase}/relatorios/autores`
  }

  obterTodosAutores(): Observable<Autor[]> {
    return this.http.get<Autor[]>(`${this.apiUrlBase}/autores`);
  }

  excluirAutorPorId(autorId: number): Observable<undefined> {
    return this.http.delete<undefined>(`${this.apiUrlBase}/autores/${autorId}`);
  }

  incluirNovoAutor(autor: Autor): Observable<Autor> {
    return this.http.post<Autor>(`${this.apiUrlBase}/autores`, autor);
  }

  atualizarAutor(autor: Autor): Observable<Autor> {
    return this.http.post<Autor>(`${this.apiUrlBase}/autores/${autor.codigo}`, autor);
  }
}
