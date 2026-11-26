import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_URL } from './api.config';
import { Categoria, Ingrediente, IngredienteGestao, NovoIngrediente } from './models';

@Injectable({ providedIn: 'root' })
export class CatalogoApi {
  private http = inject(HttpClient);

  categorias() {
    return this.http.get<Categoria[]>(`${API_URL}/categorias`);
  }

  ingredientes() {
    return this.http.get<Ingrediente[]>(`${API_URL}/ingredientes`);
  }

  proporIngrediente(dados: NovoIngrediente) {
    return this.http.post<IngredienteGestao>(`${API_URL}/ingredientes`, dados);
  }
}