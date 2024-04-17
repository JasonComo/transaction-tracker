import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { env } from 'process';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { FormBuilder, Validators } from '@angular/forms';
import { User } from '../shared/user.model';
import { Router } from '@angular/router';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private url = environment.apiBaseUrl + '/Account';


  constructor(private http: HttpClient, private router: Router, private tokenService: TokenService) { }

  private createHeaders(): HttpHeaders {
    return this.tokenService.createHeaders();
  }

  register(user: any): Observable<any> {
    console.log("Register URL:", `${this.url}/register`);
    return this.http.post(`${this.url}/register`, user)
  }

  login(loginData: any): Observable<any> {
    console.log("Login URL:", `${this.url}/login`);
    return this.http.post<any>(`${this.url}/login`, loginData);
  }

  getUserNameById(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get(`${this.url}/username`, { headers, responseType: 'text' });
  }

  // Destroys token
  signOut(): void {
    localStorage.removeItem('token');

    this.router.navigate(['/home']);
  }



}

