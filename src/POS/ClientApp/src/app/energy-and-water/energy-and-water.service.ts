import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class EnergyAndWaterService {
  constructor(private _http: HttpClient) { }

  downloadEnergyAndWater(estimateFiles: FileList, totalWorkChapter: number): void {
    let formData = new FormData();

    for (let i = 0; i < estimateFiles.length; i++) {
      formData.append('EstimateFiles', estimateFiles[i]);
    }

    formData.append('TotalWorkChapter', totalWorkChapter.toString());

    this._http.post('api/EnergyAndWater/Download', formData, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, "Энергия и вода");
    }, error => console.error(error));
  }
}
