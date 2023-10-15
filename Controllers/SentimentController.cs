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
        public PredictionResponse Post(string comment)
        {
            //Load sample data
            var sampleData = new SentimentModel.ModelInput()
            {
                Col0 = comment,
            };

            //Load model and predict output
            var prediction = SentimentModel.Predict(sampleData);


            if (prediction.PredictedLabel)
            {
                return new PredictionResponse($"This is a positive comment.", prediction.Probability);
            }
            else
            {
                return new PredictionResponse($"This is a negative comment.", prediction.Probability);
            }
        }
    }
}
