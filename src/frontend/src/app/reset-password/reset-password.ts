import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  template: '',
})
export class ResetPasswordComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private http = inject(HttpClient);
  private router = inject(Router);

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (token) {
      this.http
        .post(`https://localhost/notification/api/email/reset-password?token=${encodeURIComponent(token)}`, {})
        .subscribe({
          next: () => {
            console.log('Password reset confirmation successful');
            this.router.navigate(['/']);
          },
          error: (err) => {
            console.error('Password reset confirmation failed:', err);
            this.router.navigate(['/']);
          },
        });
    } else {
      console.error('No token provided');
      this.router.navigate(['/']);
    }
  }
}