import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';

function passwordsIguais(grupo: AbstractControl): ValidationErrors | null {
  const password = grupo.get('password')?.value;
  const confirmar = grupo.get('confirmar')?.value;
  return password === confirmar ? null : { diferentes: true };
}

@Component({
  selector: 'app-registo',
  imports: [ReactiveFormsModule],
  templateUrl: './registo.html',
})
export class RegistoPagina {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  erro = signal('');
  aEnviar = signal(false);

  form = this.fb.nonNullable.group(
    {
      nome: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmar: ['', [Validators.required]],
    },
    { validators: passwordsIguais },
  );

  registar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.aEnviar.set(true);
    this.erro.set('');

    const { nome, email, password } = this.form.getRawValue();

    this.auth.registar({ nome, email, password }).subscribe({
      next: () => this.router.navigateByUrl('/'),
      error: (e) => {
        this.aEnviar.set(false);
        this.erro.set(
          e.status === 409
            ? 'Já existe uma conta com esse email.'
            : 'Não foi possível criar a conta. Verifique os dados.',
        );
      },
    });
  }
}