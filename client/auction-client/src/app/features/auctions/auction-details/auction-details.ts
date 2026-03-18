import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuctionService } from '../../../core/services/auction.service';
import { BidService } from '../../../core/services/bid.service';
import { AuthService } from '../../../core/services/auth.service';
import { AuctionDto } from '../../../shared/models/auction.model';
import { BidDto } from '../../../shared/models/bid.model';
import { interval, Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SignalrService } from '../../../core/services/signalr.service';
import { Inject } from '@angular/core';

@Component({
  selector: 'app-auction-details',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, SignalrService, Inject],
  templateUrl: './auction-details.html'
})
export class AuctionDetailsComponent implements OnInit, OnDestroy {
  auction?: AuctionDto;
  bids: BidDto[] = [];
  timeLeft: string = '';
  timerSubscription?: Subscription;
  newBidAmount: number = 0;
private readonly route = inject(ActivatedRoute);
  private readonly auctionService = inject(AuctionService);
  private readonly bidService = inject(BidService);
  private readonly signalrService = inject(SignalrService);
  private readonly authService = inject(AuthService);

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadData(id);
    
    this.timerSubscription = interval(1000).subscribe(() => this.updateTimer());
  }

  loadData(id: number) {
    this.auctionService.getAuctionById(id).subscribe(data => {
      this.auction = data;
      this.newBidAmount = data.currentPrice + 1; 
    });
    this.bidService.getBidsByAuctionId(id).subscribe(data => this.bids = data);
  }

  updateTimer() {
    if (!this.auction) return;
    const end = new Date(this.auction.endTime).getTime();
    const now = new Date().getTime();
    const diff = end - now;

    if (diff <= 0) {
      this.timeLeft = 'המכירה הסתיימה';
      this.timerSubscription?.unsubscribe();
    } else {
      const hours = Math.floor(diff / (1000 * 60 * 60));
      const mins = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
      const secs = Math.floor((diff % (1000 * 60)) / 1000);
      this.timeLeft = `${hours}:${mins}:${secs}`;
    }
  }

  placeBid() {
    if (this.auction && this.newBidAmount > this.auction.currentPrice) {
      this.bidService.placeBid({ auctionId: this.auction.id, amount: this.newBidAmount }).subscribe({
        next: () => this.loadData(this.auction!.id), 
        error: (err) => alert(err.error || 'שגיאה בהגשת הצעה')
      });
    }
  }

  ngOnDestroy() {
    this.timerSubscription?.unsubscribe();
  }
}