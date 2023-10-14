﻿using Microsoft.AspNetCore.Mvc;

namespace SentimentAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIController : ControllerBase
    {
        private readonly ILogger<AIController> _logger;

        public AIController(ILogger<AIController> logger)
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
                result = $"I predict this is a positive comment with {prediction.Probability}% accuracy";
            }
            else
            {
                result = $"I predict this is a negative comment with {prediction.Probability}% accuracy";
            }


            return result;
        }
    }
}
