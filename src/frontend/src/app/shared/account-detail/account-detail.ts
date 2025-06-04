import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-account-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-detail.html',
  styleUrls: ['./account-detail.scss']
})
export class AccountDetailComponent {
  @Input() account: any;
  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();

  getObjectKeys(obj: any): string[] {
    return Object.keys(obj);
  }
}