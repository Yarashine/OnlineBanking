import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  template: '',
})
export class ConfirmEmailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private http = inject(HttpClient);
  private router = inject(Router);

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (token) {
      this.http
        .post(`http://localhost/notification/api/email/confirm-email?token=${encodeURIComponent(token)}`, {})
        .subscribe({
          next: () => {
            console.log('Email confirmation successful');
            this.router.navigate(['/']);
          },
          error: (err) => {
            console.error('Email confirmation failed:', err);
            this.router.navigate(['/']);
          },
        });
    } else {
      console.error('No token provided');
      this.router.navigate(['/']);
    }
  }
}