using MongoDB.Driver;
using ST_Backend.Models;

public class MusicContext
{
    private readonly IMongoDatabase _database;
    private static MusicContext _instance;  // Singleton için özel bir static alan

    // Private constructor, dışarıdan doğrudan örneğe erişimi engeller
    private MusicContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    // Singleton örneğini almak için public metot
    public static MusicContext GetInstance(string connectionString, string databaseName)
    {
        if (_instance == null)
        {
            _instance = new MusicContext(connectionString, databaseName);
        }
        return _instance;
    }

    public IMongoCollection<Music> Musics => _database.GetCollection<Music>("Musics");


}
