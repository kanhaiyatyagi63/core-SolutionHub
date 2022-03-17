import { BasicelementsComponent } from './basicelements/basicelements.component';
import { NewProjectTypesComponent } from './project-types/new-project-types/new-project-types.component';
import { DetailProjectTypesComponent } from './project-types/detail-project-types/detail-project-types.component';
import { EditProjectTypesComponent } from './project-types/edit-project-types/edit-project-types.component';
import { EditProjectComponent } from './project/edit-project/edit-project.component';
import { DetailProjectComponent } from './project/detail-project/detail-project.component';
import { NewProjectComponent } from './project/new-project/new-project.component';
import { LogoutComponent } from './Logout/Logout.component';
import { ProjectTypesComponent } from './project-types/project-types.component';
import { ProjectComponent } from './project/project.component';
import { FotterComponent } from './../shared/fotter/fotter.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NouisliderModule } from 'ng2-nouislider';
import { JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './navigation/navigation.component';
import { TypographyComponent } from './typography/typography.component';
import { ComponentsComponent } from './components.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './login/login.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        NgbModule,
        NouisliderModule,
        RouterModule,
        JwBootstrapSwitchNg2Module
    ],
    declarations: [
        ComponentsComponent,
        NavigationComponent,
        TypographyComponent,
        ProfileComponent,
        LoginComponent,
        ProjectComponent,
        FotterComponent,
        ProjectTypesComponent,
        EditProjectTypesComponent,
        DetailProjectTypesComponent,
        NewProjectTypesComponent,
        LogoutComponent,
        ProjectComponent,
        NewProjectComponent,
        DetailProjectComponent,
        EditProjectComponent,
        BasicelementsComponent
    ],
    exports: [ComponentsComponent]
})
export class ComponentsModule { }
