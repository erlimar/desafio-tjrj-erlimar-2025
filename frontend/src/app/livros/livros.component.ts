import { Component, inject } from '@angular/core';
import { LivrosService } from '../livros.service';
import { Livro } from '../livro.data';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-livros',
  imports: [CommonModule, FormsModule],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent {
  livrosService: LivrosService = inject(LivrosService);

  livrosCadastrados: Livro[] = []

  selecionado: Livro | null = null;
  acaoSelecionada: string | null = null;
  errosServidor: string[] = [];
  carregando: boolean = false;

  constructor() {
    this.livrosService.obterTodosLivros().subscribe((livros: Livro[]) => {
      this.livrosCadastrados = livros;
    });
  }

  incluirNovoLivro() {
    this.selecionar({
      codigo: 0,
      titulo: '',
      editora: '',
      edicao: null,
      anoPublicacao: null
    }, 'cadastrar');
  }

  selecionar(livro: Livro, acao: string) {
    this.selecionado = {
      codigo: livro.codigo,
      titulo: livro.titulo,
      editora: livro.editora,
      edicao: livro.edicao,
      anoPublicacao: livro.anoPublicacao
    };
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
      this.livrosService.excluirLivroPorId(this.selecionado.codigo).subscribe((r) => {
        this.livrosCadastrados = this.livrosCadastrados.filter((a) => a.codigo !== this.selecionado?.codigo);
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
      this.livrosService.incluirNovoLivro(this.selecionado).subscribe({
        next: (livroNovo: Livro) => {
          this.livrosCadastrados.push(livroNovo);
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
      this.livrosService.atualizarLivro(this.selecionado).subscribe({
        next: (livroAtualizado: Livro) => {
          this.livrosCadastrados = this.livrosCadastrados.filter((a) => a.codigo !== livroAtualizado.codigo);
          this.livrosCadastrados.push(livroAtualizado);
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
