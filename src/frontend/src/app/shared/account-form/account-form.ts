import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-account-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './account-form.html',
  styleUrls: ['./account-form.scss']
})
export class AccountFormComponent implements OnChanges {
  @Input() account: any | null = null;
  @Output() save = new EventEmitter<any>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      id: '',
      name: '',
      balance: '',
      currency: ''
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['account'] && this.account) {
      this.form.patchValue(this.account);
    }
  }

  submit() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
    }
  }
}