import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuctionService } from '../../../core/services/auction.service';
import { AuctionDto } from '../../../shared/models/auction.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-auctions',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './my-auctions.html'
})
export class MyAuctionsComponent implements OnInit {
  myAuctions: AuctionDto[] = [];

  constructor(private auctionService: AuctionService) {}

  ngOnInit() {
    this.auctionService.getMyAuctions().subscribe(data => {
      this.myAuctions = data;
    });
  }
}