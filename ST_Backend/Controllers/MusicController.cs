// Gerekli using bildirimleri eklenmiş hali
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ST_Backend.Facade;  // MusicFacade sınıfının bulunduğu namespace
using ST_Backend.Models;
using ST_Backend.Al;
using MongoDB.Driver;

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

        [HttpGet]
        public ActionResult<List<Music>> Get()
        {
            try
            {
                var musics = _database.GetCollection<Music>("Musics").Find(m => true).ToList();
                return Ok(musics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("getEmojidenDuygu")]
        public ActionResult<List<Music>> EmojidenDuygu(string duygu, int limit = 20)
        {
            try
            {
                var collectionName = duygu;
                var duyguCollection = _database.GetCollection<Music>(collectionName);

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

        [HttpGet("getDuyguEmojileri")]
        public ActionResult<List<DuyguEmoji>> GetDuyguEmojileri()
        {
            try
            {
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

        [HttpGet("getTrendMuzikler")]
        public ActionResult<List<Music>> GetTrendMuzikler()
        {
            try
            {
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

        [HttpPost("getHisAnaliz_DuyguGetir")]
        public async Task<ActionResult<IEnumerable<Music>>> GetHisAnaliz_DuyguGetir([FromBody] kullaniciGirdiModel kullaniciGirdisi)
        {
            if (kullaniciGirdisi == null || string.IsNullOrEmpty(kullaniciGirdisi.kullaniciGirdi))
            {
                return BadRequest("Kullanıcı girdisi gereklidir.");
            }

            try
            {
                var musicFacade = new MusicFacade(_database, _yapayZeka, _filtreleme);
                var musicList = await musicFacade.GetMusicByUserInput(kullaniciGirdisi.kullaniciGirdi);

                return Ok(musicList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
