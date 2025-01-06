import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { format } from 'path';
import { LoginResponse } from 'src/app/Models/LoginResponse';
import { AppService } from 'src/app/service/app.service';
import { AuthenticationService } from 'src/app/service/authentication.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
   animations: [
          trigger('toggleAnimation', [
              transition(':enter', [style({ opacity: 0, transform: 'scale(0.95)' }), animate('100ms ease-out', style({ opacity: 1, transform: 'scale(1)' }))]),
              transition(':leave', [animate('75ms', style({ opacity: 0, transform: 'scale(0.95)' }))]),
          ]),
      ],
})
export class LoginComponent implements OnInit {
  store: any;
  Form! : FormGroup
    constructor(public translate: TranslateService, public storeData: Store<any>,
        public router: Router, private appSetting: AppService , private fb : FormBuilder , private auth: AuthenticationService) {
        this.initStore();
    }
    async initStore() {
        this.storeData
            .select((d) => d.index)
            .subscribe((d) => {
                this.store = d;
            });
    }


    ngOnInit(): void {
        this.Form = this.fb.group({
            username: ['', [Validators.required, Validators.minLength(3)]],
            password: ['', [Validators.required, Validators.minLength(6)]],
          });
        }


        submitForm(): void {
            if (this.Form.invalid) {
              Swal.fire({
                icon: 'warning',
                title: "Wrong username or password",
                text: 'Please Try Again'
              });
              return;
            
            }
            
        
        
            this.auth.login( this.Form.value).subscribe({
                

                
              next: (response: any) => {
                if (response.status !== 'ok') {
                    debugger;
                  Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Wrong username or password',
                    footer: '<a href="#">Why do I have this issue?</a>'
                  });
                  return;
                }
        
                // Log response for debugging
                console.log("Response:", response);
        
        
                const userRole = response.role;
                if (userRole == 'Admin') {
                  setTimeout(() => {
                    this.router.navigate(['']);
                  }, 0); 
                } else if (userRole == 'User') {
                  setTimeout(() => {
                    this.router.navigate(['']);
                  }, 0);
                }
        
                // Store user details in local storagep
                localStorage.setItem('Username', response.username);
                localStorage.setItem('Token', response.token);
                localStorage.setItem('Role', response.role);
                localStorage.setItem('Refreshtoken', response.refreshToken);
                localStorage.setItem('TokenExpires', response.tokenExpires);
        
                // Notify authentication state
                // this.auth.isloggedin.next(true);
        
                // Redirect to home
                this.router.navigate(['']);
              },
              error: (err) => {
                console.error('Login error:', err);
                Swal.fire({
                  icon: 'error',
                  title: 'Login Failed',
                  text: 'Please check your credentials and try again.'
                });
              }
            });
          }
    
    

    changeLanguage(item: any) {
        this.translate.use(item.code);
        this.appSetting.toggleLanguage(item);
        if (this.store.locale?.toLowerCase() === 'ae') {
            this.storeData.dispatch({ type: 'toggleRTL', payload: 'rtl' });
        } else {
            this.storeData.dispatch({ type: 'toggleRTL', payload: 'ltr' });
        }
        window.location.reload();
    }
}
