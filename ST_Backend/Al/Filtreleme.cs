namespace ST_Backend.Al
{
    
    //filtreleme sınıfı
    using Newtonsoft.Json;


    public class Filtreleme
    {
        private readonly MusicContext _musicContext;

        public Filtreleme(MusicContext musicContext)
        {
            _musicContext = musicContext;
        }

        public (string Emotion, float NegativeSc, float NeutralSc, float PositiveSc) Filtre(string responseString)
        {
            // JSON stringini bir liste olarak deserialize et
            List<List<ScoreItem>> outerList = JsonConvert.DeserializeObject<List<List<ScoreItem>>>(responseString); ;

            float negatifSkor = 0;
            float notrSkor = 0;
            float pozitifSkor = 0;

            // Her bir iç liste elemanını işle
            foreach (var innerList in outerList)
            {
                // İç liste içinde dolaşarak filtreleme işlemini gerçekleştir
                foreach (var item in innerList)
                {
                    switch (item.Label)
                    {
                        case "Negative":
                            negatifSkor = item.Score;
                            break;
                        case "Neutral":
                            notrSkor = item.Score;
                            break;
                        case "Positive":
                            pozitifSkor = item.Score;
                            break;
                        default:
                            // Bilinmeyen bir etiket durumu, gerekirse başka işlemler eklenebilir
                            break;
                    }
                }
            }

            // Duygu atama
            string emotion = GetEmotion(negatifSkor, notrSkor, pozitifSkor);

            return (Emotion: emotion, NegativeSc: negatifSkor, NeutralSc: notrSkor, PositiveSc: pozitifSkor);
        }

        private string GetEmotion(double negatifSkor, double notrSkor, double pozitifSkor)
        {
            // Skorları karşılaştırarak duyguyu belirle
            if (pozitifSkor > notrSkor && pozitifSkor > negatifSkor)
            {
                if (pozitifSkor >= 0.85)
                    return "Eglenceli";
                else
                    return "Mutlu";
            }
            else if (negatifSkor > notrSkor && negatifSkor > pozitifSkor)
            {
                if (negatifSkor >= 0.85)
                    return "Kederli";
                else
                    return "Uzuntu";
            }
            else if (notrSkor > negatifSkor && notrSkor > pozitifSkor)
            {
                return "Iyi";
            }
            else
            {
                return "Diğer";
            }
        }

    }
        // ScoreItem sınıfını tanımla
        public class ScoreItem
        {
            public string Label { get; set; }
            public float Score { get; set; }
        }

}
