import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { IOperationResult } from '../operation-result';
import { ICalendarPlan } from './models/calendar-plan';

@Component({
  selector: 'app-calendar-plan',
  templateUrl: './calendar-plan.component.html',
  styleUrls: ['./calendar-plan.component.css']
})
export class CalendarPlanComponent {
  @Input() private calendarPlan?: ICalendarPlan;
  private estimateFiles?: File[];
  totalWorkChapter = new FormControl();
  private http: HttpClient;

  constructor(http: HttpClient) {
    this.http = http;
  }

  download() {
    let formData = new FormData();

    for (let i = 0; i < this.estimateFiles!.length; i++) {
      formData.append('EstimateFiles', this.estimateFiles![i]);
    }
    formData.append('TotalWorkChapter', this.totalWorkChapter.value);

    this.http.post<IOperationResult>('api/CalendarPlan/GetViewModelForCreation', formData).subscribe(operation => {
      if (!operation.ok) {
        throw 'something went wrong';
      }

      this.calendarPlan = operation.result;
    }, error => console.error(error));
  }
}
