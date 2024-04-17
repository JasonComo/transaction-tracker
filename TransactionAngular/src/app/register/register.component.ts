import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user = {
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  register() {
    if (!this.validateFields()) {

      return;
    }

    this.authenticationService.register(this.user).subscribe({
      next: response => {
        console.log(response);
        this.toastr.success('Registration successful');
        this.router.navigate(['/log-in']);
      },
      error: error => {
        console.error(error);
        this.toastr.error('Registration failed. Please try again.');
      }
    });
  }

  validateFields(): boolean {
    if (this.user.name.length <= 0) {
      this.toastr.error('Name must be provided.');
      return false;
    }

    if (!this.validateEmail(this.user.email)) {
      return false;
    }

    if (!this.validatePassword(this.user.password)) {
      return false;
    }

    if (this.user.password !== this.user.confirmPassword) {
      this.toastr.error('Passwords do not match.');
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

  validatePassword(password: string): boolean {
    const hasDigit = /\d/.test(password);
    const hasLowercase = /[a-z]/.test(password);
    const hasUppercase = /[A-Z]/.test(password);
    const hasNonAlphanumeric = /\W/.test(password);
    const isLengthValid = password.length >= 6;
    const hasUniqueChars = new Set(password).size >= 1;

    if (!hasDigit) {
      this.toastr.error('Password must include at least one digit.');
      return false;
    }
    if (!hasLowercase) {
      this.toastr.error('Password must include at least one lowercase letter.');
      return false;
    }
    if (!hasUppercase) {
      this.toastr.error('Password must include at least one uppercase letter.');
      return false;
    }
    if (!hasNonAlphanumeric) {
      this.toastr.error('Password must include at least one special character.');
      return false;
    }
    if (!isLengthValid) {
      this.toastr.error('Password must be at least 6 characters long.');
      return false;
    }
    if (!hasUniqueChars) {
      this.toastr.error('Password must contain at least one unique character.');
      return false;
    }

    return true;
  }
}
