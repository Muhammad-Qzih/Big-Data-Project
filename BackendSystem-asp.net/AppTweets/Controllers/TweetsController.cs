using AppTweets.Models.Tweets;
using AppTweets.Models.Tweets.AppTweets.Models.Tweets;
using AppTweets.Services.TweetServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppTweets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        public readonly ITweetService _tweetService;

        public TweetsController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpGet]
        public async Task<ActionResult<Tweet>> GetAllTweet()
        {
            try
            {
                IEnumerable<Tweet> tweets = await _tweetService.GetFirst10TweetsAsync();
                return Ok(tweets);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTweet: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("top")]
        public async Task<ActionResult<Tweet>> Get20Count()
        {
            try
            {
                IEnumerable<UserTweetCount> tweets = await _tweetService.GetTop20UsersAsync();
                return Ok(tweets);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTweet: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("distribution")]
        public async Task<ActionResult<List<TweetsDisriputionOverTime>>> GetTrendDataAsync(
            [FromQuery] string userName
        )
        {
            try
            {
                var tweetsDistribution = await _tweetService.GetTrendDataAsync(userName);
                return Ok(tweetsDistribution);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTweetDistribution: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
