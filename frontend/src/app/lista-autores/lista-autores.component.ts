import { Component, inject } from '@angular/core';
import { WebapiService } from '../webapi.service';
import { Autor } from '../autor.data';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-lista-autores',
  imports: [CommonModule, FormsModule],
  templateUrl: './lista-autores.component.html',
  styleUrl: './lista-autores.component.css'
})
export class ListaAutoresComponent {
  webapi: WebapiService = inject(WebapiService);

  autoresCadastrados: Autor[] = []

  selecionado: Autor | null = null;
  acaoSelecionada: string | null = null;
  errosServidor: string[] = [];
  carregando: boolean = false;

  constructor() {
    this.webapi.obterTodosAutores().subscribe((autores: Autor[]) => {
      this.autoresCadastrados = autores;
    });
  }

  incluirNovoAutor() {
    this.selecionar({ codigo: 0, nome: '' }, 'cadastrar');
  }

  selecionar(autor: Autor, acao: string) {
    this.selecionado = autor;
    this.acaoSelecionada = acao;
  }

  cancelarSelecao() {
    this.selecionado = null;
    this.acaoSelecionada = null;
    this.errosServidor = [];
    this.carregando = false;
  }

  confirmaExclusaoSelecionado() {
    this.carregando = true;

    if (this.selecionado && this.acaoSelecionada === 'excluir') {
      this.webapi.excluirAutorPorId(this.selecionado.codigo).subscribe((r) => {
        this.autoresCadastrados = this.autoresCadastrados.filter((a) => a.codigo !== this.selecionado?.codigo);
        this.carregando = false;
        this.selecionado = null;
        this.acaoSelecionada = null;
      })
    } else {
      this.carregando = false;
    }
  }

  confirmaCadastroSelecionado() {
    this.carregando = true;
    this.errosServidor = [];

    // Incluir novo
    if (this.selecionado && this.acaoSelecionada === 'cadastrar' && !this.selecionado.codigo) {
      this.webapi.incluirNovoAutor(this.selecionado).subscribe({
        next: (autorNovo: Autor) => {
          this.autoresCadastrados.push(autorNovo);
          this.carregando = false;
          this.selecionado = null;
          this.acaoSelecionada = null;
        },
        error: (failure: any) => {
          if (failure && failure.error && failure.error.errors) {
            for (const e in failure.error.errors) {
              const errors = failure.error.errors[e];

              if (Array.isArray(errors)) {
                errors.map(error => this.errosServidor.push(`${e}: ${error}`));
              }
            }
          }

          this.carregando = false;
        }
      });

      // Atualizar existente
    } else if (this.selecionado && this.acaoSelecionada === 'cadastrar' && this.selecionado.codigo) {
      this.webapi.atualizarAutor(this.selecionado).subscribe({
        next: (autorAtualizado: Autor) => {
          this.autoresCadastrados = this.autoresCadastrados.filter((a) => a.codigo !== autorAtualizado.codigo);
          this.autoresCadastrados.push(autorAtualizado);
          this.carregando = false;
          this.selecionado = null;
          this.acaoSelecionada = null;
        },
        error: (failure: any) => {
          if (failure && failure.error && failure.error.errors) {
            for (const e in failure.error.errors) {
              const errors = failure.error.errors[e];

              if (Array.isArray(errors)) {
                errors.map(error => this.errosServidor.push(`${e}: ${error}`));
              }
            }
          }

          this.carregando = false;
        }
      });
    } else {
      this.carregando = false;
    }
  }
}
