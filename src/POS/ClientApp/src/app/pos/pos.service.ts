import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { saveAs } from 'file-saver';

import { IProject, ITableOfContents, ITitlePage } from "./pos.model";

@Injectable({
  providedIn: 'root'
})
export class POSService {
  constructor(private _http: HttpClient) { }

  downloadProject(project: IProject, calculationFiles: FileList): void {
    let formData = new FormData();

    for (let i = 0; i < calculationFiles.length; i++) {
      formData.append('CalculationFiles', calculationFiles[i]);
    }
    formData.append('ObjectCipher', project.objectCipher);
    formData.append('ProjectTemplate', project.projectTemplate.toString());
    formData.append('ProjectEngineer', project.projectEngineer.toString());
    formData.append('NormalInspectionEngineer', project.normalInspectionEngineer.toString());
    formData.append('ChiefEngineer', project.chiefEngineer.toString());
    formData.append('ChiefProjectEngineer', project.chiefProjectEngineer.toString());

    let projectName: string;
    switch (project.projectTemplate) {
      case 0:
        projectName = "ЭХЗ";
        break;
      case 1:
        projectName = "ШРП";
        break;
      case 2:
        projectName = "ТЛМ";
        break;
      default:
        projectName = "Проект";
    }

    this._http.post('api/POS/DownloadProject', formData, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, projectName + ".doc");
    }, error => console.error(error));
  }

  downloadTableOfContents(tableOfContents: ITableOfContents): void {
    this._http.post('api/POS/DownloadTableOfContents', tableOfContents, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, "Содержание.doc");
    }, error => console.error(error));
  }

  downloadTitlePage(titlePage: ITitlePage): void {
    this._http.post('api/POS/DownloadTitlePage', titlePage, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, "Титульник.doc");
    }, error => console.error(error));
  }
}
