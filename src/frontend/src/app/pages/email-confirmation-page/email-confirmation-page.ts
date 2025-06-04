import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-email-confirmation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './email-confirmation-page.html',
  styleUrls: ['./email-confirmation-page.scss'],
})
export class EmailConfirmationComponent {
  private http = inject(HttpClient);
  email: string = localStorage.getItem('email') || 'your email';
  isResending: boolean = false;

  resendEmail(): void {
    if (this.isResending) return;

    this.isResending = true;
    this.http
      .post('https://localhost/notification/api/email-confirmation', { email: this.email })
      .subscribe({
        next: () => {
          this.isResending = false;
          console.log('Confirmation email resent successfully');
        },
        error: (err) => {
          this.isResending = false;
          console.error('Failed to resend confirmation email:', err);
        },
      });
  }
}