import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';

interface Account {
  id: string;
  userId: number;
  balance: number;
  createdAt: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss'],
})
export class DashboardComponent implements OnInit {
  private http = inject(HttpClient);
  isAuthenticated = !!localStorage.getItem('accessToken');
  userId = parseInt(localStorage.getItem('userId') || '1', 10);
  accounts: Account[] = [];
  isCreating = false;

  ngOnInit(): void {
    if (this.isAuthenticated) {
      this.loadAccounts();
    }
  }

  loadAccounts(): void {
    this.http
      .get<Account[]>(`https://localhost/account/api/account/all-${this.userId}`)
      .subscribe({
        next: (accounts) => {
          this.accounts = accounts;
        },
        error: (err) => {
          console.error('Failed to load accounts:', err);
        },
      });
  }

  createAccount(): void {
    if (this.isCreating) return;
    this.isCreating = true;
    this.http
      .post(`https://localhost/account/api/account/`, { userId: this.userId })
      .subscribe({
        next: () => {
          this.isCreating = false;
          this.loadAccounts();
        },
        error: (err) => {
          console.error('Failed to create account:', err);
          this.isCreating = false;
        },
      });
  }
}