using Microsoft.AspNetCore.Mvc;

namespace SentimentAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SentimentController : ControllerBase
    {
        private readonly ILogger<SentimentController> _logger;

        public SentimentController(ILogger<SentimentController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostSentiment")]
        public string Post(string comment)
        {
            //Load sample data
            var sampleData = new SentimentModel.ModelInput()
            {
                Col0 = comment,
            };

            //Load model and predict output
            var prediction = SentimentModel.Predict(sampleData);
            string result = "";


            if (prediction.PredictedLabel)
            {
                result = $"I predict this is a positive comment with {prediction.Probability}% Probability.";
            }
            else
            {
                result = $"I predict this is a negative comment with {prediction.Probability}% Probability.";
            }


            return result;
        }
    }
}
