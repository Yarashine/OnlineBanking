import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordResetConfirmationPage } from './password-reset-confirmation-page';

describe('PasswordResetConfirmationPage', () => {
  let component: PasswordResetConfirmationPage;
  let fixture: ComponentFixture<PasswordResetConfirmationPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PasswordResetConfirmationPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordResetConfirmationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
