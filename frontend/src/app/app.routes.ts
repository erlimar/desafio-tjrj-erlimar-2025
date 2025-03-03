import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListaAutoresComponent } from './lista-autores/lista-autores.component';

export const routes: Routes = [
  {
    path: '',
    title: 'Home Page',
    component: HomeComponent
  },
  {
    path: 'autores',
    title: 'Autores',
    component: ListaAutoresComponent
  }
];
