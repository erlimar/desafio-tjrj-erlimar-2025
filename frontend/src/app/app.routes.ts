import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AutoresComponent } from './autores/autores.component';

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
  }
];
