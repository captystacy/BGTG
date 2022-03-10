import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

import { AlertService } from '../_alert/alert.service';
import { IOperationResult } from '../operation-result.model';
import { ICalendarPlan, ICalendarWork } from './calendar-plan.model';

const ONE_MONTH_PERCENT: number = 100;

@Injectable({
  providedIn: 'root'
})
export class CalendarPlanService {
  private _initialPercentageValue!: number;
  private _totalWorkChapter!: string;

  private _estimateFiles!: File[];

  public get estimateFiles(): File[] {
    return this._estimateFiles;
  }

  public set estimateFiles(value: File[]) {
    this._calendarPlan = undefined as any;
    this._months = [];
    this._estimateFiles = value;
    this._columnPercentages = [];
    this._totalPercentages = [];
    this._calendarPlanIsFetched = false;
    this._constructionStartDateIsCorrupted = false;
    this._constructionDurationIsCorrupted = false;
  }

  public get totalWorkChapter() {
    return this._totalWorkChapter;
  }

  public set totalWorkChapter(value: string) {
    this._totalWorkChapter = value;
  }

  private _calendarPlan!: ICalendarPlan;

  public get calendarPlan(): ICalendarPlan {
    return this._calendarPlan;
  }

  private _columnPercentages: number[] = [];

  public get columnPercentages(): number[] {
    return this._columnPercentages;
  }

  private _months: string[] = [];

  public get months() {
    return this._months;
  }

  private _totalPercentages: number[] = [];

  public get totalPercentages(): number[] {
    return this._totalPercentages;
  }

  private _calendarPlanIsFetched: boolean = false;

  public get calendarPlanIsFetched(): boolean {
    return this._calendarPlanIsFetched;
  }

  private _constructionStartDateIsCorrupted: boolean = false;

  public get constructionStartDateIsCorrupted(): boolean {
    return this._constructionStartDateIsCorrupted;
  }

  private _constructionDurationIsCorrupted: boolean = false;

  public get constructionDurationIsCorrupted(): boolean {
    return this._constructionDurationIsCorrupted;
  }

  constructor(
    private _http: HttpClient,
    private _alertService: AlertService) {
  }

  public fetchCalendarPlan(): void {
    if (this.constructionStartDateIsCorrupted
      || this.constructionDurationIsCorrupted) {
      this.configureCalendarPlan();
      return;
    }

    if (this._calendarPlanIsFetched) {
      this.downloadCalendarPlan();
      return;
    }

    let formData = new FormData();
    this.appendEstimateFiles(formData);
    this.appendTotalWorkChapter(formData);

    this._http.post<IOperationResult>('api/CalendarPlan/GetViewModelForCreation', formData).subscribe(operation => {
      if (!operation.ok) {
        this._alertService.error(operation.metadata.message);
        return;
      }

      this._calendarPlan = operation.result;

      this.configureCalendarPlan();
    }, error => console.error(error));
  }

  private configureCalendarPlan(): void {
    this._constructionStartDateIsCorrupted = new Date(Date.parse(this._calendarPlan.constructionStartDate)).getFullYear() <= 1900;
    this._constructionDurationIsCorrupted = this._calendarPlan.constructionDuration === 0;

    if (this._calendarPlan.constructionDuration <= 1) {
      this._initialPercentageValue = ONE_MONTH_PERCENT;
    } else {
      this._initialPercentageValue = 0;
    }

    this.setCalendarWorksPercentagesInitialValues();
    this.setColumnPercentages();
    this.setMonths();

    this._calendarPlanIsFetched = true;
  }

  public downloadCalendarPlan(): void {
    let formData = this.generateFormDataForRequest();
    this._http.post('api/CalendarPlan/GetFile', formData, { responseType: 'blob' }).subscribe(data => {
      saveAs(data, "Календарный план");
    },
      error => console.error(error));
  }

  public fetchTotalPercentages() {
    let formData = this.generateFormDataForRequest();
    this._http.post<IOperationResult>('api/CalendarPlan/GetTotalPercentages', formData).subscribe(operation => {
      if (!operation.ok) {
        this._alertService.error(operation.metadata.message);
        return;
      }
      this._totalPercentages = operation.result.map((x: number) => `${(x * 100).toFixed(2)} %`);
    },
      error => console.error(error));
  }

  private setColumnPercentages(): void {
    this._columnPercentages = [];
    for (let i = 0; i < this._calendarPlan.constructionDuration; i++) {
      this._columnPercentages.push(this._initialPercentageValue);
    }
  }

  private setMonths(): void {
    this._months = [];
    let milliseconds = Date.parse(this._calendarPlan.constructionStartDate);
    let currentDate = new Date(milliseconds);
    let formatter = new Intl.DateTimeFormat('ru', { month: 'long', year: 'numeric' });
    for (let i = 0; i < this._calendarPlan.constructionDuration + 1; i++) {
      var monthYearStr = formatter.format(currentDate);
      currentDate.setMonth(currentDate.getMonth() + 1);
      this._months.push(monthYearStr[0].charAt(0).toUpperCase() + monthYearStr.slice(1));
    }
  }

  private setCalendarWorksPercentagesInitialValues(): void {
    this._calendarPlan.calendarWorks.forEach((x: ICalendarWork) => {
      x.percentages = [];
      for (let i = 0; i < this._calendarPlan.constructionDuration; i++) {
        x.percentages.push({ value: this._initialPercentageValue });
      }
    });
  }

  private generateFormDataForRequest(): FormData {
    let formData = new FormData();
    this.appendEstimateFiles(formData);
    this.appendCalendarWorks(formData);
    this.appendConstructionStartDateAndDurationAndDurationCeiling(formData);
    this.appendTotalWorkChapter(formData);
    return formData;
  }

  private appendConstructionStartDateAndDurationAndDurationCeiling(formData: FormData): void {
    formData.append('ConstructionStartDate', this._calendarPlan.constructionStartDate.toString());
    formData.append('ConstructionDuration', this._calendarPlan.constructionDuration.toString());
  }

  private appendTotalWorkChapter(formData: FormData): void {
    formData.append('TotalWorkChapter', this._totalWorkChapter);
  }

  private appendCalendarWorks(formData: FormData): void {
    for (let i = 0; i < this._calendarPlan.calendarWorks.length; i++) {
      formData.append(`CalendarWorks[${i}].WorkName`, this._calendarPlan.calendarWorks[i].workName);
      for (var j = 0; j < this._calendarPlan.calendarWorks[i].percentages.length; j++) {
        formData.append(`CalendarWorks[${i}].Percentages[${j}]`,
          (this._calendarPlan.calendarWorks[i].percentages[j].value / 100).toString());
      }
    }
  }

  private appendEstimateFiles(formData: FormData): void {
    for (let i = 0; i < this._estimateFiles.length; i++) {
      formData.append('EstimateFiles', this._estimateFiles[i]);
    }
  }
}
