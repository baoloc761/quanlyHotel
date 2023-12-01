import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AngularMaterialModule } from './angular-material.module';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { RegisterComponent } from './LoginHotel/register.component';
import { LoginComponent } from './LoginHotel/login.component';
import { Dashboard } from './DashBoard/mainDashboard.component';
import { ErrorInterceptor, JwtInterceptor, fakeBackendProvider } from './_helpers';
import { UsersComponent } from './UsersManagement/users.component';


@NgModule({
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AngularMaterialModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    Dashboard,
    RegisterComponent,
    UsersComponent
  ],
  providers: [ 
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    fakeBackendProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
