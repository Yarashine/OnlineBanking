import { Routes } from '@angular/router';
import { SignupComponent } from './features/auth/signup/signup';
import { LoginComponent } from './features/auth/login/login';
import { NotificationsComponent } from './pages/notifications-page/notifications-page';
import { ForgotPasswordComponent } from './features/auth/forgot-password/forgot-password';
import { EmailConfirmationComponent } from './pages/email-confirmation-page/email-confirmation-page';
import { ConfirmEmailComponent } from './confirm-email/confirm-email';
import { ResetPasswordComponent } from './reset-password/reset-password';
import { PasswordResetConfirmationComponent } from './pages/password-reset-confirmation-page/password-reset-confirmation-page';
import { DashboardComponent } from './dashboard/dashboard';
import { TransactionHistoryComponent } from './transaction-history/transaction-history';
import { TransferComponent } from './transfer/transfer';
import { ProfileComponent } from './profile/profile';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'signup', component: SignupComponent },
  { path: 'login', component: LoginComponent },
  { path: 'notifications', component: NotificationsComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'email-confirmation', component: EmailConfirmationComponent },
  { path: 'email/confirm-email', component: ConfirmEmailComponent },
  { path: 'email/reset-password', component: ResetPasswordComponent },
  { path: 'password-reset-confirmation', component: PasswordResetConfirmationComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'transfer', component: TransferComponent },
  { path: 'transactions/:accountId', component: TransactionHistoryComponent },
  { path: 'profile', component: ProfileComponent },
];