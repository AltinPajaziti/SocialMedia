import { Injectable } from '@angular/core';
import { Envirements } from '../Envirements/Envirements';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class FrendsRequestsService {

   public api = Envirements.api_Url + 'FollowRequests/'
  
    constructor(private http : HttpClient , private router : Router , private auth : AuthenticationService) { }


    getAllFriendRequests(): Observable<any> {
      return this.http.get<any>(`${this.api}GetAllFollowRequests` , {headers : this.auth.Headers()});
    }
  

    AcceptFollow(followId: number) {
      return this.http.get<any>(this.api + 'AcceptFollow?Followid=' + followId ,  {headers : this.auth.Headers()});
    }

    DeclineFollow(followId: number) {
      return this.http.get<any>(this.api + 'DeclineFollow?Followid=' + followId);
    }


    RequestForFollow(followId: number) {
      return this.http.post<any>(this.api + 'RequestForFollow', { Followid: followId });
    }
    
    
    
}
