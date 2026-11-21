import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
})
export class LoginPagina {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  erro = signal('');
  aEnviar = signal(false);

  form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  entrar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.aEnviar.set(true);
    this.erro.set('');

    this.auth.login(this.form.getRawValue()).subscribe({
      next: () => this.router.navigateByUrl('/'),
      error: (e) => {
        this.aEnviar.set(false);
        this.erro.set(
          e.status === 401
            ? 'Email ou palavra-passe inválidos, ou conta bloqueada.'
            : 'Não foi possível entrar. Tente novamente.',
        );
      },
    });
  }
}