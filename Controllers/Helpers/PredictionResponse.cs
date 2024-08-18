using Newtonsoft.Json;

namespace DevAI.Controllers.Helpers
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
            prediction = new Prediction();
            prediction.response = newPrediction;
            prediction.probability = newProbability;
        }

        public Prediction prediction { get; set; }
    }

    public class Prediction
    {
        public string response { get; set; }
        public float probability { get; set; }
    }

}
