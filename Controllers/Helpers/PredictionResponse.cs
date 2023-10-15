using Newtonsoft.Json;

namespace SentimentAI.Controllers.Helpers
{
    public class PredictionResponse
    {
        public override string ToString()
        {
            string jsonString = JsonConvert.SerializeObject(this);
            return jsonString;
        }

        public PredictionResponse(string newPrediction, float newProbability = -1) 
        {
            prediction.response = newPrediction;
            prediction.probability = newProbability;
        }

        public class prediction 
        {
            public static string response { get; set; }
            public static float probability { get; set; }
        }
    }
}
