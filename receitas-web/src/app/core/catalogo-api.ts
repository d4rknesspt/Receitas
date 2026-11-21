import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_URL } from './api.config';
import { Categoria, Ingrediente } from './models';

@Injectable({ providedIn: 'root' })
export class CatalogoApi {
  private http = inject(HttpClient);

  categorias() {
    return this.http.get<Categoria[]>(`${API_URL}/categorias`);
  }

  ingredientes() {
    return this.http.get<Ingrediente[]>(`${API_URL}/ingredientes`);
  }
}