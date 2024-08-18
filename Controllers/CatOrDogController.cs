using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DevAI.Controllers.Helpers;
using System.Net;

namespace DevAI.Controllers
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
        public async Task<PredictionResponse> Post(string imageURL)
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

            string tmpImageLoc = imageDir + $"/{filename}";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageURL, tmpImageLoc);
            }

            bool downloading = true;
            var client = new WebClient();

            client.DownloadFileCompleted += (sender, e) => downloading = false;
            client.DownloadFileAsync(uri, tmpImageLoc);

            while (downloading) 
            { 
                await Task.Delay(50); 
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
                return new PredictionResponse($"This is a Dog.", prediction.Score.Max() * 100);
            }
            else
            {
                return new PredictionResponse($"This is a Cat.", prediction.Score.Max() * 100);
            }
        }
    }
}
