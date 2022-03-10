import { Component, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { IDurationByLC } from "./duration-by-lc.model";
import { DurationByLCService } from "./duration-by-lc.service";

@Component({
  selector: 'app-duration-by-lc',
  templateUrl: './duration-by-lc.component.html',
  styleUrls: ['./duration-by-lc.component.css']
})
export class DurationByLCComponent {
  @Input() estimateFiles!: File[];

  durationByLCForm = new FormGroup({
    numberOfWorkingDays: new FormControl(21.5, [Validators.required]),
    workingDayDuration: new FormControl(8, [Validators.required]),
    shift: new FormControl(1.5, [Validators.required]),
    numberOfEmployees: new FormControl(4, [Validators.required]),
    technologicalLaborCosts: new FormControl(),
    acceptanceTimeIncluded: new FormControl(true, [Validators.required]),
  });

  constructor(private _durationByLCService: DurationByLCService) { }

  downloadDurationByLC(): void {
    let a = this.durationByLCForm.value;

    let b = 0;
    //this._durationByLCService.downloadDurationByLc(undefined as any, this.estimateFiles);
  }
}
