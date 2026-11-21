import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DIFICULDADES, Receita } from '../../core/models';
import { ReceitasApi } from '../../core/receitas-api';

@Component({
  selector: 'app-receita-detalhe',
  imports: [RouterLink],
  templateUrl: './receita-detalhe.html',
})
export class ReceitaDetalhe {
  private api = inject(ReceitasApi);
  private route = inject(ActivatedRoute);

  receita = signal<Receita | null>(null);
  erro = signal('');
  dificuldades = DIFICULDADES;

  constructor() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.api.obter(id).subscribe({
      next: (r) => this.receita.set(r),
      error: (e) =>
        this.erro.set(
          e.status === 404 ? 'Receita não encontrada.' : 'Não foi possível carregar a receita.',
        ),
    });
  }
}