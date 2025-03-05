import { Component, inject } from '@angular/core';
import { AutoresService } from '../autores.service';
import { Autor } from '../autor.data';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-autores',
  imports: [CommonModule, FormsModule],
  templateUrl: './autores.component.html',
  styleUrl: './autores.component.css'
})
export class AutoresComponent {
  autoresService: AutoresService = inject(AutoresService);

  autoresCadastrados: Autor[] = []

  selecionado: Autor | null = null;
  acaoSelecionada: string | null = null;
  errosServidor: string[] = [];
  carregando: boolean = false;

  constructor() {
    this.autoresService.obterTodosAutores().subscribe((autores: Autor[]) => {
      this.autoresCadastrados = autores;
    });
  }

  incluirNovoAutor() {
    this.selecionar({ codigo: 0, nome: '' }, 'cadastrar');
  }

  selecionar(autor: Autor, acao: string) {
    this.selecionado = { codigo: autor.codigo, nome: autor.nome };
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
      this.autoresService.excluirAutorPorId(this.selecionado.codigo).subscribe((r) => {
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
      this.autoresService.incluirNovoAutor(this.selecionado).subscribe({
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
      this.autoresService.atualizarAutor(this.selecionado).subscribe({
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
