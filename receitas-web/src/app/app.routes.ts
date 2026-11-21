import { Routes } from '@angular/router';
import { autenticadoGuard } from './core/auth.guard';
import { LoginPagina } from './features/login/login';
import { ReceitaDetalhe } from './features/receita-detalhe/receita-detalhe';
import { ReceitaForm } from './features/receita-form/receita-form';
import { ReceitasLista } from './features/receitas-lista/receitas-lista';
import { RegistoPagina } from './features/registo/registo';

export const routes: Routes = [
  { path: '', component: ReceitasLista },
  { path: 'receitas/nova', component: ReceitaForm, canActivate: [autenticadoGuard] },
  { path: 'receitas/:id/editar', component: ReceitaForm, canActivate: [autenticadoGuard] },
  { path: 'receitas/:id', component: ReceitaDetalhe },
  { path: 'login', component: LoginPagina },
  { path: 'registo', component: RegistoPagina },
];