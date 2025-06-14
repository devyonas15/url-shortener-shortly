export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  loginId: string;
  email: string;
  accessToken: string;
}
