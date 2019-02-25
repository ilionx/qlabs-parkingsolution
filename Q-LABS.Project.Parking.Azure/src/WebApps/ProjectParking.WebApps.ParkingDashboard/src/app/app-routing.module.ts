import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/parking', pathMatch: 'full' },
  {
    path: 'parking',
    loadChildren: './parking/parking.module#ParkingModule',
    canActivate: [],
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
