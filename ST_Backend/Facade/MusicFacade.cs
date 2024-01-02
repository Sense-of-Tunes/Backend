// MusicFacade
using MongoDB.Driver;
using ST_Backend.Al;
using ST_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST_Backend.Facade
{
    public class MusicFacade
    {
        private readonly IMongoDatabase _database;
        private readonly Yapay_Zeka _yapayZeka;
        private readonly Filtreleme _filtreleme;

        public MusicFacade(IMongoDatabase database, Yapay_Zeka yapayZeka, Filtreleme filtreleme)
        {
            _database = database;
            _yapayZeka = yapayZeka;
            _filtreleme = filtreleme;
        }

        public async Task<IEnumerable<Music>> GetMusicByUserInput(string userInput)
        {
            // Yapay Zeka sınıfı ile analiz yap
            string result = await _yapayZeka.AlModel(userInput);

            // Filtreleme sınıfı ile analiz sonuçlarını al
            var filtreSonuc = _filtreleme.Filtre(result);

            // Analiz sonucuna göre duygu koleksiyonundan veri çek
            string duygu = filtreSonuc.Emotion;

            try
            {
                var collectionName = duygu;
                var duyguKoleksiyonu = _database.GetCollection<Music>(collectionName);
                var duyguVerileri = await duyguKoleksiyonu.AsQueryable().ToListAsync();

                var random = new Random();
                var randomDuyguVerileri = duyguVerileri.OrderBy(x => random.Next()).Take(20).ToList();

                return randomDuyguVerileri;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya gerekli işlemleri gerçekleştirme
                throw new Exception($"Error retrieving music: {ex.Message}");
            }
        }

    }
}
