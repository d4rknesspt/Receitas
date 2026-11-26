import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from './api.config';
import { FiltroReceitas, Receita, ReceitaGuardar } from './models';

@Injectable({ providedIn: 'root' })
export class ReceitasApi {
  private http = inject(HttpClient);

  listar(filtro: FiltroReceitas = {}): Observable<Receita[]> {
    let params = new HttpParams();
    for (const [chave, valor] of Object.entries(filtro)) {
      if (valor !== undefined && valor !== null && valor !== '') {
        params = params.set(chave, String(valor));
      }
    }
    return this.http.get<Receita[]>(`${API_URL}/receitas`, { params });
  }

  obter(id: number): Observable<Receita> {
    return this.http.get<Receita>(`${API_URL}/receitas/${id}`);
  }

  criar(dados: ReceitaGuardar): Observable<Receita> {
    return this.http.post<Receita>(`${API_URL}/receitas`, dados);
  }

  atualizar(id: number, dados: ReceitaGuardar): Observable<void> {
    return this.http.put<void>(`${API_URL}/receitas/${id}`, dados);
  }

  validar(id: number): Observable<void> {
    return this.http.put<void>(`${API_URL}/receitas/${id}/validar`, {});
  }

  apagar(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/receitas/${id}`);
  }
}