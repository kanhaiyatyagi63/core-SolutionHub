import { Router, ActivatedRoute } from '@angular/router';
import { ProjectModel } from './../../../../models/project/projectModel';
import { Subscription } from 'rxjs';
import { ProjectService } from './../../../../services/project.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import * as Rellax from 'rellax';

@Component({
  selector: 'app-detail-project',
  templateUrl: './detail-project.component.html',
  styleUrls: ['./detail-project.component.scss']
})
export class DetailProjectComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  id: any;
  constructor(private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.id = params.id
    });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    var body = document.getElementsByTagName('body')[0];
    body.classList.remove('profile-page');
    var navbar = document.getElementsByTagName('nav')[0];
    navbar.classList.remove('navbar-transparent');
  }

  ngOnInit() {
    this.subscription = this.projectService.getProjectById(this.id)
      .subscribe((data: ProjectModel) => {
        console.log("project detail");
        console.log(data);
      }, (error: any) => {
        console.log(error);
      });
    var rellaxHeader = new Rellax('.rellax-header');

    var body = document.getElementsByTagName('body')[0];
    body.classList.add('profile-page');
    var navbar = document.getElementsByTagName('nav')[0];
    navbar.classList.add('navbar-transparent');
  }
}
