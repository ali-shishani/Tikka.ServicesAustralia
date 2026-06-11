
/*
Interface for the Refresh Token (can look different, based on your backend api)
*/
export interface RefreshToken {
  id: number;
  userId: number;
  token: string;
  refreshCount: number;
  expiryDate: Date;
}

/*
Interface for the Login Response (can look different, based on your backend api)
*/
export interface LoginResponse {
  token: string;
  refreshToken: string;
  refreshTokenExpireTime: string;
}

/*
Interface for the Login Request (can look different, based on your backend api)
*/
export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
  stayLoggedIn: boolean;
}

export interface LogoutRequest {
  token: string;
  refreshToken: string;
  refreshTokenExpireTime: string;
}

/*
Interface for the Register Request (can look different, based on your backend api)
*/
export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
  dateOfBirth: Date;
  gender: number;
}

/*
Interface for the Register Response (can look different, based on your backend api)
*/
export interface RegisterResponse {
  success: boolean;
  message: string;
  email: string;
}

  /*
  refresh token
  */
export interface RefreshTokenRequest {
  token: string;
  refreshToken: string;
  stayLoggedIn: boolean;
}

export interface RefreshTokenResponse {
  token: string;
  refreshToken: string;
  refreshTokenExpireTime: string;
}
