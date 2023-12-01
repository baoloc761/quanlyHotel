import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/LoginHotel/app.component';
import _ from 'lodash';

// This makes `_` available globally as a TS type:
declare global {
  const _: typeof _;
}

// And this makes `_` available globally as a JS object:
(window as any)['_'] = _;

// Or use this in case browser is not the only target platform:
(globalThis as any)['_'] = _;

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
