import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { saveAs } from 'file-saver';

import { IDurationByTCP } from "./duration-by-tcp.model";

@Injectable({
  providedIn: 'root'
})
export class DurationByTCPService {
  constructor(private _http: HttpClient) { }

  downloadDurationByTCP(durationByTCP: IDurationByTCP): void {
    this._http.post('api/DurationByTCP/Download', durationByTCP, { responseType: 'blob' }).subscribe(data => {
        saveAs(data, "Продолжительность по ТКП");
      },
      error => console.error(error));
  }
}
