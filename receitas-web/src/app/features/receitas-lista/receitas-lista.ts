import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CatalogoApi } from '../../core/catalogo-api';
import { Categoria, DIFICULDADES, FiltroReceitas, Receita } from '../../core/models';
import { ReceitasApi } from '../../core/receitas-api';

@Component({
  selector: 'app-receitas-lista',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './receitas-lista.html',
})
export class ReceitasLista {
  private api = inject(ReceitasApi);
  private catalogo = inject(CatalogoApi);
  private fb = inject(FormBuilder);

  receitas = signal<Receita[]>([]);
  categorias = signal<Categoria[]>([]);
  erro = signal('');
  dificuldades = DIFICULDADES;
  opcoesDificuldade = [1, 2, 3];

  filtros = this.fb.nonNullable.group({
    texto: '',
    ingrediente: '',
    categoriaId: '',
    dificuldade: '',
    duracaoMax: '',
  });

  constructor() {
    this.catalogo.categorias().subscribe((c) => this.categorias.set(c));
    this.pesquisar();
  }

  pesquisar() {
    const v = this.filtros.getRawValue();

    const filtro: FiltroReceitas = {
      texto: v.texto.trim(),
      ingrediente: v.ingrediente.trim(),
      categoriaId: v.categoriaId ? Number(v.categoriaId) : undefined,
      dificuldade: v.dificuldade ? Number(v.dificuldade) : undefined,
      duracaoMax: v.duracaoMax ? Number(v.duracaoMax) : undefined,
    };

    this.erro.set('');
    this.api.listar(filtro).subscribe({
      next: (dados) => this.receitas.set(dados),
      error: () => this.erro.set('Não foi possível carregar as receitas.'),
    });
  }

  limpar() {
    this.filtros.reset();
    this.pesquisar();
  }
}