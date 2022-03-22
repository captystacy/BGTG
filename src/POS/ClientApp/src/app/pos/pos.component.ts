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
    normalInspectionEngineer: new FormControl('', [Validators.required]),
    chiefEngineer: new FormControl('', [Validators.required]),
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

  get chiefProjectEngineer() {
    return this.posForm.get('chiefProjectEngineer')!;
  }

  get chiefEngineer() {
    return this.posForm.get('chiefEngineer')!;
  }

  get normalInspectionEngineer() {
    return this.posForm.get('normalInspectionEngineer')!;
  }

  get projectEngineer() {
    return this.posForm.get('projectEngineer')!;
  }

  get householdTownIncluded() {
    return this.posForm.get('householdTownIncluded')!;
  }

  readonly projectTemplates =
    [
      { title: "ЭХЗ", value: 0 },
      { title: "ШРП", value: 1 }
    ];

  readonly engineers =
    [
      { title: "Неизвестно", value: 0 },
      { title: "Капитан", value: 2 },
      { title: "Прищеп", value: 3 },
      { title: "Селиванова", value: 4 },
      { title: "Сайко", value: 5 },
      { title: "Близнюк", value: 6 }
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
