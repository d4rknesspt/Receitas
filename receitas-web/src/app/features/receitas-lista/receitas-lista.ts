import { Component, inject, signal } from '@angular/core';
import { DIFICULDADES, Receita } from '../../core/models';
import { ReceitasApi } from '../../core/receitas-api';

@Component({
  selector: 'app-receitas-lista',
  templateUrl: './receitas-lista.html',
})
export class ReceitasLista {
  private api = inject(ReceitasApi);

  receitas = signal<Receita[]>([]);
  erro = signal('');
  dificuldades = DIFICULDADES;

  constructor() {
    this.api.listar().subscribe({
      next: (dados) => this.receitas.set(dados),
      error: () => this.erro.set('Não foi possível carregar as receitas.'),
    });
  }
}