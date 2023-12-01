import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import * as lodash from 'lodash';

// This makes `_` available globally as a TS type:
declare global {
  const _: typeof lodash;
}

// And this makes `_` available globally as a JS object:
(window as any)['_'] = lodash;

// Or use this in case browser is not the only target platform:
(globalThis as any)['_'] = lodash;

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
