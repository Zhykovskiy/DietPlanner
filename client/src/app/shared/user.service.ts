import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly baseUrl = "https://localhost:7212/api";

  constructor(private _http:HttpClient) { }

  register(formData: any) {    
    return this._http.post(this.baseUrl + "/AppUser/Register", formData);
  }

  login(formData: any) {
    return this._http.post(this.baseUrl + '/AppUser/Login', formData);
  }
}
