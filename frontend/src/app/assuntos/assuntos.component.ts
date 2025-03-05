import { Component, inject } from '@angular/core';
import { AssuntosService } from '../assuntos.service';
import { Assunto } from '../assunto.data';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-assuntos',
  imports: [CommonModule, FormsModule],
  templateUrl: './assuntos.component.html',
  styleUrl: './assuntos.component.css'
})
export class AssuntosComponent {
  assuntosService: AssuntosService = inject(AssuntosService);

  assuntosCadastrados: Assunto[] = []

  selecionado: Assunto | null = null;
  acaoSelecionada: string | null = null;
  errosServidor: string[] = [];
  carregando: boolean = false;

  constructor() {
    this.assuntosService.obterTodosAssuntos().subscribe((assuntos: Assunto[]) => {
      this.assuntosCadastrados = assuntos;
    });
  }

  incluirNovoAssunto() {
    this.selecionar({ codigo: 0, descricao: '' }, 'cadastrar');
  }

  selecionar(assunto: Assunto, acao: string) {
    this.selecionado = { codigo: assunto.codigo, descricao: assunto.descricao };
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
      this.assuntosService.excluirAssuntoPorId(this.selecionado.codigo).subscribe((r) => {
        this.assuntosCadastrados = this.assuntosCadastrados.filter((a) => a.codigo !== this.selecionado?.codigo);
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
      this.assuntosService.incluirNovoAssunto(this.selecionado).subscribe({
        next: (assuntoNovo: Assunto) => {
          this.assuntosCadastrados.push(assuntoNovo);
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
      this.assuntosService.atualizarAssunto(this.selecionado).subscribe({
        next: (assuntoAtualizado: Assunto) => {
          this.assuntosCadastrados = this.assuntosCadastrados.filter((a) => a.codigo !== assuntoAtualizado.codigo);
          this.assuntosCadastrados.push(assuntoAtualizado);
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
