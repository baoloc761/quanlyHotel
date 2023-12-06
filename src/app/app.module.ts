import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AngularMaterialModule } from './angular-material.module';

import { TranslateLoader, TranslateModule} from '@ngx-translate/core'
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';

import { RegisterComponent } from './LoginHotel/register.component';
import { LoginComponent } from './LoginHotel/login.component';
import { Dashboard } from './DashBoard/mainDashboard.component';
import { DetailsUsersComponent } from './DetailsUsers/detailsUser.component';
import { ErrorInterceptor, JwtInterceptor, fakeBackendProvider } from './_helpers';
import { UsersComponent } from './UsersManagement/users.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { UserEditComponent } from './UsersManagement/edit/user-edit.component';
import { PageNotFoundComponent } from './404/404.component';
import { authorrizationUserComponent } from './UsersManagement/authorizationUser/authorrizationUser.component';


@NgModule({
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AngularMaterialModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatDialogModule,
    MatSnackBarModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    })
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    Dashboard,
    RegisterComponent,
    UsersComponent,
    DetailsUsersComponent,
    UserEditComponent,
    authorrizationUserComponent,
    PageNotFoundComponent
  ],
  providers: [ 
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    fakeBackendProvider
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }


export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json')
}