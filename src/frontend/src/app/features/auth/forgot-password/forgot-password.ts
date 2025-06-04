import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './forgot-password.html',
  styleUrls: ['./forgot-password.scss'],
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  loading = false;

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  constructor() {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  get email() {
    return this.forgotPasswordForm.get('email')!;
  }

  get newPassword() {
    return this.forgotPasswordForm.get('newPassword')!;
  }

  onSubmit() {
    console.log(this.forgotPasswordForm.value)
    if (this.forgotPasswordForm.invalid) return;
    
    console.log(this.forgotPasswordForm.value)
    this.loading = true;
    const userId = localStorage.getItem('userId') || '1';
    const formValue = { ...this.forgotPasswordForm.value, userId: parseInt(userId, 10) };

    this.authService.forgotPassword(formValue).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/password-reset-confirmation']);
      },
      error: (err) => {
        console.error('Error:', err);
        this.loading = false;
      },
    });
  }
}