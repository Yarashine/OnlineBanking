import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface Notification {
  id: number;
  title: string;
  message: string;
  isRead: boolean;
}

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications-page.html',
  styleUrls: ['./notifications-page.scss'],
})
export class NotificationsComponent implements OnInit {
  private http = inject(HttpClient);
  userId = localStorage.getItem('userId') || '1';
  pageNumber = 1;
  pageSize = 5;

  notifications: Notification[] = [];
  totalPages = 1;
  showAll = false;

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications(): void {
    const url = this.showAll
      ? `https://localhost/notification/api/notification/all/${this.userId}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`
      : `https://localhost/notification/api/notification/unread/${this.userId}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`;

    const countUrl = this.showAll
      ? `https://localhost/notification/api/notification/all/count/${this.userId}`
      : `https://localhost/notification/api/notification/unread/count/${this.userId}`;

    this.http.get<Notification[]>(url).subscribe({
      next: (data) => {
        this.notifications = data;
      },
      error: (err) => {
        console.error('Failed to load notifications:', err);
      }
    });

    this.http.get<number>(countUrl).subscribe({
      next: (count) => {
        this.totalPages = Math.ceil(count / this.pageSize);
      },
      error: (err) => {
        console.error('Failed to load notification count:', err);
      }
    });
  }

  toggleShowAll(): void {
    this.showAll = !this.showAll;
    this.pageNumber = 1;
    this.loadNotifications();
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadNotifications();
    }
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadNotifications();
    }
  }
}