import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-transfer',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './transfer.html',
  styleUrls: ['./transfer.scss'],
})
export class TransferComponent {
  transferForm: FormGroup;
  isSubmitting = false;

  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private router = inject(Router);

  constructor() {
    this.transferForm = this.fb.group({
      senderAccountId: ['', [Validators.required, Validators.pattern(/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i)]],
      receiverAccountId: ['', [Validators.required, Validators.pattern(/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i)]],
      amount: ['', [Validators.required, Validators.min(0.01)]],
    });
  }

  get senderAccountId() {
    return this.transferForm.get('senderAccountId')!;
  }

  get receiverAccountId() {
    return this.transferForm.get('receiverAccountId')!;
  }

  get amount() {
    return this.transferForm.get('amount')!;
  }

  onSubmit(): void {
    if (this.transferForm.invalid || this.isSubmitting) return;

    this.isSubmitting = true;
    const transferData = this.transferForm.value;

    this.http
      .post('https://localhost/account/api/account', transferData)
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Failed to create transfer:', err);
          this.isSubmitting = false;
        },
      });
  }
}