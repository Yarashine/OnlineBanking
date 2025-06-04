import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';



@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.html',
  styleUrls: ['./header.scss']
})
export class HeaderComponent implements OnInit {
  private router = inject(Router);
  private http = inject(HttpClient);  
  private hubConnection!: signalR.HubConnection;

  isLoggedIn = false;
  unreadCount: number = 0;


  ngOnInit(): void {
    this.checkAuthStatus();

    if (this.isLoggedIn) {
      const userId = localStorage.getItem('userId') || '1';

      this.http.get<number>(`https://localhost/notification/api/notification/unread/count/${userId}`)
        .subscribe({
          next: (count) => this.unreadCount = count,
          error: (err) => console.error('Failed to fetch unread count:', err)
        });

      this.startSignalRConnection(userId);
    }
  }

  startSignalRConnection(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost/notificationHub`, {
        accessTokenFactory: () => localStorage.getItem('accessToken') || ''
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected.');

      this.hubConnection.on('ReceiveNotification', (message: string) => {
        console.log('Notification received:', message);
        this.unreadCount++;
      });
    }).catch(err => console.error('SignalR connection error:', err));
  }


  checkAuthStatus(): void {
    this.isLoggedIn = !!localStorage.getItem('accessToken');
  }

  logout(): void {
    const refresh = localStorage.getItem('refreshToken');
    this.http.delete(`https://localhost/user/api/auth/logout?refresh=${refresh}`).subscribe({
      next: () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userId');
        localStorage.removeItem('role');
        this.isLoggedIn = false;
        this.unreadCount = 0;
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Logout failed:', err);
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userId');
        localStorage.removeItem('role');
        this.isLoggedIn = false;
        this.unreadCount = 0;
        this.router.navigate(['/login']);
      }
    });
  }

  goToNotifications(): void {
    this.router.navigate(['/notifications']);
  }
}