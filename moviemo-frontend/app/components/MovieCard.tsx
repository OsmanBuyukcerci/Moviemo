import Link from 'next/link';

// Define the Movie interface
interface Movie {
  id: number;
  title: string;
  overview: string;
  posterPath: string;
}

// Define the props interface for MovieCard
interface MovieCardProps {
  movie: Movie;
}

export default function MovieCard({ movie }: MovieCardProps) {
  return (
    <div className="bg-white rounded-xl shadow-lg p-6 flex flex-col transform hover:scale-105 transition-transform duration-300">
      <img
        src={movie.posterPath}
        alt={`${movie.title} poster`}
        className="w-full h-64 object-cover rounded-md mb-4"
        onError={(e) => {
          e.currentTarget.src = 'https://via.placeholder.com/500x750?text=Poster+Yok';
        }}
      />
      <h2 className="text-xl font-semibold text-gray-800 mb-2">{movie.title}</h2>
      <p className="text-gray-600 line-clamp-3 mb-4">{movie.overview}</p>
      <Link
        href={`/movie/${movie.id}`}
        className="mt-auto bg-gradient-to-r from-blue-500 to-purple-600 text-white font-semibold px-4 py-2 rounded-lg hover:bg-gradient-to-l transition-colors"
      >
        İncele
      </Link>
    </div>
  );
}