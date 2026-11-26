import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CatalogoApi } from '../../core/catalogo-api';
import {
  Categoria,
  DIFICULDADES,
  Ingrediente,
  Receita,
  ReceitaGuardar,
  TIPOS_MEDIDA,
} from '../../core/models';
import { ReceitasApi } from '../../core/receitas-api';

const TIPO_UNIDADE = 1;

type LinhaForm = FormGroup<{
  ingredienteId: FormControl<number>;
  quantidade: FormControl<number>;
  unidade: FormControl<string>;
}>;

function inteiro(controlo: AbstractControl): ValidationErrors | null {
  return Number.isInteger(controlo.value) ? null : { inteiro: true };
}

function duasCasas(controlo: AbstractControl): ValidationErrors | null {
  const valor = Number(controlo.value);
  return Math.abs(valor * 100 - Math.round(valor * 100)) < 1e-6 ? null : { casas: true };
}

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
  tiposMedida = TIPOS_MEDIDA;
  erro = signal('');
  aEnviar = signal(false);

  mostrarNovo = signal(false);
  aProporNovo = signal(false);
  erroNovo = signal('');
  mensagemNovo = signal('');

  novoNome = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(2), Validators.maxLength(80)],
  });
  novoTipo = new FormControl(0, { nonNullable: true, validators: [Validators.min(1)] });

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
    this.carregarIngredientes();

    if (this.id) {
      this.api.obter(this.id).subscribe({
        next: (r) => this.preencher(r),
        error: () => this.erro.set('Não foi possível carregar a receita.'),
      });
    }
  }

  private carregarIngredientes() {
    this.catalogo.ingredientes().subscribe((i) => {
      this.ingredientes.set(i);
      this.linhas.controls.forEach((linha) => this.ajustarLinha(linha));
    });
  }

  novaLinha(): LinhaForm {
    const linha = this.fb.nonNullable.group({
      ingredienteId: [0, [Validators.min(1)]],
      quantidade: [1, [Validators.required, Validators.min(0.01)]],
      unidade: ['', [Validators.required]],
    });

    linha.controls.ingredienteId.valueChanges.subscribe(() => this.ajustarLinha(linha));
    return linha;
  }

  ingredienteDe(linha: LinhaForm): Ingrediente | undefined {
    return this.ingredientes().find((i) => i.id === linha.controls.ingredienteId.value);
  }

  unidadesDe(linha: LinhaForm): string[] {
    return this.ingredienteDe(linha)?.unidades ?? [];
  }

  passoDe(linha: LinhaForm): number {
    return this.ingredienteDe(linha)?.tipoMedida === TIPO_UNIDADE ? 1 : 0.01;
  }

  private ajustarLinha(linha: LinhaForm) {
    if (this.ingredientes().length === 0) {
      return;
    }

    const ingrediente = this.ingredienteDe(linha);
    const quantidade = linha.controls.quantidade;
    const unidade = linha.controls.unidade;

    if (!ingrediente) {
      unidade.setValue('');
      quantidade.setValidators([Validators.required, Validators.min(0.01)]);
    } else {
      if (!ingrediente.unidades.includes(unidade.value)) {
        unidade.setValue(ingrediente.unidades[0]);
      }

      quantidade.setValidators(
        ingrediente.tipoMedida === TIPO_UNIDADE
          ? [Validators.required, Validators.min(1), inteiro]
          : [Validators.required, Validators.min(0.01), duasCasas],
      );
    }

    quantidade.updateValueAndValidity();
  }

  adicionarLinha() {
    this.linhas.push(this.novaLinha());
  }

  removerLinha(indice: number) {
    if (this.linhas.length > 1) {
      this.linhas.removeAt(indice);
    }
  }

  alternarNovo() {
    this.mostrarNovo.update((aberto) => !aberto);
    this.erroNovo.set('');
    this.mensagemNovo.set('');
  }

  proporIngrediente() {
    this.erroNovo.set('');
    this.mensagemNovo.set('');

    if (this.novoNome.invalid || this.novoTipo.invalid) {
      this.novoNome.markAsTouched();
      this.novoTipo.markAsTouched();
      return;
    }

    this.aProporNovo.set(true);

    this.catalogo
      .proporIngrediente({ nome: this.novoNome.value.trim(), tipoMedida: this.novoTipo.value })
      .subscribe({
        next: (criado) => {
          this.aProporNovo.set(false);
          this.novoNome.reset();
          this.novoTipo.reset();

          if (criado.validado) {
            this.mensagemNovo.set(`"${criado.nome}" foi criado e já pode ser usado.`);
            this.carregarIngredientes();
          } else {
            this.mensagemNovo.set(
              `"${criado.nome}" foi proposto. Fica disponível depois de validado pelo administrador.`,
            );
          }
        },
        error: (e: HttpErrorResponse) => {
          this.aProporNovo.set(false);
          this.erroNovo.set(
            e.status === 409
              ? 'Esse ingrediente já existe.'
              : 'Não foi possível propor o ingrediente. Verifique os dados.',
          );
        },
      });
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

    this.linhas.controls.forEach((linha) => this.ajustarLinha(linha));
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
        error: (e: HttpErrorResponse) => this.falhou(e),
      });
    } else {
      this.api.criar(dados).subscribe({
        next: (receita) => this.router.navigate(['/receitas', receita.id]),
        error: (e: HttpErrorResponse) => this.falhou(e),
      });
    }
  }

  private falhou(e: HttpErrorResponse) {
    this.aEnviar.set(false);

    if (e.status === 403) {
      this.erro.set('Não tem permissão para editar esta receita.');
    } else if (e.error?.mensagem) {
      this.erro.set(e.error.mensagem);
    } else {
      this.erro.set('Não foi possível guardar a receita. Verifique os dados.');
    }
  }
}