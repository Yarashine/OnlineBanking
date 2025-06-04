import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-item-detail',
  standalone: true,
  templateUrl: './item-detail.html',
  styleUrls: ['./item-detail.scss'],
})
export class ItemDetailComponent {
  @Input() item: any;
  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
}
