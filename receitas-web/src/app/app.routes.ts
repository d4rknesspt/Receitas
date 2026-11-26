import { Routes } from '@angular/router';
import { adminGuard } from './core/admin.guard';
import { autenticadoGuard } from './core/auth.guard';
import { AdminIngredientes } from './features/admin/admin-ingredientes/admin-ingredientes';
import { AdminLayout } from './features/admin/admin-layout/admin-layout';
import { AdminReceitas } from './features/admin/admin-receitas/admin-receitas';
import { AdminUtilizadores } from './features/admin/admin-utilizadores/admin-utilizadores';
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
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [adminGuard],
    children: [
      { path: '', redirectTo: 'receitas', pathMatch: 'full' },
      { path: 'receitas', component: AdminReceitas },
      { path: 'utilizadores', component: AdminUtilizadores },
      { path: 'ingredientes', component: AdminIngredientes },
    ],
  },
];