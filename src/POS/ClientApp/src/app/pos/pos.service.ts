import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { saveAs } from 'file-saver';

import { IProject, ITableOfContents, ITitlePage } from "./pos.model";

@Injectable({
  providedIn: 'root'
})
export class POSService {
  constructor(private _http: HttpClient) { }

  downloadProject(project: IProject): void {
    let formData = new FormData();

    let files = project.calculationFilesInput.files!;
    for (let i = 0; i < files.length; i++) {
      formData.append('CalculationFiles', files[i]);
    }
    formData.append('ObjectCipher', project.objectCipher);
    formData.append('ProjectTemplate', project.projectTemplate.toString());
    formData.append('ProjectEngineer', project.projectEngineer.toString());
    formData.append('NormalInspectionEngineer', project.normalInspectionEngineer.toString());
    formData.append('ChiefEngineer', project.chiefEngineer.toString());
    formData.append('ChiefProjectEngineer', project.chiefProjectEngineer.toString());

    this._http.post('api/POS/DownloadProject', formData, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, `${project.objectCipher}ПОС.doc`);
    }, error => console.error(error));
  }

  downloadTableOfContents(tableOfContents: ITableOfContents): void {
    this._http.post('api/POS/DownloadTableOfContents', tableOfContents, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, `${tableOfContents.objectCipher}Содержание.doc`);
    }, error => console.error(error));
  }

  downloadTitlePage(titlePage: ITitlePage): void {
    this._http.post('api/POS/DownloadTitlePage', titlePage, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, `${titlePage.objectCipher}Титульник.doc`);
    }, error => console.error(error));
  }
}
