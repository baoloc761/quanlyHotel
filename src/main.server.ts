import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/LoginHotel/app.component';
import { config } from './app/app.config.server';
import 'localstorage-polyfill';

const bootstrap = () => bootstrapApplication(AppComponent, config);

export default bootstrap;
