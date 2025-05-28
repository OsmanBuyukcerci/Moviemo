// types/auth.ts
export interface UserCreateDto {
  name: string;
  surname: string;
  username: string;
  password: string;
  email: string;
  userRole: UserRole;
}

export interface UserLoginDto {
  username: string;
  password: string;
}

export interface TokenResponseDto {
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenRequestDto {
  refreshToken: string;
}

export interface LoginResponseDto {
  username: string;
  isLoggedIn: boolean;
  tokenResponse: TokenResponseDto;
  issue?: LoginIssue;
}

export enum UserRole {
  Basic = 0,
  Admin = 1,
  Manager = 2
}

export enum LoginIssue {
  None = 0,
  NotFound = 1,
  IncorrectPassword = 2
}

export enum CreateIssue {
  None = 0,
  SameContent = 1
}

export interface CreateResponseDto {
  isCreated: boolean;
  issue?: CreateIssue;
}