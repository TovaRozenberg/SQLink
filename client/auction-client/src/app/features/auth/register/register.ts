import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html', // ודאי שזה תואם לשם הקובץ שנוצר לך
  styleUrls: ['./register.scss']
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

onSubmit() {
  if (this.registerForm.valid) {
    const { email, password, fullName } = this.registerForm.value;

    this.authService.register({ email, password, fullName }).pipe(
      // ברגע שההרשמה מצליחה, אנחנו עוברים ישר להתחברות
      switchMap(() => this.authService.login({ email, password }))
    ).subscribe({
      next: (res) => {
        console.log('Registered and Logged in automatically!', res);
        // עכשיו המשתמש כבר מחובר והטוקן שמור (בזכות ה-tap בתוך ה-AuthService)
        this.router.navigate(['/auctions']); // נשלח אותו ישר לרשימת המכירות
      },
      error: (err) => {
        this.errorMessage = 'חלה שגיאה בתהליך הרישום';
        console.error(err);
      }
    });
  }
} 
}