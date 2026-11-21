import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { API_URL } from './api.config';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (pedido, seguinte) => {
  const token = inject(AuthService).token;

  if (token && pedido.url.startsWith(API_URL)) {
    pedido = pedido.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
  }

  return seguinte(pedido);
};