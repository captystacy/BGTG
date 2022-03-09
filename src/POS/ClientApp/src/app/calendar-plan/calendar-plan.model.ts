export interface ICalendarPlan {
  constructionStartDate: string;
  constructionDuration: number;
  calendarWorks: ICalendarWork[];
  totalWorkChapter: number;
}

export interface ICalendarWork {
  workName: string;
  chapter: number;
  percentages: IPercentage[];
}

export interface IPercentage {
  value: number;
}
