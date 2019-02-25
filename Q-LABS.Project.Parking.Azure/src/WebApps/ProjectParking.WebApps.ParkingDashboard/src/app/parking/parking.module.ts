import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ParkingRoutingModule } from './parking-routing.module';
import { MaterialModule } from '../material/material.module';
import { StoreModule } from '@ngrx/store';
import { reducers, ParkingEffects, CarparkEffects  } from './store';
import { EffectsModule } from '@ngrx/effects';

import * as fromContainers from './containers';
import * as fromComponents from './components';

import { CarparkInterceptor } from './services/carpark.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CarparkService } from './services/carpark.service';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    ParkingRoutingModule,
    StoreModule.forFeature('parking', reducers),
    EffectsModule.forFeature([ParkingEffects, CarparkEffects])
  ],
  declarations: [
    ...fromContainers.containers,
    ...fromComponents.components
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CarparkInterceptor,
      multi: true
    },
    CarparkService
  ]
})
export class ParkingModule {}
