using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Tweet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("id")]
    public long TweetId { get; set; }

    [BsonElement("target")]
    public int Target { get; set; }

    [BsonElement("flag")]
    public required string Flag { get; set; }

    [BsonElement("date")]
    public required DateTime Date { get; set; }

    [BsonElement("user")]
    public required string User { get; set; }

    [BsonElement("text")]
    public required string Text { get; set; }

    [BsonElement("producedTweet")]
    public long ProducedTweet { get; set; }
}
