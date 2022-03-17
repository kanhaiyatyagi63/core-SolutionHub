import { Component, OnInit, ElementRef, Input } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'services/authentication.service';
import { AuthenticateResponseModel } from 'models/authenticationModel';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
    @Input() IsValid: boolean;
    private toggleButton: any;
    private sidebarVisible: boolean;
    user: AuthenticateResponseModel;
    isValidUser: boolean;

    constructor(public location: Location, private router: Router, private element: ElementRef,
        private authenticationService: AuthenticationService,
        private route: ActivatedRoute) {
        this.sidebarVisible = false;
        // this.authenticationService.user$.subscribe((user) => {
        //     debugger;
        //     this.isValidUser = user != null && !this.authenticationService.isTokenExpired();
        //     if (user === null) {
        //         this.router.navigate(['/account/logout']);
        //     }
        //     this.user = user;
        // });
    }

    ngOnInit() {
        const navbar: HTMLElement = this.element.nativeElement;
        this.toggleButton = navbar.getElementsByClassName('navbar-toggler')[0];
    }
    sidebarOpen() {
        const toggleButton = this.toggleButton;
        const html = document.getElementsByTagName('html')[0];
        setTimeout(function () {
            toggleButton.classList.add('toggled');
        }, 500);
        html.classList.add('nav-open');

        this.sidebarVisible = true;
    };
    sidebarClose() {
        const html = document.getElementsByTagName('html')[0];
        // console.log(html);
        this.toggleButton.classList.remove('toggled');
        this.sidebarVisible = false;
        html.classList.remove('nav-open');
    };
    sidebarToggle() {
        // const toggleButton = this.toggleButton;
        // const body = document.getElementsByTagName('body')[0];
        if (this.sidebarVisible === false) {
            this.sidebarOpen();
        } else {
            this.sidebarClose();
        }
    };

    isDocumentation() {
        var titlee = this.location.prepareExternalUrl(this.location.path());
        if (titlee === '/documentation') {
            return true;
        }
        else {
            return false;
        }
    }
}
