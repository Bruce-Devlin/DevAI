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
            response = newPrediction;
            probability = newProbability;
        }

        public string response { get; set; }
        public float probability { get; set; }
    }
}
