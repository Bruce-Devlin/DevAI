using Microsoft.VisualBasic;
using System.Drawing;
using System.Net;
using static Tensorflow.TensorShapeProto.Types;

namespace DevAI.Controllers.Helpers
{
    public class ImageCompare
    {
        public static bool Compare(string image1URL, string image2URL, out double likeness)
        {
            var tmpImage1URI = Images.Download(image1URL).Result;
            var tmpImage2URI = Images.Download(image2URL).Result;

            var image1 = (Bitmap)Image.FromFile(tmpImage1URI);
            var image2 = (Bitmap)Image.FromFile(tmpImage2URI);

            var pixelatedImage1 = Pixelate(image1);
            var pixelatedImage2 = Pixelate(image2);

            var interpretedImage1 = Interpret(pixelatedImage1);
            var interpretedImage2 = Interpret(pixelatedImage2);

            var equal = interpretedImage1.SequenceEqual(interpretedImage2);

            double matchingTotal = interpretedImage1.Where(x => interpretedImage2.Contains(x)).Count();
            double total = interpretedImage1.Count();

            var result = matchingTotal / total;
            likeness = result * 100;

            return equal;
        }

        private static Bitmap Pixelate(Bitmap image)
        {
            Bitmap resizedSmall = new Bitmap(image, new Size(image.Width / 8, image.Height / 8));
            Bitmap resizedLarge = new Bitmap(resizedSmall, new Size(resizedSmall.Width * 4, resizedSmall.Height * 4));
            Bitmap resizedFinal = new Bitmap(resizedLarge, new Size(image.Width / 3, image.Height / 3));

            Bitmap finalImage = ReplaceTransparency(resizedFinal);

            return finalImage;
        }

        private static Bitmap ReplaceTransparency(Bitmap bitmap)
        {
            /* Important: you have to set the PixelFormat to remove the alpha channel.
             * Otherwise you'll still have a transparent image - just without transparent areas */
            var result = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var g = Graphics.FromImage(result);

            g.Clear(Color.White);
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            g.DrawImage(bitmap, 0, 0);

            return result;
        }

        private static byte[] Interpret(Bitmap image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
