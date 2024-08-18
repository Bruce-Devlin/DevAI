using Microsoft.AspNetCore.Mvc;
using DevAI.Controllers.Helpers;
using Humanizer;

namespace DevAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageCompareController : ControllerBase
    {
        private readonly ILogger<SentimentController> _logger;

        public ImageCompareController(ILogger<SentimentController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "ImageCompare")]
        public PredictionResponse Post(string image1URL, string image2URL)
        {
            double likeness = 0;
            bool theSame = ImageCompare.Compare(image1URL, image2URL, out likeness);

            if (theSame)
            {
                return new PredictionResponse($"This is the same image.", Convert.ToSingle(likeness));
            }
            else
            {
                return new PredictionResponse($"This is not the same image", Convert.ToSingle(likeness));
            }
        }
    }
}
