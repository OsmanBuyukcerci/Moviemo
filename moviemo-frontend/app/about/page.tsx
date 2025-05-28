'use client';

export default function AboutPage() {
  return (
    <div className="container mx-auto px-4 py-12 max-w-5xl">
      {/* Başlık Bölümü */}
      <header className="text-center mb-12">
        <h1 className="text-5xl font-extrabold text-gray-900 mb-4 bg-gradient-to-r from-blue-500 to-purple-600 text-transparent bg-clip-text">
          Moviemo Hakkında
        </h1>
        <p className="text-lg text-gray-600 max-w-2xl mx-auto">
          Sinema tutkunlarının buluşma noktası. Filmleri keşfedin, incelemeler yazın ve topluluğumuzun bir parçası olun!
        </p>
      </header>

      {/* Ana İçerik */}
      <section className="grid gap-8 md:grid-cols-2">
        {/* Biz Kimiz Kartı */}
        <div className="bg-white rounded-xl shadow-lg p-6 transform hover:scale-105 transition-transform duration-300">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">Biz Kimiz?</h2>
          <p className="text-gray-600 leading-relaxed">
            Moviemo, sinema severlerin bir araya geldiği bir platformdur. Amacımız, film tutkunlarının favori filmlerini keşfetmelerine, incelemeler yazmalarına ve diğer sinemaseverlerle fikir alışverişinde bulunmalarına olanak sağlamak. Her film, bir hikaye anlatır; biz de sizin hikayenizi paylaşmanız için buradayız.
          </p>
        </div>

        {/* Vizyonumuz Kartı */}
        <div className="bg-white rounded-xl shadow-lg p-6 transform hover:scale-105 transition-transform duration-300">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">Vizyonumuz</h2>
          <p className="text-gray-600 leading-relaxed">
            Sinema dünyasını daha erişilebilir ve etkileşimli hale getirmek istiyoruz. Moviemo, her türden filmsevere hitap eden, kullanıcı odaklı bir platform olarak tasarlandı. Teknoloji ve sanatı birleştirerek, sinema deneyiminizi unutulmaz kılmayı hedefliyoruz.
          </p>
        </div>

        {/* Değerlerimiz Kartı */}
        <div className="bg-white rounded-xl shadow-lg p-6 transform hover:scale-105 transition-transform duration-300 md:col-span-2">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">Değerlerimiz</h2>
          <ul className="list-disc list-inside text-gray-600 space-y-2">
            <li><span className="font-medium">Topluluk:</span> Film severleri bir araya getirerek güçlü bir topluluk oluşturuyoruz.</li>
            <li><span className="font-medium">Keşif:</span> Yeni filmler ve türler keşfetmenizi sağlıyoruz.</li>
            <li><span className="font-medium">Özgürlük:</span> Herkesin fikirlerini özgürce paylaşabileceği bir alan sunuyoruz.</li>
            <li><span className="font-medium">Kalite:</span> Kullanıcı dostu bir deneyim ve yüksek kaliteli içerik sunuyoruz.</li>
          </ul>
        </div>
      </section>

      {/* Çağrı Eylemi (CTA) */}
      <section className="mt-12 text-center">
        <div className="bg-gradient-to-r from-blue-500 to-purple-600 rounded-xl p-8 text-white">
          <h2 className="text-3xl font-bold mb-4">Moviemo Topluluğuna Katıl!</h2>
          <p className="text-lg mb-6">
            Hemen üye ol, favori filmlerini incele ve sinema tutkunlarıyla bağlantı kur.
          </p>
          <a
            href="/signup"
            className="inline-block bg-white text-blue-600 font-semibold px-6 py-3 rounded-lg hover:bg-gray-100 transition-colors"
          >
            Şimdi Katıl
          </a>
        </div>
      </section>
    </div>
  );
}