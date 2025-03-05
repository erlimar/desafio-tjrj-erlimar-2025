import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Assunto } from './assunto.data';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AssuntosService {
  apiUrlBase = 'https://localhost:7207';

  constructor(private http: HttpClient) { }

  obterUrlRelatorioAssuntos() {
    return `${this.apiUrlBase}/relatorios/assuntos`
  }

  obterTodosAssuntos(): Observable<Assunto[]> {
    return this.http.get<Assunto[]>(`${this.apiUrlBase}/assuntos`);
  }

  excluirAssuntoPorId(assuntoId: number): Observable<undefined> {
    return this.http.delete<undefined>(`${this.apiUrlBase}/assuntos/${assuntoId}`);
  }

  incluirNovoAssunto(assunto: Assunto): Observable<Assunto> {
    return this.http.post<Assunto>(`${this.apiUrlBase}/assuntos`, assunto);
  }

  atualizarAssunto(assunto: Assunto): Observable<Assunto> {
    return this.http.post<Assunto>(`${this.apiUrlBase}/assuntos/${assunto.codigo}`, assunto);
  }
}
