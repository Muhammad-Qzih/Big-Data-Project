namespace AppTweets.Models.MongoSettings
{
    public class MongoDbSettings:IMongoDbSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string CollectionName { get; set; }

    }
}
