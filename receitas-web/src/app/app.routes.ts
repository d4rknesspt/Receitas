import { Routes } from '@angular/router';
import { LoginPagina } from './features/login/login';
import { ReceitasLista } from './features/receitas-lista/receitas-lista';
import { RegistoPagina } from './features/registo/registo';

export const routes: Routes = [
  { path: '', component: ReceitasLista },
  { path: 'login', component: LoginPagina },
  { path: 'registo', component: RegistoPagina },
];