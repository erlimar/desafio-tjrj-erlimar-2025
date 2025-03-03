import { Injectable } from '@angular/core';
import { Autor } from './autor.data';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebapiService {

  url = 'https://localhost:7207';

  constructor(private http: HttpClient) {

  }

  obterTodosAutores(): Observable<Autor[]> {
    return this.http.get<Autor[]>(`${this.url}/autores`);
  }

  excluirAutorPorId(autorId: number): Observable<undefined> {
    return this.http.delete<undefined>(`${this.url}/autores/${autorId}`);
  }

  incluirNovoAutor(autor: Autor): Observable<Autor> {
    return this.http.post<Autor>(`${this.url}/autores`, autor);
  }

  atualizarAutor(autor: Autor): Observable<Autor> {
    return this.http.post<Autor>(`${this.url}/autores/${autor.codigo}`, autor);
  }
}
