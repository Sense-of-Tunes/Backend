namespace ST_Backend.Al
{
    using System.Text;

    using System.Text.Json;
    public class Yapay_Zeka
    {
        private readonly string _apiUrl;
        private readonly string _token;

        public Yapay_Zeka(IConfiguration configuration)
        {
            _apiUrl = configuration["YapayZekaApiUrl"];
            _token = configuration["YapayZekaToken"];
        }

        public async Task<string> AlModel(string kullaniciGirdi)
        {
            // JSON içeriğini hazırla
            string jsonContent = JsonSerializer.Serialize(new { inputs = kullaniciGirdi });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // HTTP POST isteği gönder
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

                // Doğru URI kullanılacak şekilde düzenleme
                var response = await client.PostAsync(new Uri(_apiUrl), content);

                // Yanıtı oku
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

}
