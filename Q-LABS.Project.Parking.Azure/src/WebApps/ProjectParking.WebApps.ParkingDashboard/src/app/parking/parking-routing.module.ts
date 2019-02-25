import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

import * as fromContainers from './containers';

export const routes: Routes = [
  { path: '', component: fromContainers.ParkingPageComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ParkingRoutingModule {}
