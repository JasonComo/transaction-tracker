import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent {
  loginData = {
    email: '',
    password: ''
  };

  constructor(
    private authenticationService: AuthenticationService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  login() {
    if (!this.validateLoginFields()) {

      return;
    }

    console.log('Login data:', this.loginData);
    this.authenticationService.login(this.loginData).subscribe({
      next: response => {
        console.log('Login response:', response);
        if (response && response.token) {

          localStorage.setItem('token', response.token);
          this.toastr.success('Login successful');
          this.router.navigate(['/home']);
        } else {

          this.toastr.error('Login failed. Invalid credentials.');

        }
      },
      error: error => {
        console.error('Login error:', error);

        this.toastr.error('Login failed. Please check your credentials.');
      }
    });
  }


  validateLoginFields(): boolean {
    if (!this.validateEmail(this.loginData.email)) {

      return false;
    }

    if (this.loginData.password.length === 0) {
      this.toastr.error('Password field cannot be empty.');
      return false;
    }

    return true;
  }

  validateEmail(email: string): boolean {
    const emailPattern = /^[^\s@]+@[^\s@]+\.(com|net|edu)$/i;
    if (!emailPattern.test(email)) {
      this.toastr.error('Please enter a valid email address.');
      return false;
    }
    return true;
  }
}
