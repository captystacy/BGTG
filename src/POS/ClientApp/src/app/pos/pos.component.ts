import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { POSService } from "./pos.service";

@Component({
  selector: 'app-pos',
  templateUrl: './pos.component.html',
  styleUrls: ['./pos.component.css']
})
export class POSComponent {

  constructor(private _posService: POSService) { }

  calculationFilesInput!: HTMLInputElement;

  posForm = new FormGroup({
    objectCipher: new FormControl('', [Validators.required]),
    objectName: new FormControl('', [Validators.required]),
    projectTemplate: new FormControl('', [Validators.required]),
    projectEngineer: new FormControl('', [Validators.required]),
    chiefProjectEngineer: new FormControl('', [Validators.required]),
    householdTownIncluded: new FormControl(true),
  });

  get objectCipher() {
    return this.posForm.get('objectCipher')!;
  }

  get objectName() {
    return this.posForm.get('objectName')!;
  }

  get projectTemplate() {
    return this.posForm.get('projectTemplate')!;
  }

  get projectEngineer() {
    return this.posForm.get('projectEngineer')!;
  }

  get chiefProjectEngineer() {
    return this.posForm.get('chiefProjectEngineer')!;
  }

  get householdTownIncluded() {
    return this.posForm.get('householdTownIncluded')!;
  }

  readonly projectTemplates =
    [
      { title: "ЭХЗ", value: 0 }
    ];

  readonly projectEngineers =
    [
      { title: "Неизвестно", value: 0 },
      { title: "Капитан", value: 1 },
      { title: "Прищеп", value: 2 },
      { title: "Селиванова", value: 3 }
    ];

  readonly chiefProjectEngineers =
    [
      { title: "Сайко", value: 0 }
    ];

  downloadProject(): void {
    this._posService.downloadProject(this.posForm.value, this.calculationFilesInput.files!);
  }

  downloadTableOfContents(): void {
    this._posService.downloadTableOfContents(this.posForm.value);
  }

  downloadTitlePage(): void {
    this._posService.downloadTitlePage(this.posForm.value);
  }
}
