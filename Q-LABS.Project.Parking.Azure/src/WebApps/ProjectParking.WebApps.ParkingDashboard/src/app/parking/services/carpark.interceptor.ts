import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler } from "@angular/common/http";
import { SettingsService } from "../../settings.service";
import { Observable } from "rxjs";

@Injectable(
)
export class CarparkInterceptor implements HttpInterceptor {

  constructor(private settingsService: SettingsService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {

    console.log("call intercept");

    const config = this.settingsService.getConfig();

    if (config != undefined ) {
      req = req.clone({
        setHeaders: {
          'X-API-KEY': config.apiKey
        }
      });

    }


    return next.handle(req);


  }

}
