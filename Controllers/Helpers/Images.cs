using System.Net;

namespace DevAI.Controllers.Helpers
{
    public class Images
    {
        public static async Task<string> Download(string imageURL)
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
                return "";
            }


            string filename = Path.GetFileName(uri.LocalPath);
            if (!filename.EndsWith(".png") && !filename.EndsWith(".jpg") && !filename.EndsWith(".jpeg"))
            {
                return "";
            }

            string tmpImageLoc = imageDir + $"/{filename}";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageURL, tmpImageLoc);
            }

            bool downloading = true;
            var client = new WebClient();

            client.DownloadFileCompleted += (sender, e) => 
            { 
                downloading = false;
                client.Dispose();
            };
            client.DownloadFileAsync(uri, tmpImageLoc);

            while (downloading)
            {
                await Task.Delay(50);
            }
            return tmpImageLoc;
        }
    }
}
