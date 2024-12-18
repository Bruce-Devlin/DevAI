﻿using DevAI.Controllers.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DevAI.Controllers
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
                return new PredictionResponse($"This is a positive comment.", prediction.Probability * 100);
            }
            else
            {
                return new PredictionResponse($"This is a negative comment.", prediction.Probability * 100);
            }
        }
    }
}
