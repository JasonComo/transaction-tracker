import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor() { }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();

    if (token) {
      return new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });
    } else {
      return new HttpHeaders();
    }
  }
}
