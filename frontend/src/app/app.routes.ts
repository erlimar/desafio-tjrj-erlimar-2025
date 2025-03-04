import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AssuntosComponent } from './assuntos/assuntos.component';
import { AutoresComponent } from './autores/autores.component';
import { LivrosComponent } from './livros/livros.component';

export const routes: Routes = [
  {
    path: '',
    title: 'Home Page',
    component: HomeComponent
  },
  {
    path: 'autores',
    title: 'Autores',
    component: AutoresComponent
  },
  {
    path: 'assuntos',
    title: 'Assuntos',
    component: AssuntosComponent
  },
  {
    path: 'livros',
    title: 'Livros',
    component: LivrosComponent
  }
];
