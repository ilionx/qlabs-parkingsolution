import { Component, OnInit } from '@angular/core';
import { SettingsService } from './settings.service';
import { SocketService } from './socket.service';


@Component({
  selector: 'qnh-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'ParkingDashboard';

  constructor(private settingsService: SettingsService, private socketService: SocketService) {}

  ngOnInit() {
    this.socketService.connected$.subscribe(connected => console.log('socket connected', connected));
  }
}
