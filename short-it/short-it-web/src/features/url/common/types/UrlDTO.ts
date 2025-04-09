export interface GenerateUrlRequest {
  longUrl: string;
}

export interface GenerateUrlResponse {
  id: number;
  shortUrl?: string;
  success: boolean;
}
