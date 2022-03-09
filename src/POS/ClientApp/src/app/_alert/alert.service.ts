import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { filter } from 'rxjs/operators';

import { Alert, AlertType } from './alert.model';

@Injectable({ providedIn: 'root' })
export class AlertService {
  private _subject = new Subject<Alert>();
  private _defaultId = 'default-alert';

  // enable subscribing to alerts observable
  onAlert(id = this._defaultId): Observable<Alert> {
    return this._subject.asObservable().pipe(filter(x => x && x.id === id));
  }

  // convenience methods
  success(message: string, options?: any) {
    this.alert(new Alert({ ...options, type: AlertType.Success, message }));
  }

  error(message: string, options?: any) {
    this.alert(new Alert({ ...options, type: AlertType.Error, message }));
  }

  info(message: string, options?: any) {
    this.alert(new Alert({ ...options, type: AlertType.Info, message }));
  }

  warn(message: string, options?: any) {
    this.alert(new Alert({ ...options, type: AlertType.Warning, message }));
  }

  // main alert method    
  alert(alert: Alert) {
    alert.id = alert.id || this._defaultId;
    this._subject.next(alert);
  }

  // clear alerts
  clear(id = this._defaultId) {
    this._subject.next(new Alert({ id }));
  }
}
