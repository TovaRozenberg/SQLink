import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BidDto, CreateBidDto } from '../../shared/models/bid.model';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  private apiUrl = `${environment.apiUrl}/bids`;

  constructor(private http: HttpClient) { }

  // שליפת כל ההצעות למכירה מסוימת
  getBidsByAuctionId(auctionId: number): Observable<BidDto[]> {
    return this.http.get<BidDto[]>(`${this.apiUrl}/auction/${auctionId}`);
  }

  // הגשת הצעה חדשה (דורש לוגין)
  placeBid(bid: CreateBidDto): Observable<any> {
    return this.http.post(this.apiUrl, bid);
  }
}