'use client';

import { useState, useEffect } from 'react';
import MovieCard from './components/MovieCard';
import PageSelector from './components/PageSelector';
import { useSearchParams } from 'next/navigation';

// Film veri tipi (MovieGetDto'ya uygun)
interface Movie {
  id: number;
  title: string;
  overview: string;
  posterPath: string;
}

export default function Home() {
  const searchParams = useSearchParams();
  const pageIndex = searchParams.get('pageIndex') ?? '1';
  const pageSize = searchParams.get('pageSize') ?? '9';

  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalMovieCount, setTotalMovieCount] = useState(0);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const response = await fetch('https://localhost:7179/api/movies?pageIndex=' + pageIndex + '&pageSize=' + pageSize, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          },
        });

        if (!response.ok) {
          throw new Error('Filmler yüklenirken hata oluştu');
        }

        const responseData = await response.json();
        const data: Movie[] = responseData.data;
        setMovies(data);
        setTotalMovieCount(responseData.total);
        setLoading(false);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Bilinmeyen bir hata oluştu');
        setLoading(false);
      }
    };

    fetchMovies();
  }, []);

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12 max-w-5xl">
        <h1 className="text-5xl font-extrabold text-center text-gray-900 mb-8 bg-gradient-to-r from-blue-500 to-purple-600 text-transparent bg-clip-text">
          Moviemo'ya Hoş Geldiniz
        </h1>
        <div className="flex justify-center items-center">
          <div className="animate-spin rounded-full h-12 w-12 border-t-4 border-blue-500"></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-12 max-w-5xl">
        <h1 className="text-5xl font-extrabold text-center text-gray-900 mb-8 bg-gradient-to-r from-blue-500 to-purple-600 text-transparent bg-clip-text">
          Moviemo'ya Hoş Geldiniz
        </h1>
        <div className="bg-red-100 text-red-600 p-6 rounded-xl text-center max-w-lg mx-auto">
          <p>{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-12 max-w-5xl">
      <h1 className="text-5xl font-extrabold text-center text-gray-900 mb-8 bg-gradient-to-r from-blue-500 to-purple-600 text-transparent bg-clip-text">
        Moviemo'ya Hoş Geldiniz
      </h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {movies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </div>
      <div><PageSelector total={totalMovieCount} pageIndex={parseInt(pageIndex)} pageSize={parseInt(pageSize)} /></div>
    </div>
  );
}