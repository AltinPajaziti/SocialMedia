import { Injectable } from '@angular/core';
import { Envirements } from '../Envirements/Envirements';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginModel } from '../Models/LoginModel';
import { LoginResponse } from '../Models/LoginResponse';
import { Observable, catchError } from 'rxjs';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public api = Envirements.api_Url + 'Authentication/'

  constructor(private http : HttpClient , private router : Router) { }


  Login(Login : LoginModel){
    this.http.post<LoginResponse>(this.api , {})
  }


  login(login: LoginModel): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.api + 'Login', login, {
      headers: { 'Content-Type': 'application/json' }
    }).pipe(
      catchError(err => {
        console.error('Login error:', err);
        if (err.error) {
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Wrong username or password",
            footer: '<a href="#">Why do I have this issue?</a>'
          });
          this.router.navigate(['/login']);
        }
        this.router.navigate(['']);
        throw err;
      })
    );
  }


  Headers():HttpHeaders{
  

    const  token = localStorage.getItem('Token')
    return new HttpHeaders({
      'Authorization' : `bearer ${token}`
    })

    
  }
  
}
