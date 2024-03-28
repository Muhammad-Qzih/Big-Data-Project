using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppTweets.Models;
using AppTweets.Models.MongoSettings;
using AppTweets.Models.Tweets;
using AppTweets.Models.Tweets.AppTweets.Models.Tweets;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppTweets.Services.TweetServices
{
    public class TweetService : ITweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;

        public TweetService(IMongoDbSettings mongoDbSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
            _tweetCollection = database.GetCollection<Tweet>(mongoDbSettings.CollectionName);
        }

        public async Task<List<Tweet>> GetFirst10TweetsAsync()
        {
            try
            {
                var tweets = await _tweetCollection.Find(tweet => true).Limit(10).ToListAsync();

                Console.WriteLine(tweets.FirstOrDefault());
                return tweets;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFirst10TweetsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<UserTweetCount>> GetTop20UsersAsync()
        {
            try
            {
                var userTweetCounts = await _tweetCollection
                    .Aggregate()
                    .Group(
                        new BsonDocument
                        {
                            { "_id", "$user" },
                            { "tweetCount", new BsonDocument("$sum", 1) }
                        }
                    )
                    .Sort(new BsonDocument("tweetCount", -1))
                    .Limit(20)
                    .ToListAsync();

                var topUsers = userTweetCounts
                    .Select(
                        document =>
                            new UserTweetCount
                            {
                                UserName = document["_id"].AsString,
                                TweetCount = document["tweetCount"].AsInt32
                            }
                    )
                    .ToList();

                Console.WriteLine(topUsers.FirstOrDefault());
                return topUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTop20UsersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TweetsDisriputionOverTime>> GetTrendDataAsync(string userName)
        {
            try
            {
                var aggregationPipeline = new List<BsonDocument>
                {
                    new BsonDocument("$match", new BsonDocument("user", userName)),
                    new BsonDocument(
                        "$project",
                        new BsonDocument
                        {
                            { "_id", 0 },
                            {
                                "Date",
                                new BsonDocument(
                                    "$dateToString",
                                    new BsonDocument
                                    {
                                        { "format", "%Y-%m-%d" },
                                        { "date", "$date" }
                                    }
                                )
                            },

                            { "User", "$user" }
                        }
                    ),

                    new BsonDocument(
                        "$group",
                        new BsonDocument
                        {
                            {
                                "_id",
                                new BsonDocument { 
                                    { "Date", "$Date" },
                                    { "User", "$User" } 
                                }
                            },
                            { "countTweets", new BsonDocument("$sum", 1) }
                        }
                    ),

                    new BsonDocument(
                        "$project",
                        new BsonDocument
                        {
                            { "_id", 0 },

                            { "DistributionId", "$_id" },

                            { "CountTweets", "$countTweets" }
                        }
                    ),
                    new BsonDocument("$sort", new BsonDocument("DistributionId.Date", 1))
                };

                var trendData = await _tweetCollection
                    .Aggregate<TweetsDisriputionOverTime>(aggregationPipeline)
                    .ToListAsync();

                return trendData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTrendDataAsync: {ex.Message}");
                throw;
            }
        }
    }
}
