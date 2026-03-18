import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { AuctionListComponent } from './features/auctions/auction-list/auction-list';
import { AuctionDetailsComponent } from './features/auctions/auction-details/auction-details';
import { AuctionCreateComponent } from './features/auctions/auction-create/auction-create';
import { authGuard } from './core/guards/auth-guard';
import { MyAuctionsComponent } from './features/auctions/my-auctions/my-auctions';

export const routes: Routes = [
  { path: '', component: AuctionListComponent },
  
  // דפי הזדהות
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  
  // מכירות פומביות
  { path: 'auctions', component: AuctionListComponent },
  { path: 'auctions/create', component: AuctionCreateComponent, canActivate: [authGuard] },
  { path: 'auctions/:id', component: AuctionDetailsComponent }, // דף פרטי מכירה (פתוח לצפייה לכולם)
  { path: 'my-auctions', component: MyAuctionsComponent, canActivate: [authGuard] },
  // נתיב ברירת מחדל לכל מקרה של טעות (Wildcard)
  { path: '**', redirectTo: '' }
];