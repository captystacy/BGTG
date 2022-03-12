import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class EnergyAndWaterService {
  constructor(private _http: HttpClient) { }

  downloadEnergyAndWater(estimateFiles: FileList): void {
    let formData = new FormData();

    for (let i = 0; i < estimateFiles.length; i++) {
      formData.append('EstimateFiles', estimateFiles[i]);
    }

    this._http.post('api/EnergyAndWater/GetFile', formData, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, "Энергия и вода");
    }, error => console.error(error));
  }
}
