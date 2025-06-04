import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-password-reset-confirmation',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './password-reset-confirmation-page.html',
  styleUrls: ['./password-reset-confirmation-page.scss'],
})
export class PasswordResetConfirmationComponent {
  email: string = localStorage.getItem('email') || 'your email';
}