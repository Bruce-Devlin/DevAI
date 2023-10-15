using Microsoft.AspNetCore.Mvc;
using SentimentAI.Controllers.Helpers;

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
            PredictionResponse response = new PredictionResponse();


            if (prediction.PredictedLabel)
            {
                response.prediction = $"I predict this is a positive comment with {prediction.Probability}% Probability.";
            }
            else
            {
                response.prediction = $"I predict this is a negative comment with {prediction.Probability}% Probability.";
            }


            return response.ToString();
        }
    }
}
