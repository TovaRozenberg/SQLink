
export interface UserDto {
  id: number;
  fullName: string;
  email: string;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password?: string;
}

export interface LoginDto {
  email: string;
  password?: string;
}

export interface AuthResponse {
  token: string;
  user: UserDto;
}