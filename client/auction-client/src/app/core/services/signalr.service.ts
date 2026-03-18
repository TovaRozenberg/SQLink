import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;
  
  // נשתמש ב-Subject כדי להעביר את הנתונים לקומפוננטות בצורה נוחה
  public newBidReceived$ = new Subject<any>();
  public auctionExpired$ = new Subject<number>();

  constructor() {
    // 1. בניית החיבור
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl.replace('/api', '')}/auctionhub`) // הנתיב שהגדרנו ב-MapHub
      .withAutomaticReconnect() // ניסיון התחברות אוטומטי במקרה של ניתוק
      .build();

    // 2. התחלת החיבור
    this.startConnection();

    // 3. הגדרת המאזינים (Listeners)
    this.registerListeners();
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected!'))
      .catch(err => console.error('Error while starting SignalR connection: ' + err));
  }

  private registerListeners() {
    // האזנה להצעת מחיר חדשה
    this.hubConnection.on('ReceiveNewBid', (data) => {
      this.newBidReceived$.next(data);
    });

    // האזנה לסגירת מכירה
    this.hubConnection.on('AuctionExpired', (auctionId: number) => {
      this.auctionExpired$.next(auctionId);
    });
  }

  // הצטרפות לחדר של מכירה ספציפית
  joinAuction(auctionId: number) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('JoinAuctionGroup', auctionId);
    } else {
      // אם עדיין לא מחובר, נחכה שנייה וננסה שוב
      setTimeout(() => this.joinAuction(auctionId), 1000);
    }
  }

  // עזיבת חדר (חשוב לביצועים כשסוגרים את הדף)
  leaveAuction(auctionId: number) {
    this.hubConnection.invoke('LeaveAuctionGroup', auctionId);
  }
}