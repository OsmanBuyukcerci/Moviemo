'use client';

import { useState } from 'react';
import { useParams } from 'next/navigation';
import Link from 'next/link';
import { apiService } from '@/app/services/api';

export default function ChangePasswordPage() {
  const { id } = useParams();
  const usersApiUrl = 'https://localhost:7179/api/users';
  const [oldPassword, setOldPassword] = useState<string>('');
  const [newPassword, setNewPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const [error, setError] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [success, setSuccess] = useState<string>('');

  // Handle password change
  const handleChangePassword = async () => {
    setError('');
    setSuccess('');

    // Validate inputs
    if (!oldPassword || !newPassword || !confirmPassword) {
      setError('Tüm alanlar doldurulmalıdır.');
      return;
    }

    if (newPassword !== confirmPassword) {
      setError('Yeni parola ve parola kontrolü eşleşmiyor.');
      return;
    }

    setIsLoading(true);
    try {
      const response = await fetch(`${usersApiUrl}/${id}/change-password`, {
        method: 'PUT',
        headers: apiService.getHeaders(true),
        body: JSON.stringify({ oldPassword, newPassword }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Parola değiştirme işlemi başarısız oldu.');
      }

      setOldPassword('');
      setNewPassword('');
      setConfirmPassword('');
      setSuccess('Parola başarıyla değiştirildi!');

      apiService.logout();
      window.location.href = '/login';
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 flex items-center justify-center px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        {/* Header */}
        <div className="text-center m-8">
          <h2 className="text-3xl font-extrabold text-white mb-2">
            Parola Değiştir
          </h2>
          <p className="text-gray-300">
            Yeni parolanızı aşağıdan ayarlayın
          </p>
        </div>

        {/* Password Change Form */}
        <div className="bg-white/10 backdrop-blur-lg rounded-2xl p-8 shadow-2xl border border-white/20">
          {error && (
            <div className="bg-red-500/20 border border-red-500/50 text-red-200 px-4 py-3 rounded-lg text-sm mb-6">
              {error}
            </div>
          )}
          {success && (
            <div className="bg-green-500/20 border border-green-500/50 text-green-200 px-4 py-3 rounded-lg text-sm mb-6">
              {success}
            </div>
          )}

          <div className="space-y-6">
            {/* Old Password */}
            <div>
              <label htmlFor="oldPassword" className="block text-sm font-medium text-gray-200 mb-2">
                Eski Parola
              </label>
              <div className="relative flex items-center">
                <input
                  id="oldPassword"
                  type="password"
                  value={oldPassword}
                  onChange={(e) => setOldPassword(e.target.value)}
                  className="block w-full pl-10 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                  placeholder="Eski parolanızı girin"
                />
              </div>
            </div>

            {/* New Password */}
            <div>
              <label htmlFor="newPassword" className="block text-sm font-medium text-gray-200 mb-2">
                Yeni Parola
              </label>
              <div className="relative flex items-center">
                <input
                  id="newPassword"
                  type="password"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  className="block w-full pl-10 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                  placeholder="Yeni parolanızı girin"
                />
              </div>
            </div>

            {/* Confirm Password */}
            <div>
              <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-200 mb-2">
                Parola Kontrol
              </label>
              <div className="relative flex items-center">
                <input
                  id="confirmPassword"
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className="block w-full pl-10 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                  placeholder="Yeni parolanızı tekrar girin"
                />
              </div>
            </div>

            {/* Change Button */}
            <button
              type="button"
              onClick={handleChangePassword}
              className="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200 transform hover:scale-105"
              disabled={isLoading}
            >
              Değiştir
            </button>
          </div>

          {/* Footer */}
          <div className="mt-8 text-center">
            <p className="text-gray-300">
              Geri dön{' '}
              <Link
                href={`/profile/${id}`}
                className="text-purple-400 hover:text-purple-300 font-medium transition-colors"
              >
                Profil
              </Link>
            </p>
          </div>
        </div>

        {/* Additional Info */}
        <div className="m-8 text-center">
          <p className="text-gray-400 text-sm">
            Film severler için tasarlandı ❤️
          </p>
        </div>
      </div>
    </div>
  );
}