import { Component, inject } from '@angular/core';
import { LivrosService } from '../livros.service';
import { Livro } from '../livro.data';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Autor } from '../autor.data';
import { Assunto } from '../assunto.data';
import { AutoresService } from '../autores.service';
import { AssuntosService } from '../assuntos.service';
import { ValorLivro } from '../valor-livro.data';

@Component({
  selector: 'app-livros',
  imports: [CommonModule, FormsModule],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent {
  livrosService: LivrosService = inject(LivrosService);
  autoresService: AutoresService = inject(AutoresService);
  assuntosService: AssuntosService = inject(AssuntosService);

  livrosCadastrados: Livro[] = []

  selecionado: Livro | null = null;
  autoresDisponiveis: Autor[] = [];
  autorSelecionadoParaIncluir: Autor | null = null;
  assuntosDisponiveis: Assunto[] = [];
  formasCompraDisponiveis: string[] = [
    "balcão",
    "self-service",
    "internet",
    "evento",
    "outros"
  ]
  assuntoSelecionadoParaIncluir: Assunto | null = null;
  valorSelecionadoParaIncluir: number | null = null;
  valorFormaSelecionadoParaIncluir: string | null = null;
  acaoSelecionada: string | null = null;
  errosServidor: string[] = [];
  carregando: boolean = false;
  incluindoAutor: boolean = false;
  incluindoAssunto: boolean = false;
  incluindoValor: boolean = false;

  constructor() {
    this.livrosService.obterTodosLivros().subscribe((livros: Livro[]) => {
      this.livrosCadastrados = livros;
    });
  }

  obterAutoresParaIncluir(): Autor[] {
    if (!this.selecionado || !Array.isArray(this.autoresDisponiveis) || !this.autoresDisponiveis.length) {
      return [];
    }

    const jaSelecionados = this.selecionado.autores || [];

    return this.autoresDisponiveis.filter(
      (autor: Autor) => !jaSelecionados.filter(s => s.codigo === autor.codigo).length);
  }

  obterAssuntosParaIncluir(): Assunto[] {
    if (!this.selecionado || !Array.isArray(this.assuntosDisponiveis) || !this.assuntosDisponiveis.length) {
      return [];
    }

    const jaSelecionados = this.selecionado.assuntos || [];

    return this.assuntosDisponiveis.filter(
      (assunto: Assunto) => !jaSelecionados.filter(s => s.codigo === assunto.codigo).length);
  }

  obterFormasCompraParaIncluir(): string[] {
    if (!this.selecionado || !Array.isArray(this.formasCompraDisponiveis) || !this.formasCompraDisponiveis.length) {
      return [];
    }

    const jaSelecionados = this.selecionado.valores || [];

    return this.formasCompraDisponiveis.filter(
      (formaCompra: string) => !jaSelecionados.filter(s => s.formaCompra === formaCompra).length);
  }

  removerAutorDaSelecao(autor: Autor) {
    if (this.selecionado?.autores) {
      this.selecionado.autores = this.selecionado.autores.filter((a: Autor) => a.codigo !== autor.codigo);
    }
  }

  removerAssuntoDaSelecao(assunto: Assunto) {
    if (this.selecionado?.assuntos) {
      this.selecionado.assuntos = this.selecionado.assuntos.filter((a: Assunto) => a.codigo !== assunto.codigo);
    }
  }

  removerValorDaSelecao(valor: ValorLivro) {
    if (this.selecionado?.valores) {
      this.selecionado.valores = this.selecionado.valores.filter((v: ValorLivro) => v.formaCompra !== valor.formaCompra);
    }
  }

  incluirAutor() {
    if (this.autorSelecionadoParaIncluir) {
      this.selecionado?.autores?.push(this.autorSelecionadoParaIncluir);
    }

    this.incluindoAutor = false;
    this.autorSelecionadoParaIncluir = null;
  }

  incluirAssunto() {
    if (this.assuntoSelecionadoParaIncluir) {
      this.selecionado?.assuntos?.push(this.assuntoSelecionadoParaIncluir);
    }

    this.incluindoAssunto = false;
    this.assuntoSelecionadoParaIncluir = null;
  }

  incluirValor() {
    // TODO: Conferir dados do formulário
    if (this.valorSelecionadoParaIncluir && this.valorFormaSelecionadoParaIncluir) {
      this.selecionado?.valores?.push({
        codigo: 0,
        formaCompra: this.valorFormaSelecionadoParaIncluir,
        valor: this.valorSelecionadoParaIncluir
      });
    } else {
      alert('Ops!')
      return;
    }

    this.incluindoValor = false;
    this.valorSelecionadoParaIncluir = null;
    this.valorFormaSelecionadoParaIncluir = null;
  }

  incluirNovoLivro() {
    this.selecionar({
      codigo: 0,
      titulo: '',
      editora: '',
      edicao: null,
      anoPublicacao: null,
      autores: [],
      assuntos: [],
      valores: []
    }, 'cadastrar');
  }

  selecionar(livro: Livro, acao: string) {
    this.selecionado = {
      codigo: livro.codigo,
      titulo: livro.titulo,
      editora: livro.editora,
      edicao: livro.edicao,
      anoPublicacao: livro.anoPublicacao,

      // Mesmo que estejamos recebendo um livro como parâmetro
      // deixamos explicitamente os autores e assuntos vazios
      // em cada seleção, pois a intenção é preenchê-los
      // dinamicamente logo em seguida
      autores: [],
      assuntos: [],
      valores: []
    };
    this.acaoSelecionada = acao;

    if (acao == 'cadastrar') {
      // Carregamos os autores do livro se for uma edição
      if (livro.codigo) {
        this.livrosService.obterAutoresDoLivro(livro.codigo).subscribe((autores: Autor[]) => {
          if (this.selecionado) {
            this.selecionado.autores = autores;
          }
        });

        this.livrosService.obterAssuntosDoLivro(livro.codigo).subscribe((assuntos: Assunto[]) => {
          if (this.selecionado) {
            this.selecionado.assuntos = assuntos;
          }
        });
      }

      // Carregamos todas as outras opções de autores e assuntos disponíveis
      this.autoresService.obterTodosAutores().subscribe((autores: Autor[]) => {
        this.autoresDisponiveis = autores;
      })

      this.assuntosService.obterTodosAssuntos().subscribe((assuntos: Assunto[]) => {
        this.assuntosDisponiveis = assuntos;
      })
    }
  }

  cancelarSelecao() {
    this.selecionado = null;
    this.autoresDisponiveis = [];
    this.autorSelecionadoParaIncluir = null;
    this.assuntosDisponiveis = [];
    this.assuntoSelecionadoParaIncluir = null;
    this.valorSelecionadoParaIncluir = null;
    this.valorFormaSelecionadoParaIncluir = null;
    this.acaoSelecionada = null;
    this.errosServidor = [];
    this.carregando = false;
    this.incluindoAssunto = false;
    this.incluindoAutor = false;
    this.incluindoValor = false;
  }

  confirmaExclusaoSelecionado() {
    this.carregando = true;

    if (this.selecionado && this.acaoSelecionada === 'excluir') {
      this.livrosService.excluirLivroPorId(this.selecionado.codigo).subscribe((r) => {
        this.livrosCadastrados = this.livrosCadastrados.filter((a) => a.codigo !== this.selecionado?.codigo);
        this.carregando = false;
        this.incluindoAssunto = false;
        this.incluindoAutor = false;
        this.incluindoValor = false;
        this.selecionado = null;
        this.acaoSelecionada = null;
      })
    } else {
      this.carregando = false;
      this.incluindoAssunto = false;
      this.incluindoAutor = false;
      this.incluindoValor = false;
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
          this.incluindoAssunto = false;
          this.incluindoAutor = false;
          this.incluindoValor = false;
          this.selecionado = null;
          this.autoresDisponiveis = [];
          this.autorSelecionadoParaIncluir = null;
          this.assuntosDisponiveis = [];
          this.assuntoSelecionadoParaIncluir = null;
          this.valorSelecionadoParaIncluir = null;
          this.valorFormaSelecionadoParaIncluir = null;
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
          this.incluindoAssunto = false;
          this.incluindoAutor = false;
          this.incluindoValor = false;
        }
      });

      // Atualizar existente
    } else if (this.selecionado && this.acaoSelecionada === 'cadastrar' && this.selecionado.codigo) {
      this.livrosService.atualizarLivro(this.selecionado).subscribe({
        next: (livroAtualizado: Livro) => {
          this.livrosCadastrados = this.livrosCadastrados.filter((a) => a.codigo !== livroAtualizado.codigo);
          this.livrosCadastrados.push(livroAtualizado);
          this.carregando = false;
          this.incluindoAssunto = false;
          this.incluindoAutor = false;
          this.incluindoValor = false;
          this.selecionado = null;
          this.autoresDisponiveis = [];
          this.autorSelecionadoParaIncluir = null;
          this.assuntosDisponiveis = [];
          this.assuntoSelecionadoParaIncluir = null;
          this.valorSelecionadoParaIncluir = null;
          this.valorFormaSelecionadoParaIncluir = null;
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
          this.incluindoAssunto = false;
          this.incluindoAutor = false;
          this.incluindoValor = false;
        }
      });
    } else {
      this.carregando = false;
      this.incluindoAssunto = false;
      this.incluindoAutor = false;
      this.incluindoValor = false;
    }
  }
}
