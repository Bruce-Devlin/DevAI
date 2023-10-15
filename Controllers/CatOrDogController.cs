using Microsoft.AspNetCore.Mvc;
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
        public string Post(string imageURL)
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
                return "Not a valid URL!";
            }

            
            string filename = Path.GetFileName(uri.LocalPath);
            if (!filename.EndsWith(".png") && !filename.EndsWith(".jpg") && !filename.EndsWith(".jpeg"))
            {
                return "The file must be a .png, .jpg or .jpeg";
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
            string result = "";


            if (prediction.PredictedLabel == "Dogs")
            {
                result = $"\"I predict this is a Dog.\"";
            }
            else
            {
                result = $"\"I predict this is a Cat.\"";
            }
            System.IO.File.Delete(tmpImageLoc);

            return result;


        }
    }
}
