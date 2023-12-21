using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using ST_Backend.Al;
using ST_Backend.Models;


namespace ST_Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMongoDatabase _database;
        private readonly MusicContext _musicContext;
        private readonly Yapay_Zeka _yapayZeka;
        private readonly Filtreleme _filtreleme;

        public MusicController(MusicContext context, Yapay_Zeka yapayZeka, Filtreleme filtreleme)
        {
            _database = context.Musics.Database;
            _musicContext = context;
            _yapayZeka = yapayZeka;
            _filtreleme = filtreleme;
        }

        // Tüm müzik verilerini getiren endpoint
        [HttpGet]
        public ActionResult<List<Music>> Get()
        {
            try
            {
                // MongoDB'den "Musics" koleksiyonundaki tüm müzikleri çek ve listeye dönüştür.
                var musics = _database.GetCollection<Music>("Musics").Find(m => true).ToList();
                return Ok(musics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        // Belirli bir duygu için popüler müzikleri getiren endpoint
        [HttpGet("getEmojidenDuygu")]
        public ActionResult<List<Music>> EmojidenDuygu(string duygu, int limit = 20)
        {
            try
            {
                // Verilen duygu adına sahip MongoDB koleksiyonunu al.
                var collectionName = duygu;
                var duyguCollection = _database.GetCollection<Music>(collectionName);

                // Belirli duygu için popülerlik sırasına göre sırala ve belirlenen limitteki verileri çek.
                var duyguVerileri = duyguCollection
                    .Find(m => true)
                    .SortByDescending(m => m.Populerlik)
                    .Limit(limit)
                    .ToList();

                return Ok(duyguVerileri);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // Duygu emojilerini getiren endpoint
        [HttpGet("getDuyguEmojileri")]
        public ActionResult<List<DuyguEmoji>> GetDuyguEmojileri()
        {
            try
            {
                // "DuyguEmojileri" koleksiyonundaki tüm duygu emojilerini çek ve listeye dönüştür.
                var duyguEmojileri = _database.GetCollection<DuyguEmoji>("DuyguEmojileri")
                    .Find(m => true)
                    .ToList();

                return Ok(duyguEmojileri);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // Trend müziklerini getiren endpoint
        [HttpGet("getTrendMuzikler")]
        public ActionResult<List<Music>> GetTrendMuzikler()
        {
            try
            {
                // "Trend" koleksiyonundan tüm verileri çek.
                var trendMuzikler = _database.GetCollection<Music>("Trend")
                    .Find(m => true)
                    .ToList();

                return Ok(trendMuzikler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }




        // Kullanıcı girdisi üzerinden duygu analizi yaparak müzik önerileri getiren endpoint
        [HttpPost("getHisAnaliz_DuyguGetir")]
        public async Task<ActionResult<IEnumerable<Music>>> GetHisAnaliz_DuyguGetir([FromBody] kullaniciGirdiModel kullaniciGirdisi)
        {
            if (kullaniciGirdisi == null || string.IsNullOrEmpty(kullaniciGirdisi.kullaniciGirdi))
            {
                return BadRequest("Kullanıcı girdisi gereklidir.");
            }

            // Yapay Zeka sınıfı ile analiz yap
            string result = await _yapayZeka.AlModel(kullaniciGirdisi.kullaniciGirdi);

            // Filtreleme sınıfı ile analiz sonuçlarını al
            var filtreSonuc = _filtreleme.Filtre(result);

            // Console'a yazdır
            Console.WriteLine($"Yapay Zeka Sonucu: {result}");
            Console.WriteLine($"Filtreleme Sonucu - Emotion: {filtreSonuc.Emotion}");

            // Analiz sonucuna göre duygu koleksiyonundan veri çek
            string duygu = filtreSonuc.Emotion;

            try
            {
                var collectionName = duygu;
                var duyguKoleksiyonu = _database.GetCollection<Music>(collectionName);

                var randomDuyguVerileri = duyguKoleksiyonu.AsQueryable()
                    .OrderBy(x => Guid.NewGuid())  // Koleksiyonu rastgele sırala
                    .Take(20)                       // İlk 20 öğeyi al
                    .ToList();

                return Ok(randomDuyguVerileri);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
