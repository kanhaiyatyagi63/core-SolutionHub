import { NewProjectTypesComponent } from './components/project-types/new-project-types/new-project-types.component';
import { BasicelementsComponent } from './components/basicelements/basicelements.component';
import { TypographyComponent } from './components/typography/typography.component';
import { DetailProjectComponent } from './components/project/detail-project/detail-project.component';
import { LogoutComponent } from './components/Logout/Logout.component';
import { ProjectTypesComponent } from './components/project-types/project-types.component';
import { ProjectComponent } from './components/project/project.component';
import { FotterComponent } from './shared/fotter/fotter.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { ComponentsComponent } from './components/components.component';
import { LoginComponent } from './components/login/login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AuthenticationGuard } from 'guards/authentication.guard';
import { EditProjectTypesComponent } from './components/project-types/edit-project-types/edit-project-types.component';

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: ComponentsComponent, canActivate: [AuthenticationGuard] },
    { path: 'typo', component: TypographyComponent, canActivate: [AuthenticationGuard] },
    { path: 'basic', component: BasicelementsComponent, canActivate: [AuthenticationGuard] },
    { path: 'account/login', component: LoginComponent },
    { path: 'account/logout', component: LogoutComponent },
    { path: 'profile', component: ProfileComponent },
    { path: 'nav', component: NavigationComponent, canActivate: [AuthenticationGuard] },
    { path: 'fotter', component: FotterComponent },
    { path: 'new-project-type', component: NewProjectTypesComponent },
    { path: 'edit-project-type/:id', component: EditProjectTypesComponent },

    { path: 'project-type', component: ProjectTypesComponent, canActivate: [AuthenticationGuard] },
    { path: 'project-list', component: ProjectComponent, canActivate: [AuthenticationGuard] },
    { path: 'project-detail/:id', component: DetailProjectComponent, canActivate: [AuthenticationGuard] }
];

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        RouterModule.forRoot(routes)
    ],
    exports: [
    ],
})
export class AppRoutingModule { }
