import React from 'react';
import { ChevronLeft, ChevronRight, MoreHorizontal } from 'lucide-react';
import { Movie } from '../types/movie';

interface PaginationProps {
  total: number;
  pageIndex: number;
  pageSize: number;
  className?: string;
}

interface ApiResponse {
  data: Movie[];
  total: number;
  pageIndex: number;
  pageSize: number;
}

interface PaginationProps {
    total: number;
    pageIndex: number;
    pageSize: number;
}

export default function PageSelector (props: PaginationProps) {
const { total, pageIndex, pageSize } = props;

  const totalPages = Math.ceil(total / pageSize);
  
  const handlePageClick = (page: number): void => {
    if (page !== pageIndex && page >= 1 && page <= totalPages) {
      const queryString = `?pageIndex=${page}&pageSize=${pageSize}`;
      
      window.location.href = queryString;
    }
  };

  const getVisiblePages = (): (number | string)[] => {
    const delta = 2;
    const range: number[] = [];
    const rangeWithDots: (number | string)[] = [];

    for (let i = Math.max(2, pageIndex - delta); i <= Math.min(totalPages - 1, pageIndex + delta); i++) {
      range.push(i);
    }

    if (pageIndex - delta > 2) {
      rangeWithDots.push(1, '...');
    } else {
      rangeWithDots.push(1);
    }

    rangeWithDots.push(...range);

    if (pageIndex + delta < totalPages - 1) {
      rangeWithDots.push('...', totalPages);
    } else {
      if (totalPages > 1) {
        rangeWithDots.push(totalPages);
      }
    }

    return rangeWithDots.filter((page, index, array) => 
      array.indexOf(page) === index
    );
  };

  if (totalPages <= 1) return null;

  const visiblePages = getVisiblePages();

  return (
    <nav 
      className={`flex items-center justify-center space-x-1 mt-8`} 
      aria-label="Sayfa navigasyonu"
    >
      {/* Önceki Butonu */}
      <button
        onClick={() => handlePageClick(pageIndex - 1)}
        disabled={pageIndex === 1}
        className={`
          flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-all duration-200
          ${pageIndex === 1 
            ? 'text-gray-400 cursor-not-allowed bg-gray-100 dark:bg-gray-800 dark:text-gray-600' 
            : 'text-gray-700 hover:bg-gray-100 hover:text-gray-900 dark:text-gray-300 dark:hover:bg-gray-700 dark:hover:text-white hover:scale-105'
          }
        `}
        aria-label="Önceki sayfa"
      >
        <ChevronLeft className="w-4 h-4 mr-1" />
        Önceki
      </button>

      {/* Sayfa Numaraları */}
      <div className="flex items-center space-x-1">
        {visiblePages.map((page, index) => {
          if (page === '...') {
            return (
              <span 
                key={`dots-${index}`} 
                className="px-3 py-2 text-gray-500 dark:text-gray-400"
                aria-hidden="true"
              >
                <MoreHorizontal className="w-4 h-4" />
              </span>
            );
          }

          const pageNumber = page as number;
          return (
            <button
              key={pageNumber}
              onClick={() => handlePageClick(pageNumber)}
              className={`
                px-4 py-2 text-sm font-medium rounded-lg transition-all duration-200 min-w-[40px] hover:scale-105
                ${pageNumber === pageIndex
                  ? 'bg-gradient-to-r from-blue-500 to-purple-600 text-white shadow-lg hover:from-blue-600 hover:to-purple-700'
                  : 'text-gray-700 hover:bg-gray-100 hover:text-gray-900 dark:text-gray-300 dark:hover:bg-gray-700 dark:hover:text-white'
                }
              `}
              aria-label={`Sayfa ${pageNumber}`}
              aria-current={pageNumber === pageIndex ? 'page' : undefined}
            >
              {pageNumber}
            </button>
          );
        })}
      </div>

      {/* Sonraki Butonu */}
      <button
        onClick={() => handlePageClick(pageIndex + 1)}
        disabled={pageIndex === totalPages}
        className={`
          flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-all duration-200
          ${pageIndex === totalPages 
            ? 'text-gray-400 cursor-not-allowed bg-gray-100 dark:bg-gray-800 dark:text-gray-600' 
            : 'text-gray-700 hover:bg-gray-100 hover:text-gray-900 dark:text-gray-300 dark:hover:bg-gray-700 dark:hover:text-white hover:scale-105'
          }
        `}
        aria-label="Sonraki sayfa"
      >
        Sonraki
        <ChevronRight className="w-4 h-4 ml-1" />
      </button>
    </nav>
  );
};