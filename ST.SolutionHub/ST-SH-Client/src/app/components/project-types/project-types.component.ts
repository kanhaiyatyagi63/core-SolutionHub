import { Subscription } from 'rxjs';
import { ProjectTypesService } from './../../../services/project-types.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import * as Rellax from 'rellax';
import { ProjectTypeViewModel } from 'models/project-types/projectTypeViewModel';
@Component({
  selector: 'app-project-types',
  templateUrl: './project-types.component.html',
  styleUrls: ['./project-types.component.scss']
})
export class ProjectTypesComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  projectTypes: ProjectTypeViewModel[] = [];
  constructor(private projectTypeService: ProjectTypesService) { }

  ngOnInit() {

    this.subscription = this.projectTypeService.getProjectTypes().subscribe((data: ProjectTypeViewModel[]) => {
      console.log(data);
      if (data && data.length > 0) {
        this.projectTypes = data;
      }
    },
      (error: any) => console.log(error));
    var rellaxHeader = new Rellax('.rellax-header');

    var body = document.getElementsByTagName('body')[0];
    body.classList.add('profile-page');
    var navbar = document.getElementsByTagName('nav')[0];
    navbar.classList.add('navbar-transparent');
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
    var body = document.getElementsByTagName('body')[0];
    body.classList.remove('profile-page');
    var navbar = document.getElementsByTagName('nav')[0];
    navbar.classList.remove('navbar-transparent');
  }
  public createImgPath = (serverPath: string) => {
    return `https://localhost:5001/${serverPath}`;
  }
}
