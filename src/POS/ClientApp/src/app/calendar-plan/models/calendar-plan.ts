import { ICalendarWork } from "./calendar-work"

export interface ICalendarPlan {
  constructionStartDate: Date,
  constructionDuration: number,
  constructionDurationCeiling: number,
  calendarWorkViewModels: ICalendarWork[],
  totalWorkChapter: number,
}
