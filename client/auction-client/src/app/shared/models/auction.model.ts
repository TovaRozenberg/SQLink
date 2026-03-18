
export interface AuctionDto {
  id: number;
  title: string;
  description: string;
  currentPrice: number;
  endTime: Date | string;
  sellerId: number;
}

export interface CreateAuctionDto {
  title: string;
  description: string;
  startingPrice: number;
  endTime: Date | string;
}