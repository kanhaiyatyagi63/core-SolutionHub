import { NewProjectTypesComponent } from './components/project-types/new-project-types/new-project-types.component';
import { FotterComponent } from './shared/fotter/fotter.component';
import { ModalComponent } from './shared/Modal/Modal.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing';
import { ComponentsModule } from './components/components.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from 'interceptors/jwt.interceptor';
import { AuthenticationService } from 'services/authentication.service';
import { environment } from 'environments/environment';
import { AppInitializer } from 'services/app.initializer';
import { GoogleLoginProvider, SocialAuthServiceConfig, SocialLoginModule } from 'angularx-social-login';
import { NgxSummernoteModule } from 'ngx-summernote';

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        ModalComponent
    ],
    imports: [
        BrowserAnimationsModule,
        NgxSummernoteModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AppRoutingModule,
        ComponentsModule,
        HttpClientModule,
        SocialLoginModule
    ],
    providers: [{ provide: APP_INITIALIZER, useFactory: AppInitializer, multi: true, deps: [AuthenticationService] },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    {
        provide: 'SocialAuthServiceConfig',
        useValue: {
            autoLogin: false,
            providers: [
                {
                    id: GoogleLoginProvider.PROVIDER_ID,
                    provider: new GoogleLoginProvider(
                        environment.googleClientId
                    ),
                }
            ],
        } as SocialAuthServiceConfig
    }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
