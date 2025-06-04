import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-account-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-list.html',
  styleUrls: ['./account-list.scss']
})
export class AccountListComponent implements OnChanges {
  @Input() accounts: any[] = [];
  @Input() pageSize = 5;
  @Output() view = new EventEmitter<any>();
  @Output() delete = new EventEmitter<string>();

  pagedAccounts: any[] = [];
  currentPage = 1;
  totalPages = 1;

  ngOnChanges() {
    this.totalPages = Math.ceil(this.accounts.length / this.pageSize);
    this.changePage(1);
  }

  changePage(page: number) {
    this.currentPage = page;
    const start = (page - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedAccounts = this.accounts.slice(start, end);
  }
}
