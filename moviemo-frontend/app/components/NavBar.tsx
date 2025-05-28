'use client';

import Link from 'next/link';
import { useEffect, useState, useRef } from 'react';
import { apiService } from '../services/api';

interface SearchResult {
  id: number;
  title: string;
  posterPath: string;
}

export default function Navbar() {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [firstLetterOfUsername, setFirstLetterOfUsername] = useState<string>("");
  const [searchQuery, setSearchQuery] = useState("");
  const [searchResults, setSearchResults] = useState<SearchResult[]>([]);
  const [showSearchResults, setShowSearchResults] = useState(false);
  const [isSearching, setIsSearching] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [showMobileSearch, setShowMobileSearch] = useState(false);
  const searchTimeoutRef = useRef<NodeJS.Timeout | null>(null);
  const searchRef = useRef<HTMLDivElement>(null);

  const searchApiUrl = 'https://localhost:7179/api/movies/search?Query='

  const handleLogout = () => {
    // Clear tokens and username from localStorage
    apiService.logout();
    setIsDropdownOpen(false);
    setIsLoggedIn(false);
    setIsMobileMenuOpen(false);
  };

  // Search function
  const searchMovies = async (query: string) => {
    if (!query.trim()) {
      setSearchResults([]);
      setShowSearchResults(false);
      return;
    }

    setIsSearching(true);
    try {
      const response = await fetch(searchApiUrl + `${encodeURIComponent(query)}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
      });

      if (response.ok) {
        const results = await response.json();
        setSearchResults(results || []);
        setShowSearchResults(true);
      } else {
        setSearchResults([]);
        setShowSearchResults(false);
      }
    } catch (error) {
      console.error('Search error:', error);
      setSearchResults([]);
      setShowSearchResults(false);
    } finally {
      setIsSearching(false);
    }
  };

  // Handle search input change with debounce
  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const query = e.target.value;
    setSearchQuery(query);

    // Clear previous timeout
    if (searchTimeoutRef.current) {
      clearTimeout(searchTimeoutRef.current);
    }

    // Set new timeout for debounced search
    searchTimeoutRef.current = setTimeout(() => {
      searchMovies(query);
    }, 300);
  };

  // Handle search result click
  const handleSearchResultClick = (movieId: number) => {
    setSearchQuery("");
    setShowSearchResults(false);
    setShowMobileSearch(false);
    setIsMobileMenuOpen(false);
    // Navigate to movie detail page
    window.location.href = `/movie/${movieId}`;
  };

  // Handle click outside search to close results
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        setShowSearchResults(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Check if user is logged in by verifying token existence and expiration
  useEffect(() => {
    setIsLoggedIn(apiService.isAuthenticated());
    setFirstLetterOfUsername(localStorage.getItem('username')?.charAt(0).toUpperCase()!)
  }, []);

  // Cleanup timeout on unmount
  useEffect(() => {
    return () => {
      if (searchTimeoutRef.current) {
        clearTimeout(searchTimeoutRef.current);
      }
    };
  }, []);

  // Close mobile menu on resize
  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth >= 768) {
        setIsMobileMenuOpen(false);
        setShowMobileSearch(false);
      }
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  if (isLoggedIn === null) {
    return null;
  }

  return (
    <nav className="bg-gradient-to-r from-gray-900 via-gray-800 to-gray-900 text-white shadow-2xl backdrop-blur-md border-b border-gray-700/50">
      <div className="container mx-auto px-4 sm:px-6 py-4">
        {/* Desktop Navigation */}
        <div className="flex justify-between items-center">
          {/* Logo */}
          <Link href="/" className="group flex items-center space-x-2 flex-shrink-0">
            <div className="w-8 h-8 bg-gradient-to-br from-red-500 to-pink-600 rounded-lg flex items-center justify-center transform group-hover:scale-110 transition-transform duration-200">
              <span className="text-white font-bold text-sm">M</span>
            </div>
            <span className="text-xl sm:text-2xl font-bold bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
              Moviemo
            </span>
          </Link>

          {/* Desktop Search Bar */}
          <div className="hidden md:flex flex-1 max-w-lg mx-8 relative" ref={searchRef}>
            <div className="relative w-full">
              <input
                type="text"
                placeholder="Film ara..."
                value={searchQuery}
                onChange={handleSearchChange}
                className="w-full px-4 py-2.5 pl-11 bg-gray-800/50 border border-gray-600/50 rounded-lg text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500/50 focus:border-red-500/50 transition-all duration-200 backdrop-blur-sm"
                onFocus={() => searchQuery && setShowSearchResults(true)}
              />
              <div className="absolute left-3 top-1/2 transform -translate-y-1/2">
                {isSearching ? (
                  <div className="w-5 h-5 border-2 border-gray-400 border-t-red-500 rounded-full animate-spin"></div>
                ) : (
                  <svg className="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                )}
              </div>
            </div>

            {/* Desktop Search Results */}
            {showSearchResults && (
              <div className="absolute top-full left-0 right-0 mt-2 bg-gray-800/95 backdrop-blur-lg rounded-xl shadow-2xl border border-gray-700/50 z-[9999] max-h-96 overflow-y-auto">
                {searchResults.length > 0 ? (
                  <div className="p-2">
                    {searchResults.map((movie) => (
                      <div
                        key={movie.id}
                        onClick={() => handleSearchResultClick(movie.id)}
                        className="flex items-center space-x-3 p-3 hover:bg-gray-700/50 rounded-lg cursor-pointer transition-all duration-200"
                      >
                        <div className="w-12 h-16 bg-gray-700 rounded-md overflow-hidden flex-shrink-0">
                          {movie.posterPath ? (
                            <img
                              src={movie.posterPath}
                              alt={movie.title}
                              className="w-full h-full object-cover"
                            />
                          ) : (
                            <div className="w-full h-full flex items-center justify-center">
                              <svg className="w-6 h-6 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 4V2a1 1 0 011-1h8a1 1 0 011 1v2h4a1 1 0 011 1v1a1 1 0 01-1 1h-1v12a2 2 0 01-2 2H6a2 2 0 01-2-2V7H3a1 1 0 01-1-1V5a1 1 0 011-1h4z" />
                              </svg>
                            </div>
                          )}
                        </div>
                        <div className="flex-1 min-w-0">
                          <h3 className="text-white font-medium truncate">{movie.title}</h3>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : searchQuery && !isSearching ? (
                  <div className="p-4 text-center text-gray-400">
                    <svg className="w-12 h-12 mx-auto mb-2 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                    </svg>
                    <p>Sonuç bulunamadı</p>
                  </div>
                ) : null}
              </div>
            )}
          </div>

          {/* Desktop Navigation Items */}
          <div className="hidden md:flex items-center space-x-6 lg:space-x-8">
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
                  <span className="text-gray-300 hidden lg:block">Hesap</span>
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
                  <div className="absolute right-0 mt-2 w-56 bg-gray-800/95 backdrop-blur-lg rounded-xl shadow-2xl border border-gray-700/50 z-[9999] overflow-hidden">
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
                className="px-4 lg:px-6 py-2.5 bg-gradient-to-r from-red-500 to-pink-600 hover:from-red-600 hover:to-pink-700 text-white font-medium rounded-lg shadow-lg shadow-red-500/25 hover:shadow-red-500/40 transition-all duration-200 transform hover:scale-105"
              >
                Giriş Yap
              </Link>
            )}
          </div>

          {/* Mobile Menu Buttons */}
          <div className="flex md:hidden items-center space-x-2">
            {/* Mobile Search Button */}
            <button
              onClick={() => setShowMobileSearch(!showMobileSearch)}
              className="p-2 text-gray-300 hover:text-white transition-colors duration-200"
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </button>

            {/* Mobile Hamburger Button */}
            <button
              onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
              className="p-2 text-gray-300 hover:text-white transition-colors duration-200"
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} 
                      d={isMobileMenuOpen ? "M6 18L18 6M6 6l12 12" : "M4 6h16M4 12h16M4 18h16"} />
              </svg>
            </button>
          </div>
        </div>

        {/* Mobile Search Bar */}
        {showMobileSearch && (
          <div className="md:hidden mt-4 relative" ref={searchRef}>
            <div className="relative">
              <input
                type="text"
                placeholder="Film ara..."
                value={searchQuery}
                onChange={handleSearchChange}
                className="w-full px-4 py-2.5 pl-11 bg-gray-800/50 border border-gray-600/50 rounded-lg text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500/50 focus:border-red-500/50 transition-all duration-200 backdrop-blur-sm"
                onFocus={() => searchQuery && setShowSearchResults(true)}
              />
              <div className="absolute left-3 top-1/2 transform -translate-y-1/2">
                {isSearching ? (
                  <div className="w-5 h-5 border-2 border-gray-400 border-t-red-500 rounded-full animate-spin"></div>
                ) : (
                  <svg className="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                )}
              </div>
            </div>

            {/* Mobile Search Results */}
            {showSearchResults && (
              <div className="absolute top-full left-0 right-0 mt-2 bg-gray-800/95 backdrop-blur-lg rounded-xl shadow-2xl border border-gray-700/50 z-[9999] max-h-80 overflow-y-auto">
                {searchResults.length > 0 ? (
                  <div className="p-2">
                    {searchResults.map((movie) => (
                      <div
                        key={movie.id}
                        onClick={() => handleSearchResultClick(movie.id)}
                        className="flex items-center space-x-3 p-3 hover:bg-gray-700/50 rounded-lg cursor-pointer transition-all duration-200"
                      >
                        <div className="w-10 h-14 bg-gray-700 rounded-md overflow-hidden flex-shrink-0">
                          {movie.posterPath ? (
                            <img
                              src={movie.posterPath}
                              alt={movie.title}
                              className="w-full h-full object-cover"
                            />
                          ) : (
                            <div className="w-full h-full flex items-center justify-center">
                              <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 4V2a1 1 0 011-1h8a1 1 0 011 1v2h4a1 1 0 011 1v1a1 1 0 01-1 1h-1v12a2 2 0 01-2 2H6a2 2 0 01-2-2V7H3a1 1 0 01-1-1V5a1 1 0 011-1h4z" />
                              </svg>
                            </div>
                          )}
                        </div>
                        <div className="flex-1 min-w-0">
                          <h3 className="text-white font-medium text-sm truncate">{movie.title}</h3>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : searchQuery && !isSearching ? (
                  <div className="p-4 text-center text-gray-400">
                    <svg className="w-10 h-10 mx-auto mb-2 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                    </svg>
                    <p className="text-sm">Sonuç bulunamadı</p>
                  </div>
                ) : null}
              </div>
            )}
          </div>
        )}

        {/* Mobile Menu */}
        {isMobileMenuOpen && (
          <div className="md:hidden mt-4 pb-4 border-t border-gray-700/50">
            <div className="flex flex-col space-y-4 pt-4">
              <Link 
                href="/about" 
                className="text-gray-300 hover:text-white transition-colors duration-200 px-2 py-2"
                onClick={() => setIsMobileMenuOpen(false)}
              >
                Hakkımızda
              </Link>
              
              {isLoggedIn ? (
                <>
                  <Link
                    href="/profile"
                    className="flex items-center space-x-3 text-gray-300 hover:text-white transition-colors duration-200 px-2 py-2"
                    onClick={() => setIsMobileMenuOpen(false)}
                  >
                    <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-purple-600 rounded-full flex items-center justify-center">
                      <span className="text-white text-sm font-medium">{firstLetterOfUsername}</span>
                    </div>
                    <span>Profil</span>
                  </Link>
                  <button
                    onClick={handleLogout}
                    className="flex items-center space-x-3 text-gray-300 hover:text-red-400 transition-colors duration-200 px-2 py-2 text-left"
                  >
                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                    </svg>
                    <span>Çıkış Yap</span>
                  </button>
                </>
              ) : (
                <Link 
                  href="/login" 
                  className="inline-flex items-center justify-center px-6 py-2.5 bg-gradient-to-r from-red-500 to-pink-600 hover:from-red-600 hover:to-pink-700 text-white font-medium rounded-lg shadow-lg shadow-red-500/25 hover:shadow-red-500/40 transition-all duration-200"
                  onClick={() => setIsMobileMenuOpen(false)}
                >
                  Giriş Yap
                </Link>
              )}
            </div>
          </div>
        )}
      </div>
    </nav>
  );
}