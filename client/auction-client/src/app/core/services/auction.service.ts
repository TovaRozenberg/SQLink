import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuctionDto, CreateAuctionDto } from '../../shared/models/auction.model';

@Injectable({
  providedIn: 'root'
})
export class AuctionService {
  private apiUrl = `${environment.apiUrl}/auctions`;

  constructor(private http: HttpClient) { }

  // שליפת כל המכירות הפעילות
  getActiveAuctions(): Observable<AuctionDto[]> {
    return this.http.get<AuctionDto[]>(this.apiUrl);
  }

  // שליפת מכירה ספציפית לפי ID
  getAuctionById(id: number): Observable<AuctionDto> {
    return this.http.get<AuctionDto>(`${this.apiUrl}/${id}`);
  }

  // יצירת מכירה חדשה (דורש לוגין)
  createAuction(auction: CreateAuctionDto): Observable<AuctionDto> {
    return this.http.post<AuctionDto>(this.apiUrl, auction);
  }
  getMyAuctions(): Observable<AuctionDto[]> {
  return this.http.get<AuctionDto[]>(`${this.apiUrl}/my-auctions`);
}
}