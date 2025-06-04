import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';

interface Client {
  id: string;
  firstName: string;
  lastName: string;
  patronymic: string;
  phoneNumber: string;
  passportIdentifier: string;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.scss'],
})
export class ProfileComponent implements OnInit {
  private http = inject(HttpClient);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  isAuthenticated = !!localStorage.getItem('accessToken');
  userId = parseInt(localStorage.getItem('userId') || '1', 10);
  profileForm: FormGroup;
  isSubmitting = false;

  constructor() {
    this.profileForm = this.fb.group({
      id: [{ value: '', disabled: true }, [Validators.required]],
      firstName: ['', [Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z\s-]*$/)]],
      lastName: ['', [Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z\s-]*$/)]],
      patronymic: ['', [Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z\s-]*$/)]],
      phoneNumber: ['', [Validators.pattern(/^\+\d{10,15}$/)]],
      passportIdentifier: ['', [Validators.minLength(10), Validators.maxLength(10), Validators.pattern(/^[a-zA-Z0-9]*$/)]],
    });
  }

  get firstName() {
    return this.profileForm.get('firstName')!;
  }

  get lastName() {
    return this.profileForm.get('lastName')!;
  }

  get patronymic() {
    return this.profileForm.get('patronymic')!;
  }

  get phoneNumber() {
    return this.profileForm.get('phoneNumber')!;
  }

  get passportIdentifier() {
    return this.profileForm.get('passportIdentifier')!;
  }

  ngOnInit(): void {
    if (!this.isAuthenticated) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadProfile();
  }

  loadProfile(): void {
    console.log(this.userId)
    this.http
      .get<Client>(`https://localhost/user/api/client/user-id?id=${this.userId}`)
      .subscribe({
        next: (client) => {
          console.log(client)
          this.profileForm.patchValue(client);
        },
        error: (err) => {
          console.error('Failed to load profile:', err);
        },
      });
  }

  onSubmit(): void {
    if (this.profileForm.invalid || this.isSubmitting) return;

    this.isSubmitting = true;
    const updateData = {
      ...this.profileForm.getRawValue(),
      id: this.profileForm.get('id')!.value,
    };
    console.log(this.userId)
    this.http
      .put('https://localhost/user/api/client', updateData)
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          console.log('Profile updated successfully');
        },
        error: (err) => {
          console.error('Failed to update profile:', err);
          this.isSubmitting = false;
        },
      });
  }
}