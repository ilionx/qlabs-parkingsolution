import { Injectable } from "@angular/core";
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject, Observable } from "rxjs";
import { SettingsService } from "./settings.service";

@Injectable(
  {providedIn: 'root'}
)
export class SocketService {

  private socket: signalR.HubConnection;
  private connected = new BehaviorSubject<boolean>(false);
  connected$ = this.connected.asObservable();


    // connection.start().catch(err => document.write(err));

    // connection.on('NotifyParkingStatusForCarpark', (username: string, message: any) => {
    //   console.log(`message ${username} ->  ${JSON.stringify(message)}`);
    // });

    constructor(private settingsService: SettingsService) {
        const baseUrl = settingsService.getConfig().baseUrl;
       this.socket = new signalR.HubConnectionBuilder()
       .withUrl(`${baseUrl}/broadcast`)
       .build();

       this.socket.start().then(() => {
         console.log('socket started');
         this.connected.next(true);
       }).catch(err => console.log('socket failed'));

       this.socket.onclose(() => {
         console.log('close socket');
         this.connected.next(false);
       });
    }

    listen(event: string): Observable<any> {
      return new Observable( observer => {

        this.socket.on(event, (data, message) => {

          // console.group();
          //   console.log('----- SOCKET INBOUND -----');
          //   console.log('Action: ', event);
          //   console.log('Payload: ', data);
          //   console.log('Message: ', message);
          // console.groupEnd();

          observer.next(data);
        });
        // dispose of the event listener when unsubscribed
        return () => this.socket.off(event);
      });
    }
}
