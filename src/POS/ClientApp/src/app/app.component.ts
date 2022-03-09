import { Component } from '@angular/core';
import { LoadingService } from './loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  loading$ = this.loadingService.loading$;

  constructor(public loadingService: LoadingService) { }
}
