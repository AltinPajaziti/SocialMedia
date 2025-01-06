import { Injectable } from '@angular/core';
import { Envirements } from '../Envirements/Envirements';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class FrendsRequestsService {

   public api = Envirements.api_Url + 'Authentication/'
  
    constructor(private http : HttpClient , private router : Router) { }


    GetAllFrendRequests(){
      return this.http.get<any>(this.api+ 'GetAllFollowRequests' )
    }

    AcceptFollow(followId: number) {
      return this.http.get<any>(this.api + 'AcceptFollow?Followid=' + followId);
    }

    DeclineFollow(followId: number) {
      return this.http.get<any>(this.api + 'DeclineFollow?Followid=' + followId);
    }


    RequestForFollow(followId: number) {
      return this.http.post<any>(this.api + 'RequestForFollow', { Followid: followId });
    }
    
    
    
}
