import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import * as jose from 'jose';

export interface SignUpRequest {
  email: string;
  password: string;
  deviceId: string;
}

export interface TokensResponse {
  accessToken: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly PUBLIC_KEY = `-----BEGIN PUBLIC KEY----- MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2TQFmRRXXtIYNMNBqk9vaT/I33tsTiXePsXiek/3IfuVS76HruIMC8RCYwdvC06qH3rDWF8w8GZCQjtII5rXBAmxe8QDD3gmnnEjPpubP1GNK2FCSKp1n2p+pxBKvOvwMXXuh5S2JZkV5RbWIga8vKKXGI101vjzN84s14Bp//+3PNy5uuUrqWfma59FnYlNRgIYcNDj0FG/KA88lIXiNfrme0BxdfkQrh1nGKv2VmJBocwKwFKUZLtjRmXwIfiQxTqyHhRGdyvTXT9lDAKg1bLwKeTaIkxIWkAtDTusxfz0AsKTjV+9X4jSEoxA1iaS5nf/dILU8bJqzHV3f35+uQIDAQAB -----END PUBLIC KEY-----`;

  private readonly Sign_Up_URL = 'https://localhost/user/api/auth/signup';
  private readonly Sign_In_URL = 'https://localhost/user/api/auth/signin';
  private readonly Forgot_Password = 'http://localhost/notification/api/email/forgot-password';

  constructor(private http: HttpClient) {}

  signUp(data: SignUpRequest): Observable<TokensResponse> {
    return this.http.post<TokensResponse>(this.Sign_Up_URL, data).pipe(
      tap((res) => this.handleToken(res.accessToken, res.refreshToken))
    );
  }

  signIn(data: { login: string; password: string }): Observable<TokensResponse> {
    return this.http.post<TokensResponse>(this.Sign_In_URL, data).pipe(
      tap((res) => this.handleToken(res.accessToken, res.refreshToken))
    );
  }

  forgotPassword(data: { email: string; newPassword: string; userId: number }): Observable<void> {
    return this.http.post<void>(this.Forgot_Password, data);
  }

  private async handleToken(accessToken: string, refreshToken: string): Promise<void> {
    try {
      const publicKey = await jose.importSPKI(this.PUBLIC_KEY, 'RS256');

      const { payload } = await jose.jwtVerify(accessToken, publicKey, {
        algorithms: ['RS256'],
        issuer: 'UserMicroserviceOnlineBanking',
        audience: 'MicroservicesOnlineBanking',
      });

      const { sub, role } = payload as { sub: string; role: string };

      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('refreshToken', refreshToken);
      localStorage.setItem('userId', sub);
      localStorage.setItem('role', role);
    } catch (error) {
      console.error('Token verification failed:', error);
      throw new Error('Invalid token');
    }
  }
}