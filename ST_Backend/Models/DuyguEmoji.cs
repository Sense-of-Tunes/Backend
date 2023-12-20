using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class DuyguEmoji
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Duygu { get; set; }

    public string Emoji { get; set; }
}
