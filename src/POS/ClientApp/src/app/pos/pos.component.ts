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

  readonly projectTemplates =
    [
      { title: "СКЗ", value: 0 },
      { title: "СКЗ (мини)", value: 1 },
      { title: "ТЛМ (мини)", value: 2 },
      { title: "ШРП", value: 3 }
    ];
  readonly engineers =
    [
      { title: "Неизвестно", value: 0 },
      { title: "Близнюк", value: 6 },
      { title: "Вусик", value: 13 },
      { title: "Гомонов", value: 8 },
      { title: "Дмитрик", value: 11 },
      { title: "Игнатенко", value: 10 },
      { title: "Каленик", value: 9 },
      { title: "Капитан", value: 2 },
      { title: "Морозюк", value: 12 },
      { title: "Пигальская", value: 7 },
      { title: "Прищеп", value: 3 },
      { title: "Сайко", value: 5 },
      { title: "Селиванова", value: 4 }
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
