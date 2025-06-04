import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { routes } from './app.routes';
import { SignupComponent } from './features/auth/signup/signup';
import { LoginComponent } from './features/auth/login/login';
import { HeaderComponent } from './shared/header/header';
import { NotificationsComponent } from './pages/notifications-page/notifications-page';
import { ForgotPasswordComponent } from './features/auth/forgot-password/forgot-password';
import { AppComponent } from './app.component/app.component';

@NgModule({
  imports: [
    HttpClientModule,
    ReactiveFormsModule,
    AppComponent,
    SignupComponent,
    HeaderComponent,
    NotificationsComponent,
    LoginComponent,
    ForgotPasswordComponent,
    RouterModule.forRoot(routes)
  ],
})
export class AppModule {}