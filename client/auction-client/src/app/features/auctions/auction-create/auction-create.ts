import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuctionService } from '../../../core/services/auction.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auction-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './auction-create.html',
  styleUrls: ['./auction-create.scss']
})
export class AuctionCreateComponent {
  auctionForm: FormGroup;
  minDate: string;

  constructor(
    private fb: FormBuilder,
    private auctionService: AuctionService,
    private router: Router
  ) {
    // הגדרת תאריך מינימלי להיום
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];

    this.auctionForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      startingPrice: [1, [Validators.required, Validators.min(1)]],
      endTime: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.auctionForm.valid) {
      this.auctionService.createAuction(this.auctionForm.value).subscribe({
        next: (res) => {
          alert('המכירה נוצרה בהצלחה!');
          this.router.navigate(['/auctions', res.id]); // מעבר ישיר לדף המכירה החדשה
        },
        error: (err) => {
          console.error('Error creating auction', err);
          alert('חלה שגיאה ביצירת המכירה. ודא שאתה מחובר.');
        }
      });
    }
  }
}