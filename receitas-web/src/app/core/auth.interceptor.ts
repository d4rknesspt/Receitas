import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { API_URL } from './api.config';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (pedido, seguinte) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const token = auth.token;
  const eDaApi = pedido.url.startsWith(API_URL);
  const eDeAutenticacao = pedido.url.includes('/auth/');

  if (token && eDaApi) {
    pedido = pedido.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
  }

  return seguinte(pedido).pipe(
    catchError((erro: HttpErrorResponse) => {
      if (erro.status === 401 && token && eDaApi && !eDeAutenticacao) {
        auth.sair();
        router.navigate(['/login'], { queryParams: { expirada: 1 } });
      }

      return throwError(() => erro);
    }),
  );
};