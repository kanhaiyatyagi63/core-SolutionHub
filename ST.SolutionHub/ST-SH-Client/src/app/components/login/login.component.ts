import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {


  data: Date = new Date();
  focus;
  focus1;
  user = null;
  form: FormGroup;
  subscriptions: Subscription[] = [];

  forgotPasswordForm: FormGroup;
  submitted = false;
  isforgotPasswordFormSubmitted = false;
  showForgotPasswordMessage = false;
  returnUrl: any;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder,
    private socialauthService: SocialAuthService) {

    this.route.queryParams.subscribe(params => {
      this.returnUrl = params.returnUrl;
    });
  }


  ngOnInit() {
    const body = document.getElementsByTagName('body')[0];
    body.classList.add('login-page');
    const navbar = document.getElementsByTagName('nav')[0];
    navbar.classList.add('navbar-transparent');
    const socialauthSubscription = this.socialauthService.authState.subscribe((user) => {
      this.user = user;
      if (this.user != null) {
        // this.blockUI.start();
        this.authenticationService.externalLogin(this.user)
          .subscribe((isLoggedIn: boolean) => {
            this.socialauthService.signOut();
            if (!isLoggedIn) {
              return;
            }
            if (this.returnUrl && this.returnUrl !== '/account/logout') {
              this.router.navigate([this.returnUrl]);
            } else {
              this.router.navigate(['/index']);
            }
            // this.blockUI.stop();
          }, error => {
            console.log(error);
          });
      }
    });
    this.subscriptions.push(socialauthSubscription);
  }
  ngOnDestroy() {
    const body = document.getElementsByTagName('body')[0];
    body.classList.remove('login-page');

    const navbar = document.getElementsByTagName('nav')[0];
    navbar.classList.remove('navbar-transparent');
    this.subscriptions.forEach(item => item.unsubscribe());
  }
  signInWithGoogle() {
    this.socialauthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

}
