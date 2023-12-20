namespace ST_Backend.Models
{

    // Models/Music.cs
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    public class Music
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Muzik")]
        public string Muzik { get; set; }

        [BsonElement("Sanatci")]
        public string Sanatci { get; set; }

        [BsonElement("Duygu")]
        public string Duygu { get; set; }

        [BsonElement("Populerlik")]
        public int Populerlik { get; set; }

        [BsonElement("Resim_url")]
        public string Resim_url { get; set; }

        [BsonElement("Muzik_url")]
        public string Muzik_url { get; set; }
    }
}
