import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface Transfer {
  id: string;
  senderAccountId: string;
  receiverAccountId: string;
  amount: number;
  createdAt: string;
}

@Component({
  selector: 'app-transaction-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './transaction-history.html',
  styleUrls: ['./transaction-history.scss'],
})
export class TransactionHistoryComponent implements OnInit {
  private http = inject(HttpClient);
  private route = inject(ActivatedRoute);
  accountId: string | null = null;
  transactions: Transfer[] = [];

  ngOnInit(): void {
    this.accountId = this.route.snapshot.paramMap.get('accountId');
    if (this.accountId) {
      this.loadTransactions();
    }
  }

  loadTransactions(): void {
    this.http
      .get<Transfer[]>(`https://localhost/account/api/account/all-transfers-${this.accountId}`)
      .subscribe({
        next: (transactions) => {
          this.transactions = transactions;
        },
        error: (err) => {
          console.error('Failed to load transactions:', err);
        },
      });
  }
}