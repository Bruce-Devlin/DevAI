using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SentimentAI.Controllers.Helpers;
using System.Net;

namespace SentimentAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatOrDogController : ControllerBase
    {
        private readonly ILogger<CatOrDogController> _logger;

        public CatOrDogController(ILogger<CatOrDogController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostCatOrDog")]
        public PredictionResponse Post(string imageURL)
        {
            string imageDir = Environment.CurrentDirectory + "/images/";
            if (!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
            }

            Uri uri = null;

            try
            {
                uri = new Uri(imageURL);
            }
            catch (Exception ex) 
            {
                return new PredictionResponse("Not a valid URL!");
            }

            
            string filename = Path.GetFileName(uri.LocalPath);
            if (!filename.EndsWith(".png") && !filename.EndsWith(".jpg") && !filename.EndsWith(".jpeg"))
            {
                return new PredictionResponse("The file must be a .png, .jpg or .jpeg");
            }

            string tmpImageLoc = imageDir + $"\\{filename}";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageURL, tmpImageLoc);
            }

            //Load sample data
            var imageBytes = System.IO.File.ReadAllBytes(tmpImageLoc);
            CatOrDogModel.ModelInput sampleData = new CatOrDogModel.ModelInput()
            {
                ImageSource = imageBytes,
            };
            
            //Load model and predict output
            var prediction = CatOrDogModel.Predict(sampleData);
            Directory.Delete(imageDir, true);

            if (prediction.PredictedLabel == "Dogs")
            {
                return new PredictionResponse($"This is a Dog.");
            }
            else
            {
                return new PredictionResponse($"This is a Cat.");
            }
        }
    }
}
