import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-item-list',
  standalone: true,
  templateUrl: './item-list.html',
  styleUrls: ['./item-list.scss'],
})
export class ItemListComponent {
  @Input() items: any[] = [];
  @Input() displayField: string = 'firstName';
  @Output() viewDetails = new EventEmitter<any>();
  @Output() deleteItem = new EventEmitter<string>();
}
