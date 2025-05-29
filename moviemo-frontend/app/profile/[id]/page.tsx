'use client';


import { useState, useEffect } from 'react';
import { useParams } from 'next/navigation';
import Link from 'next/link';
import { apiService } from '@/app/services/api';
// Placeholder icons (replace with your icon library, e.g., react-icons)
const UserIcon = () => <span className="h-5 w-5 text-gray-400">👤</span>;
const EditIcon = () => <span className="h-5 w-5 text-gray-400">✏️</span>;

export interface User {
  id: number;
  name: string;
  surname: string;
  username: string;
  email: string;
  userRole: UserRole;
}

export enum UserRole {
  Basic = 0,
  Admin = 1,
  Manager = 2,
}

// Translation map for field labels and prompts
const fieldTranslations: Record<keyof User, string> = {
  id: 'Kimlik',
  name: 'Ad',
  surname: 'Soyad',
  username: 'Kullanıcı Adı',
  email: 'E-posta',
  userRole: 'Kullanıcı Rolü',
};

// Translation map for userRole values
const roleTranslations: Record<UserRole, string> = {
  [UserRole.Basic]: 'Temel',
  [UserRole.Admin]: 'Yönetici',
  [UserRole.Manager]: 'Müdür',
};

// Reverse mapping for userRole input parsing
const roleInputMap: Record<string, UserRole> = {
  temel: UserRole.Basic,
  yönetici: UserRole.Admin,
  müdür: UserRole.Manager,
};

// Map frontend field names to backend DTO property names
const dtoFieldMap: Record<keyof User, string> = {
  id: 'Id',
  name: 'Name',
  surname: 'Surname',
  username: 'Username',
  email: 'Email',
  userRole: 'UserRole',
};

export default function ProfilePage() {
  const { id } = useParams();
  const usersApiUrl = 'https://localhost:7179/api/users';
  const [userData, setUserData] = useState<User | null>(null);
  const [currentUser, setCurrentUser] = useState<User | null>(null); // Store current user data
  const [error, setError] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);

  // Fetch current user data
  useEffect(() => {
    const fetchCurrentUser = async () => {
      try {
        const response = await fetch(`${usersApiUrl}/${id}`, {
          headers: apiService.getHeaders(true),
        });
        if (!response.ok) throw new Error('Geçerli kullanıcı verileri alınamadı');
        const data: User = await response.json();
        setCurrentUser(data);
      } catch (err) {
        setError('Geçerli kullanıcı verileri alınamadı: ' + (err as Error).message);
      }
    };
    fetchCurrentUser();
  }, []);

  // Fetch user data
  useEffect(() => {
    if (!id) return;
    const fetchUserData = async () => {
      setIsLoading(true);
      try {
        const response = await fetch(`${usersApiUrl}/${id}`, {
          headers: apiService.getHeaders(true),
        });
        if (!response.ok) throw new Error('Kullanıcı verileri alınamadı');
        const data: User = await response.json();
        setUserData(data);
      } catch (err) {
        setError('Kullanıcı verileri alınamadı: ' + (err as Error).message);
      } finally {
        setIsLoading(false);
      }
    };
    fetchUserData();
  }, [id]);

  // Handle field update
  const handleEdit = async (field: keyof User) => {
    if (!userData) return;

    // Restrict userRole editing to Managers
    if (field === 'userRole' && currentUser?.userRole !== UserRole.Manager) {
      setError('Yalnızca Müdür rolüne sahip kullanıcılar Kullanıcı Rolü’nü değiştirebilir.');
      return;
    }

    let newValue: string | number = prompt(
      `Yeni ${fieldTranslations[field]}:`,
      getDisplayValue(field, userData[field])
    ) || '';
    if (newValue.trim() === '') return; // Cancelled or empty

    // Handle userRole specifically
    if (field === 'userRole') {
      const normalizedValue = newValue.toLowerCase();
      if (normalizedValue in roleInputMap) {
        newValue = roleInputMap[normalizedValue];
      } else {
        setError('Geçersiz rol. Temel, Yönetici veya Müdür kullanın.');
        return;
      }
    }

    setIsLoading(true);
    try {
      const response = await fetch(`${usersApiUrl}/${id}`, {
        method: 'PUT',
        headers: apiService.getHeaders(true),
        body: JSON.stringify({ [dtoFieldMap[field]]: newValue }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Kullanıcı verileri güncellenemedi');
      }

      setUserData((prev) => (prev ? { ...prev, [field]: newValue } : prev));
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setIsLoading(false);
    }
  };

  // Define fields to display
  const fields: (keyof User)[] = ['name', 'surname', 'username', 'email', 'userRole'];

  // Convert userRole enum to string for display
  const getDisplayValue = (field: keyof User, value: any): string => {
    if (field === 'userRole') {
      return roleTranslations[value as UserRole] || String(value);
    }
    return String(value || '');
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 flex items-center justify-center px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8 p-8">
        {/* Header */}
        <div className="text-center">
          <h2 className="text-3xl font-extrabold text-white mb-2">
            Kullanıcı Profili
          </h2>
          <p className="text-gray-300">
            Profil bilgilerini görüntüle ve düzenle
          </p>
        </div>

        {/* Profile Form */}
        <div className="bg-white/10 backdrop-blur-lg rounded-2xl p-8 shadow-2xl border border-white/20">
          {error && (
            <div className="bg-red-500/20 border border-red-500/50 text-red-200 px-4 py-3 rounded-lg text-sm mb-6">
              {error}
            </div>
          )}

          {isLoading ? (
            <div className="flex items-center justify-center">
              <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-white"></div>
            </div>
          ) : userData ? (
            <div className="space-y-6">
              {fields.map((field) => (
                <div key={field}>
                  <label
                    htmlFor={field}
                    className="block text-sm font-medium text-gray-200 mb-2"
                  >
                    {fieldTranslations[field]}
                  </label>
                  <div className="relative flex items-center">
                    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                      <UserIcon />
                    </div>
                    <input
                      id={field}
                      name={field}
                      type="text"
                      disabled
                      value={getDisplayValue(field, userData[field])}
                      className="block w-full pl-10 pr-12 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                    />
                    {currentUser?.userRole == UserRole.Manager && <button
                      type="button"
                      onClick={() => handleEdit(field)}
                      className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-white transition-colors"
                      disabled={isLoading || (field === 'userRole' && currentUser?.userRole !== UserRole.Manager)}
                    >
                      <EditIcon />
                    </button>
                    }
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-gray-300 text-center">Kullanıcı verisi bulunamadı</p>
          )}

          {/* Footer */}
          <div className="mt-8 text-center">
            <p className="text-gray-300">
              Geri dön{' '}
              <Link
                href="/"
                className="text-purple-400 hover:text-purple-300 font-medium transition-colors"
              >
                Anasayfa
              </Link>
            </p>
          </div>
        </div>

        {/* Additional Info */}
        <div className="text-center">
          <p className="text-gray-400 text-sm">
            Film severler için tasarlandı ❤️
          </p>
        </div>
      </div>
    </div>
  );
}