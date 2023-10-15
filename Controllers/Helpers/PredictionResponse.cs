using Newtonsoft.Json;

namespace SentimentAI.Controllers.Helpers
{
    public class PredictionResponse
    {
        public override string ToString()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return jsonString;
        }

        public PredictionResponse(string newprediction = "") 
        {
            prediction = newprediction;
        }

        public string prediction {  get; set; }
    }
}
