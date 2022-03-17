/*!

=========================================================
* Now UI Kit Angular - v1.3.0
=========================================================

* Product Page: https://www.creative-tim.com/product/now-ui-kit-angular
* Copyright 2020 Creative Tim (https://www.creative-tim.com)
* Licensed under MIT (https://github.com/creativetimofficial/now-ui-kit-angular/blob/master/LICENSE.md)

* Coded by Creative Tim

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
function addScript(link) {
  const script = document.createElement('script');
  script.type = 'text/javascript';
  script.src = link;
  document.head.append(script);
}

function addLink(css) {
  const link = document.createElement('link');
  link.href = css;
  link.rel = 'stylesheet';
  document.head.append(link);
}

addScript('https://code.jquery.com/jquery-3.4.1.min.js');
addScript('https://cdn.jsdelivr.net/npm/summernote@0.8.15/dist/summernote-lite.min.js');
addLink('https://cdn.jsdelivr.net/npm/summernote@0.8.15/dist/summernote-lite.min.css')