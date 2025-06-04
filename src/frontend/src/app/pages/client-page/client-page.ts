import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ItemListComponent } from '../../shared/item-list/item-list';
import { ItemDetailComponent } from '../../shared/item-detail/item-detail';
import { ItemFormComponent } from '../../shared/item-form/item-form';

@Component({
  selector: 'app-client-page',
  standalone: true,
  imports: [ItemListComponent, ItemDetailComponent, ItemFormComponent],
  templateUrl: './client-page.html',
})
export class ClientPageComponent {
  clients: any[] = [];
  selectedClient: any = null;
  isEditing = false;

  constructor(private http: HttpClient) {
    this.getAllClients();
  }

  getAllClients() {
    this.http.get<any[]>('/api/client/all').subscribe((data) => (this.clients = data));
  }

  selectClient(client: any) {
    this.selectedClient = client;
    this.isEditing = false;
  }

  startEdit() {
    this.isEditing = true;
  }

  updateClient(updated: any) {
    const method = updated.id ? this.http.put : this.http.post;
    const url = updated.id ? '/api/client' : '/api/client';
    method.call(this.http, url, updated).subscribe(() => {
      this.getAllClients();
      this.isEditing = false;
    });
  }

  deleteClient(id: string) {
    this.http.delete(`/api/client/${id}`).subscribe(() => this.getAllClients());
  }
}
