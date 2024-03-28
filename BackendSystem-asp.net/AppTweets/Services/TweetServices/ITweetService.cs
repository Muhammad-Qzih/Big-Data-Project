using AppTweets.Models.Tweets;
using AppTweets.Models.Tweets.AppTweets.Models.Tweets;

namespace AppTweets.Services.TweetServices
{
    public interface ITweetService
    {
        Task<List<Tweet>> GetFirst10TweetsAsync();

        Task<List<UserTweetCount>> GetTop20UsersAsync();
        
        Task<List<TweetsDisriputionOverTime>> GetTrendDataAsync(string userName);
    }
}
