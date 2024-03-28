namespace AppTweets.Models.Tweets
{
    namespace AppTweets.Models.Tweets
    {
        public class TweetsDisriputionOverTime
        {
            public DistributionId DistributionId { get; set; }
            public int CountTweets { get; set; }
        }

        public class DistributionId
        {
            public DateTime Date { get; set; }
            public string User { get; set; }
        }
    }

}
