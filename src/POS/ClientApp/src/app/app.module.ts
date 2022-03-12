import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MaterialFileInputModule } from 'ngx-material-file-input';
import { MatRadioModule } from '@angular/material/radio';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AlertModule } from './_alert';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { EstimateCalculationsComponent } from './estimate-calculations/estimate-calculations.component';
import { DurationByTCPComponent as DurationByTcpComponent } from './duration-by-tcp/duration-by-tcp.component';
import { POSComponent } from './pos/pos.component';
import { CalendarPlanComponent } from './calendar-plan/calendar-plan.component';
import { NetworkInterceptor } from './network.interceptor';
import { DurationByLCComponent } from './duration-by-lc/duration-by-lc.component';
import { EnergyAndWaterComponent } from './energy-and-water/energy-and-water.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    EstimateCalculationsComponent,
    DurationByTcpComponent,
    POSComponent,
    CalendarPlanComponent,
    DurationByLCComponent,
    EnergyAndWaterComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'estimate-calculations', pathMatch: 'full' },
      { path: 'estimate-calculations', component: EstimateCalculationsComponent },
      { path: 'duration-by-tcp', component: DurationByTcpComponent },
      { path: 'pos', component: POSComponent }
    ]),
    BrowserAnimationsModule,
    MatTabsModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatFormFieldModule,
    MatButtonModule,
    MatCheckboxModule,
    MaterialFileInputModule,
    MatRadioModule,
    AlertModule,
    MatProgressBarModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: NetworkInterceptor,
      multi: true,
    }
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
