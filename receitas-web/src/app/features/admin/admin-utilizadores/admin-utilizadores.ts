import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { AdminApi } from '../../../core/admin-api';
import { UtilizadorAdmin } from '../../../core/models';

@Component({
  selector: 'app-admin-utilizadores',
  templateUrl: './admin-utilizadores.html',
})
export class AdminUtilizadores {
  private admin = inject(AdminApi);

  utilizadores = signal<UtilizadorAdmin[]>([]);
  erro = signal('');

  constructor() {
    this.carregar();
  }

  carregar() {
    this.admin.utilizadores().subscribe({
      next: (dados) => this.utilizadores.set(dados),
      error: () => this.erro.set('Não foi possível carregar os utilizadores.'),
    });
  }

  alterarBloqueio(utilizador: UtilizadorAdmin) {
    this.erro.set('');

    const pedido = utilizador.bloqueado
      ? this.admin.desbloquear(utilizador.id)
      : this.admin.bloquear(utilizador.id);

    pedido.subscribe({
      next: () => this.carregar(),
      error: (e: HttpErrorResponse) =>
        this.erro.set(
          e.status === 403
            ? 'Não é possível bloquear administradores.'
            : 'Não foi possível alterar o estado do utilizador.',
        ),
    });
  }
}