import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CatalogoApi } from '../../core/catalogo-api';
import { Categoria, DIFICULDADES, Ingrediente, Receita, ReceitaGuardar } from '../../core/models';
import { ReceitasApi } from '../../core/receitas-api';

@Component({
  selector: 'app-receita-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './receita-form.html',
})
export class ReceitaForm {
  private fb = inject(FormBuilder);
  private api = inject(ReceitasApi);
  private catalogo = inject(CatalogoApi);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  id = Number(this.route.snapshot.paramMap.get('id')) || null;

  categorias = signal<Categoria[]>([]);
  ingredientes = signal<Ingrediente[]>([]);
  dificuldades = DIFICULDADES;
  opcoesDificuldade = [1, 2, 3];
  erro = signal('');
  aEnviar = signal(false);

  form = this.fb.nonNullable.group({
    titulo: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    modoPreparacao: ['', [Validators.required, Validators.minLength(10)]],
    categoriaId: [0, [Validators.min(1)]],
    dificuldade: [1],
    duracaoMinutos: [30, [Validators.required, Validators.min(1), Validators.max(1440)]],
    ingredientes: this.fb.array([this.novaLinha()]),
  });

  get linhas() {
    return this.form.controls.ingredientes;
  }

  constructor() {
    this.catalogo.categorias().subscribe((c) => this.categorias.set(c));
    this.catalogo.ingredientes().subscribe((i) => this.ingredientes.set(i));

    if (this.id) {
      this.api.obter(this.id).subscribe({
        next: (r) => this.preencher(r),
        error: () => this.erro.set('Não foi possível carregar a receita.'),
      });
    }
  }

  novaLinha() {
    return this.fb.nonNullable.group({
      ingredienteId: [0, [Validators.min(1)]],
      quantidade: [1, [Validators.required, Validators.min(0.01)]],
      unidade: ['', [Validators.required, Validators.maxLength(30)]],
    });
  }

  adicionarLinha() {
    this.linhas.push(this.novaLinha());
  }

  removerLinha(indice: number) {
    if (this.linhas.length > 1) {
      this.linhas.removeAt(indice);
    }
  }

  private preencher(r: Receita) {
    this.linhas.clear();
    for (const i of r.ingredientes) {
      const linha = this.novaLinha();
      linha.patchValue({
        ingredienteId: i.ingredienteId,
        quantidade: i.quantidade,
        unidade: i.unidade,
      });
      this.linhas.push(linha);
    }

    this.form.patchValue({
      titulo: r.titulo,
      modoPreparacao: r.modoPreparacao,
      categoriaId: r.categoriaId,
      dificuldade: r.dificuldade,
      duracaoMinutos: r.duracaoMinutos,
    });
  }

  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dados: ReceitaGuardar = this.form.getRawValue();
    this.aEnviar.set(true);
    this.erro.set('');

    if (this.id) {
      this.api.atualizar(this.id, dados).subscribe({
        next: () => this.router.navigate(['/receitas', this.id]),
        error: (e) => this.falhou(e.status),
      });
    } else {
      this.api.criar(dados).subscribe({
        next: (receita) => this.router.navigate(['/receitas', receita.id]),
        error: (e) => this.falhou(e.status),
      });
    }
  }

  private falhou(estado: number) {
    this.aEnviar.set(false);
    this.erro.set(
      estado === 403
        ? 'Não tem permissão para editar esta receita.'
        : 'Não foi possível guardar a receita. Verifique os dados.',
    );
  }
}