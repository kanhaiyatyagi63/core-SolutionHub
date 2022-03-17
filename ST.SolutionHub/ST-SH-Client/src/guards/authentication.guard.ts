import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { AuthenticationService } from 'services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate {
  constructor(private router: Router, private authenticationService: AuthenticationService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // If use is authenticated and token not expired
    // then proceed to page
    debugger;
    const tt = this.authenticationService.isLoggedIn();
    const ff = this.authenticationService.isTokenExpired();
    if (!this.authenticationService.isLoggedIn() || this.authenticationService.isTokenExpired()) {
      this.authenticationService.logout().subscribe(() => {
        this.router.navigate(['/account/login'], { queryParams: { returnUrl: state.url } });
      });
      return false;
    }
    return true;
  }
}
