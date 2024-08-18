using Microsoft.VisualBasic;
using System.Drawing;
using System.Net;
using static Tensorflow.TensorShapeProto.Types;

namespace DevAI.Controllers.Helpers
{
    public class ImageCompare
    {
        public static bool Compare(string image1URL, string image2URL, out double confidence)
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

            byte[] a1 = interpretedImage1;
            byte[] a2 = interpretedImage2;
            int i, j, flag, x, k = 0;
            int sameCount = 0;

            // To traverse in array1.
            for (i = 0; i < a1.Length; i++)
            {
                if (a2.Length > i) if (a1[i] == a2[i]) sameCount = sameCount + 1;
            }

            double total = interpretedImage1.Count();

            var matching = sameCount / total;
            var matchingPersent = matching * 100;
            confidence = 100 - matchingPersent;


            image1.Dispose();
            image2.Dispose();
            pixelatedImage1.Dispose(); 
            pixelatedImage2.Dispose();

            return equal;
        }

        private static Bitmap Pixelate(Bitmap image)
        {
            Bitmap resizedSmall = new Bitmap(image, new Size(image.Width / 4, image.Height / 4));
            Bitmap resizedLarge = new Bitmap(resizedSmall, new Size(resizedSmall.Width * 4, resizedSmall.Height * 4));

            Bitmap finalImage = ReplaceTransparency(resizedLarge);

            resizedSmall.Dispose();
            resizedLarge.Dispose();

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
