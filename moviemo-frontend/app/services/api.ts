import { UserLoginDto, LoginResponseDto, UserCreateDto, CreateResponseDto, TokenResponseDto } from "../types/auth";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7179/api';

class ApiService {
  public getHeaders(includeAuth = false): HeadersInit {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    if (includeAuth) {
      const token = this.getAccessToken();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
    }

    return headers;
  }

  private getAccessToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('accessToken');
    }
    return null;
  }

  private getRefreshToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('refreshToken');
    }
    return null;
  }

  private saveToLocalStorage(accessToken?: string, refreshToken?: string, username?: string): void {
    if (typeof window !== 'undefined') {
      localStorage.setItem('accessToken', accessToken!);
      localStorage.setItem('refreshToken', refreshToken!);
      localStorage.setItem('username', username!);
    }
  }

  private clearLocalStorage(): void {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('username');
    }
  }

  async login(loginData: UserLoginDto): Promise<LoginResponseDto> {
    try {
      const response = await fetch(`${API_BASE_URL}/users/login`, {
        method: 'POST',
        headers: this.getHeaders(),
        body: JSON.stringify(loginData),
      });

      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(errorData || 'Giriş başarısız');
      }

      const data: LoginResponseDto = await response.json();
      
      if (data.isLoggedIn && data.tokenResponse) {
        this.saveToLocalStorage(data.tokenResponse.accessToken, data.tokenResponse.refreshToken, data.username);
      }

      return data;
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  }

  async register(registerData: UserCreateDto): Promise<CreateResponseDto> {
    try {
      const response = await fetch(`${API_BASE_URL}/users`, {
        method: 'POST',
        headers: this.getHeaders(),
        body: JSON.stringify(registerData),
      });

      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(errorData || 'Kayıt başarısız');
      }

      const data: CreateResponseDto = await response.json();
      return data;
    } catch (error) {
      console.error('Register error:', error);
      throw error;
    }
  }

  async refreshToken(): Promise<TokenResponseDto | null> {
    try {
      const refreshToken = this.getRefreshToken();
      if (!refreshToken) {
        throw new Error('Refresh token bulunamadı');
      }

      const response = await fetch(`${API_BASE_URL}/users/refresh-token`, {
        method: 'POST',
        headers: this.getHeaders(true),
        body: JSON.stringify({ refreshToken }),
      });

      if (!response.ok) {
        this.clearLocalStorage();
        throw new Error('Token yenileme başarısız');
      }

      const data: TokenResponseDto = await response.json();
      this.saveToLocalStorage(data.accessToken, data.refreshToken);
      return data;
    } catch (error) {
      console.error('Token refresh error:', error);
      this.clearLocalStorage();
      return null;
    }
  }

  logout(): void {
    this.clearLocalStorage();
  }

  isAuthenticated(): boolean {
    return !!this.getAccessToken();
  }
}

export const apiService = new ApiService();