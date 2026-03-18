import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // לטובת ה-Search input
import { AuctionService } from '../../../core/services/auction.service';
import { AuctionDto } from '../../../shared/models/auction.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-auction-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './auction-list.html',
  styleUrls: ['./auction-list.scss']
})
export class AuctionListComponent implements OnInit {
  allAuctions: AuctionDto[] = [];      // הרשימה המלאה מהשרת
  filteredAuctions: AuctionDto[] = []; // הרשימה שמוצגת לאחר סינון

  searchTerm: string = '';
  sortBy: string = 'newest'; // ברירת מחדל למיון

  constructor(private auctionService: AuctionService) {}

  ngOnInit(): void {
    this.auctionService.getActiveAuctions().subscribe({
      next: (data) => {
        this.allAuctions = data;
        this.applyFilters(); // ראשוני
      },
      error: (err) => console.error('Error fetching auctions', err)
    });
  }

  // הפונקציה המרכזית שמבצעת חיפוש, סינון ומיון
  applyFilters() {
    let result = [...this.allAuctions];

    // 1. חיפוש לפי טקסט
    if (this.searchTerm) {
      result = result.filter(a => 
        a.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        a.description.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    // 2. מיון
    if (this.sortBy === 'priceLow') {
      result.sort((a, b) => a.currentPrice - b.currentPrice);
    } else if (this.sortBy === 'priceHigh') {
      result.sort((a, b) => b.currentPrice - a.currentPrice);
    } else if (this.sortBy === 'newest') {
      result.sort((a, b) => b.id - a.id); // מניחים ש-ID גבוה יותר הוא חדש יותר
    }

    this.filteredAuctions = result;
  }
}