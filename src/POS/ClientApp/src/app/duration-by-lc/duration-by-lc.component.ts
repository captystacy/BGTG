import { Component, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { DurationByLCService } from "./duration-by-lc.service";

@Component({
  selector: 'app-duration-by-lc',
  templateUrl: './duration-by-lc.component.html',
  styleUrls: ['./duration-by-lc.component.css']
})
export class DurationByLCComponent {
  @Input() estimateFiles?: FileList;

  durationByLCForm = new FormGroup({
    numberOfWorkingDays: new FormControl(21.5, [Validators.required, Validators.min(1)]),
    workingDayDuration: new FormControl(8, [Validators.required, Validators.min(0.1)]),
    shift: new FormControl(1.5, [Validators.required, Validators.min(0.1)]),
    numberOfEmployees: new FormControl(4, [Validators.required, Validators.min(1)]),
    technologicalLaborCosts: new FormControl(),
    acceptanceTimeIncluded: new FormControl(true),
  });

  get numberOfWorkingDays() {
    return this.durationByLCForm.get('numberOfWorkingDays')!;
  }

  get workingDayDuration() {
    return this.durationByLCForm.get('workingDayDuration')!;
  }

  get shift() {
    return this.durationByLCForm.get('shift')!;
  }

  get numberOfEmployees() {
    return this.durationByLCForm.get('numberOfEmployees')!;
  }

  get technologicalLaborCosts() {
    return this.durationByLCForm.get('technologicalLaborCosts')!;
  }

  get acceptanceTimeIncluded() {
    return this.durationByLCForm.get('acceptanceTimeIncluded')!;
  }

  constructor(private _durationByLCService: DurationByLCService) { }

  downloadDurationByLC(): void {
    this._durationByLCService.downloadDurationByLC(this.durationByLCForm.value, this.estimateFiles!);
  }
}
