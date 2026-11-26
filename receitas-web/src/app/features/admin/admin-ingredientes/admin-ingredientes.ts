import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdminApi } from '../../../core/admin-api';
import { CatalogoApi } from '../../../core/catalogo-api';
import { Ingrediente, IngredienteGestao, TIPOS_MEDIDA } from '../../../core/models';

@Component({
  selector: 'app-admin-ingredientes',
  imports: [ReactiveFormsModule],
  templateUrl: './admin-ingredientes.html',
})
export class AdminIngredientes {
  private admin = inject(AdminApi);
  private catalogo = inject(CatalogoApi);

  pendentes = signal<IngredienteGestao[]>([]);
  validados = signal<Ingrediente[]>([]);
  tiposMedida = TIPOS_MEDIDA;
  erro = signal('');
  mensagem = signal('');

  nome = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(2), Validators.maxLength(80)],
  });
  tipo = new FormControl(0, { nonNullable: true, validators: [Validators.min(1)] });

  constructor() {
    this.carregar();
  }

  carregar() {
    this.admin.ingredientesPendentes().subscribe({
      next: (dados) => this.pendentes.set(dados),
      error: () => this.erro.set('Não foi possível carregar os ingredientes.'),
    });
    this.catalogo.ingredientes().subscribe((dados) => this.validados.set(dados));
  }

  nomeDoTipo(tipo: number): string {
    return this.tiposMedida.find((t) => t.valor === tipo)?.texto ?? '';
  }

  validar(ingrediente: IngredienteGestao) {
    this.erro.set('');
    this.mensagem.set('');

    this.admin.validarIngrediente(ingrediente.id).subscribe({
      next: () => {
        this.mensagem.set(`"${ingrediente.nome}" foi validado e já pode ser usado.`);
        this.carregar();
      },
      error: () => this.erro.set('Não foi possível validar o ingrediente.'),
    });
  }

  adicionar() {
    this.erro.set('');
    this.mensagem.set('');

    if (this.nome.invalid || this.tipo.invalid) {
      this.nome.markAsTouched();
      this.tipo.markAsTouched();
      return;
    }

    this.catalogo
      .proporIngrediente({ nome: this.nome.value.trim(), tipoMedida: this.tipo.value })
      .subscribe({
        next: (criado) => {
          this.nome.reset();
          this.tipo.reset();
          this.mensagem.set(`"${criado.nome}" foi criado e já pode ser usado.`);
          this.carregar();
        },
        error: (e: HttpErrorResponse) =>
          this.erro.set(
            e.status === 409
              ? 'Esse ingrediente já existe.'
              : 'Não foi possível criar o ingrediente. Verifique os dados.',
          ),
      });
  }
}