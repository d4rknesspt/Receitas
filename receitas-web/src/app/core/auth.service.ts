import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { tap } from 'rxjs';
import { API_URL } from './api.config';
import { AuthResposta, Login, Registo } from './models';

const CHAVE = 'receitas.sessao';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private sessao = signal<AuthResposta | null>(this.lerSessao());

  utilizador = this.sessao.asReadonly();
  autenticado = computed(() => this.sessao() !== null);
  eAdmin = computed(() => this.sessao()?.perfil === 'Administrador');

  get token(): string | null {
    return this.sessao()?.token ?? null;
  }

  login(dados: Login) {
    return this.http
      .post<AuthResposta>(`${API_URL}/auth/login`, dados)
      .pipe(tap((resposta) => this.guardar(resposta)));
  }

  registar(dados: Registo) {
    return this.http
      .post<AuthResposta>(`${API_URL}/auth/registo`, dados)
      .pipe(tap((resposta) => this.guardar(resposta)));
  }

  sair() {
    localStorage.removeItem(CHAVE);
    this.sessao.set(null);
  }

  private guardar(resposta: AuthResposta) {
    localStorage.setItem(CHAVE, JSON.stringify(resposta));
    this.sessao.set(resposta);
  }

  private lerSessao(): AuthResposta | null {
    try {
      const texto = localStorage.getItem(CHAVE);
      if (!texto) {
        return null;
      }
      const sessao = JSON.parse(texto) as AuthResposta;
      return new Date(sessao.expiraEm) > new Date() ? sessao : null;
    } catch {
      return null;
    }
  }
}