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

            return Ok(duygu);
        }
    }
}
