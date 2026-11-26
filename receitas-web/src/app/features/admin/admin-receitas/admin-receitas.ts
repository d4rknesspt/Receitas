import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Receita } from '../../../core/models';
import { ReceitasApi } from '../../../core/receitas-api';

@Component({
  selector: 'app-admin-receitas',
  imports: [RouterLink],
  templateUrl: './admin-receitas.html',
})
export class AdminReceitas {
  private api = inject(ReceitasApi);

  receitas = signal<Receita[]>([]);
  apenasPorValidar = signal(false);
  erro = signal('');

  visiveis = computed(() =>
    this.apenasPorValidar() ? this.receitas().filter((r) => !r.validada) : this.receitas(),
  );

  constructor() {
    this.carregar();
  }

  carregar() {
    this.api.listar().subscribe({
      next: (dados) => this.receitas.set(dados),
      error: () => this.erro.set('Não foi possível carregar as receitas.'),
    });
  }

  validar(receita: Receita) {
    this.erro.set('');
    this.api.validar(receita.id).subscribe({
      next: () => this.carregar(),
      error: () => this.erro.set('Não foi possível validar a receita.'),
    });
  }

  apagar(receita: Receita) {
    if (!confirm(`Apagar a receita "${receita.titulo}"? Esta ação não pode ser desfeita.`)) {
      return;
    }

    this.erro.set('');
    this.api.apagar(receita.id).subscribe({
      next: () => this.carregar(),
      error: () => this.erro.set('Não foi possível apagar a receita.'),
    });
  }
}