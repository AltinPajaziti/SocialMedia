import  { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import  { Router } from '@angular/router';
import  { Observable } from 'rxjs';
import { Envirements } from '../Envirements/Envirements';
import  { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class FrendSuggestionsService {


   public api = Envirements.api_Url + 'Users/'
  
    constructor(private http : HttpClient , private router : Router , private auth : AuthenticationService) { }


    GetAllFrendSuggestions(): Observable<any> {
      return this.http.get<any>(`${this.api}GetSugestionFrends` , {headers : this.auth.Headers()});
    }
  

}
