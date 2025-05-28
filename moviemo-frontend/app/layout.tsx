import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import './globals.css';
import Navbar from './components/NavBar';
import Footer from './components/Footer';

const inter = Inter({ subsets: ['latin'] });

export const metadata: Metadata = {
  title: 'Moviemo - Film İnceleme Platformu',
  description: 'Filmler hakkında incelemeler yapın ve keşfedin.',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="tr">
      <body className={inter.className}>
        <Navbar />
        <main className="min-h-screen bg-gray-100">{children}</main>
        <Footer />
      </body>
    </html>
  );
}