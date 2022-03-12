import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { saveAs } from 'file-saver';

import { IDurationByLC } from "./duration-by-lc.model";

@Injectable({
  providedIn: 'root'
})
export class DurationByLCService {
  constructor(private _http: HttpClient) { }

  downloadDurationByLC(durationByLC: IDurationByLC, estimateFiles: FileList): void {
    let formData = new FormData();

    for (let i = 0; i < estimateFiles.length; i++) {
      formData.append('EstimateFiles', estimateFiles[i]);
    }
    formData.append('NumberOfWorkingDays', durationByLC.numberOfWorkingDays.toString());
    formData.append('WorkingDayDuration', durationByLC.workingDayDuration.toString());
    formData.append('Shift', durationByLC.shift.toString());
    formData.append('NumberOfEmployees', durationByLC.numberOfEmployees.toString());
    formData.append('TechnologicalLaborCosts', durationByLC.technologicalLaborCosts?.toString() ?? '0');
    formData.append('AcceptanceTimeIncluded', durationByLC.acceptanceTimeIncluded.toString());

    this._http.post('api/DurationByLC/GetFile', formData, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, "Продолжительность по трудозатратам");
    }, error => console.error(error));
  }
}
