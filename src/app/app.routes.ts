import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './LoginHotel/app.component';
import { mainDashboard } from './DashBoard/mainDashboard.component';

export const routes: Routes = [
  { path: '#login', component: AppComponent },
  { path: '#trang-quan-tri', component: mainDashboard }
];
