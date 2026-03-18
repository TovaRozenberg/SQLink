
export interface BidDto {
  id: number;
  amount: number;
  bidTime: Date | string;
  bidderName: string; 
}

export interface CreateBidDto {
  auctionId: number;
  amount: number;
}