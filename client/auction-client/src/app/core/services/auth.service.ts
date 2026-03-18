import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode'; // חובה להתקין: npm install jwt-decode
import { environment } from '../../../environments/environment';
import { RegisterDto, LoginDto, AuthResponse } from '../../shared/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/users`;
  private tokenKey = 'auction_jwt_token';

  constructor(private http: HttpClient) {}

  // --- פעולות מול השרת ---

  register(user: RegisterDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  login(credentials: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (response && response.token) {
          this.saveToken(response.token);
        }
      })
    );
  }

  // --- ניהול הטוקן ב-Local Storage ---

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    // אופציונלי: אפשר לבדוק כאן גם אם הטוקן פג תוקף בעזרת jwt-decode
    return !!token;
  }

  // --- שליפת מידע מתוך הטוקן ---

  getCurrentUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      
      /* שימי לב: ב-.NET ה-Claim של ה-ID נקרא בדרך כלל 'nameid' 
         או 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      */
      const userId = decoded.nameid || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      
      return userId ? parseInt(userId) : null;
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  // פונקציה בונוס: שליפת שם המשתמש מהטוקן
  getCurrentUserName(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      return decoded.unique_name || decoded.name || null;
    } catch {
      return null;
    }
  }
}