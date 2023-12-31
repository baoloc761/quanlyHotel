import { Routes } from '@angular/router';
import { Dashboard } from './DashBoard/mainDashboard.component';
import { LoginComponent } from './LoginHotel/login.component';
import { AuthGuard } from './_helpers';
import { RegisterComponent } from './LoginHotel/register.component';
import { UsersComponent } from './UsersManagement/users.component';

export const routes: Routes = [
  { path: '', component: Dashboard, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'account/login', component: LoginComponent },
  { path: 'account/register', component: RegisterComponent },
  { path: 'account/users', component: UsersComponent },

  // otherwise redirect to dashboard
  { path: '**', redirectTo: '' }
];
