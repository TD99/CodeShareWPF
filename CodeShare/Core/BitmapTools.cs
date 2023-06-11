using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace CodeShare.Core
{
    public static class BitmapTools
    {
        public static BitmapImage RessoureToBitmapImage(Bitmap bitmap, ImageFormat format)
        {
            var img = bitmap;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, format);
                var bitmapImg = new BitmapImage();

                bitmapImg.BeginInit();
                bitmapImg.StreamSource = ms;
                bitmapImg.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImg.EndInit();

                return bitmapImg;
            }
        }
    }
}
