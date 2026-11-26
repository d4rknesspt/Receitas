import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_URL } from './api.config';
import { IngredienteGestao, UtilizadorAdmin } from './models';

@Injectable({ providedIn: 'root' })
export class AdminApi {
  private http = inject(HttpClient);

  utilizadores() {
    return this.http.get<UtilizadorAdmin[]>(`${API_URL}/utilizadores`);
  }

  bloquear(id: number) {
    return this.http.put<void>(`${API_URL}/utilizadores/${id}/bloquear`, {});
  }

  desbloquear(id: number) {
    return this.http.put<void>(`${API_URL}/utilizadores/${id}/desbloquear`, {});
  }

  ingredientesPendentes() {
    return this.http.get<IngredienteGestao[]>(`${API_URL}/ingredientes/pendentes`);
  }

  validarIngrediente(id: number) {
    return this.http.put<void>(`${API_URL}/ingredientes/${id}/validar`, {});
  }
}