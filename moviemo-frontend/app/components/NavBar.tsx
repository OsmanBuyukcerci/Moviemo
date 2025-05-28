'use client';

import Link from 'next/link';
import { useEffect, useState } from 'react';
import { apiService } from '../services/api';

export default function Navbar() {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [firstLetterOfUsername, setFirstLetterOfUsername] = useState<string>("");

  const handleLogout = () => {
    // Clear tokens and username from localStorage
    apiService.logout();
    setIsDropdownOpen(false);
    setIsLoggedIn(false);
  };

    // Check if user is logged in by verifying token existence and expiration
  useEffect(() => {
    setIsLoggedIn(apiService.isAuthenticated());
    setFirstLetterOfUsername(localStorage.getItem('username')?.charAt(0).toUpperCase()!)
  }, []);

  if (isLoggedIn === null) {
    return null;
  }

  return (
  <nav className="bg-gradient-to-r from-gray-900 via-gray-800 to-gray-900 text-white shadow-2xl backdrop-blur-md border-b border-gray-700/50">
    <div className="container mx-auto px-6 py-4">
      <div className="flex justify-between items-center">
        {/* Logo */}
        <Link href="/" className="group flex items-center space-x-2">
          <div className="w-8 h-8 bg-gradient-to-br from-red-500 to-pink-600 rounded-lg flex items-center justify-center transform group-hover:scale-110 transition-transform duration-200">
            <span className="text-white font-bold text-sm">M</span>
          </div>
          <span className="text-2xl font-bold bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
            Moviemo
          </span>
        </Link>

        {/* Navigation Items */}
        <div className="flex items-center space-x-8">
          <Link 
            href="/about" 
            className="relative px-3 py-2 text-gray-300 hover:text-white transition-colors duration-200 group"
          >
            Hakkımızda
            <span className="absolute bottom-0 left-0 w-0 h-0.5 bg-gradient-to-r from-red-500 to-pink-600 group-hover:w-full transition-all duration-300"></span>
          </Link>
          
          {isLoggedIn ? (
            <div className="relative">
              <button
                onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                className="flex items-center space-x-2 px-4 py-2 bg-gray-700/50 hover:bg-gray-600/50 rounded-lg border border-gray-600/50 hover:border-gray-500/50 transition-all duration-200 backdrop-blur-sm"
              >
                <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-purple-600 rounded-full flex items-center justify-center">
                  <span className="text-white text-sm font-medium">{firstLetterOfUsername}</span>
                </div>
                <span className="text-gray-300">Hesap</span>
                <svg 
                  className={`w-4 h-4 text-gray-400 transition-transform duration-200 ${isDropdownOpen ? 'rotate-180' : ''}`}
                  fill="none" 
                  stroke="currentColor" 
                  viewBox="0 0 24 24"
                >
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                </svg>
              </button>
              
              {isDropdownOpen && (
                <div className="absolute right-0 mt-2 w-56 bg-gray-800/95 backdrop-blur-lg rounded-xl shadow-2xl border border-gray-700/50 z-50 overflow-hidden">
                  <div className="p-2">
                    <Link
                      href="/profile"
                      className="flex items-center space-x-3 px-4 py-3 text-gray-300 hover:text-white hover:bg-gray-700/50 rounded-lg transition-all duration-200"
                      onClick={() => setIsDropdownOpen(false)}
                    >
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                      </svg>
                      <span>Profil</span>
                    </Link>
                    <div className="h-px bg-gray-700/50 my-2"></div>
                    <Link
                      href="/login"
                      className="flex items-center space-x-3 px-4 py-3 text-gray-300 hover:text-red-400 hover:bg-red-500/10 rounded-lg transition-all duration-200"
                      onClick={handleLogout}
                    >
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                      </svg>
                      <span>Çıkış Yap</span>
                    </Link>
                  </div>
                </div>
              )}
            </div>
          ) : (
            <Link 
              href="/login" 
              className="px-6 py-2.5 bg-gradient-to-r from-red-500 to-pink-600 hover:from-red-600 hover:to-pink-700 text-white font-medium rounded-lg shadow-lg shadow-red-500/25 hover:shadow-red-500/40 transition-all duration-200 transform hover:scale-105"
            >
              Giriş Yap
            </Link>
          )}
        </div>
      </div>
    </div>
  </nav>
);
}