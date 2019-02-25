import { Injectable, APP_INITIALIZER } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { tap } from "rxjs/operators";
import { environment } from "@qnh/env";

@Injectable()
export class SettingsService {

  private settings: EnvSettings;

  constructor(private http: HttpClient) {}

  public loadConfig() {
    const file = `assets/${environment.configFile}`;

   return this.http.get<EnvSettings>(file)
      .pipe(
        tap((env) => console.log('env', env)),
        tap((env) => this.settings = env)
      ).toPromise();
  }

  public getConfig(): EnvSettings {
    console.log("DO WE HAVE A CONFIG?", this.settings);
    return this.settings;
  }

}

export interface EnvSettings {
  apiKey: string;
  baseUrl: string;
}

export function initSettings(
  settingsService: SettingsService,
  http: HttpClient
) {
  return () => settingsService.loadConfig();
}

export const settingsProviders = [
  SettingsService,
  {
    provide: APP_INITIALIZER,
    useFactory: initSettings,
    deps: [SettingsService, HttpClient],
    multi: true
  }
];
