import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-item-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './item-form.html',
  styleUrls: ['./item-form.scss'],
})
export class ItemFormComponent implements OnChanges {
  @Input() item: any | null = null;
  @Output() save = new EventEmitter<any>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      id: '',
      firstName: '',
      lastName: '',
      patronymic: '',
      phoneNumber: '',
      passportIdentifier: '',
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['item'] && this.item) {
      this.form.patchValue(this.item);
    }
  }

  submit() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
    }
  }
}
