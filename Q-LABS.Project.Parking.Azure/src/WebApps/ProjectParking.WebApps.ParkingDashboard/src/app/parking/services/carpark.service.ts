import { Injectable } from '@angular/core';
import { environment } from '@qnh/env';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../models/api-response';
import { Carpark } from '../models/carpark';
import { forEach } from '@angular/router/src/utils/collection';
import { SettingsService } from '../../settings.service';

@Injectable(
  {
    providedIn: 'root'
  }
)
export class CarparkService {

  private baseUrl: string;

  constructor (private http: HttpClient, private settingsService: SettingsService){
    this.baseUrl = settingsService.getConfig().baseUrl;
  }

  getCarparks() {
    return this.http.get<ApiResponse<ApiResponse<Carpark>[]>>(`${this.baseUrl}/api/v1/carparks`);
  }

  getParkingSpotsForCarpark(carparkId: number) {
    return this.http.get<ApiResponse<ApiResponse<any>[]>>(`${this.baseUrl}/api/v1/carparks/${carparkId}/ParkingSpots`);
  }

}
