using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Utilities
{
    public static class ImageUtilities
    {
        private const int DefaultWidth = 200;
        private const int DefaultHeight = 200;
        private const int DefaultJpegQuality = 90;
        private const long MaxImageSizeInBytes = 200 * 1024;

        public static void SetImageSource(Image imageControl, byte[] imageBytes, string defaultImagePath)
        {
            if (IsValidImage(imageBytes))
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    imageControl.Source = image;
                }
            }
            else
            {
                imageControl.Source = LoadBitmapFromPackUri(defaultImagePath);
            }
        }

        public static BitmapImage LoadBitmapFromPackUri(string packUri)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(packUri, UriKind.Absolute);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

        public static BitmapImage ConvertToImageSource(byte[] imageBytes)
        {
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                return LoadBitmapFromStream(memoryStream);
            }
        }

        public static byte[] ImageToByteArray(BitmapSource bitmapSource)
        {
            if (bitmapSource == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                var encoder = new JpegBitmapEncoder
                {
                    QualityLevel = DefaultJpegQuality
                };
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static bool AreImagesEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null) return true;
            if (array1 == null || array2 == null) return false;
            if (array1.Length != array2.Length) return false;

            for (int i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i]) return false;

            return true;
        }

        public static bool IsImageSizeValid(byte[] imageBytes)
        {
            return imageBytes != null && imageBytes.Length <= MaxImageSizeInBytes;
        }

        public static BitmapImage LoadAndResizeImage(string path)
        {
            var originalBitmap = LoadBitmapFromFile(path);
            var cropped = CropImageToCenter(originalBitmap);
            var resized = ResizeImage(cropped, DefaultWidth, DefaultHeight);
            return resized;
        }

        public static BitmapImage LoadBitmapFromFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return LoadBitmapFromStream(stream);
            }
        }

        private static BitmapImage LoadBitmapFromStream(Stream stream)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private static CroppedBitmap CropImageToCenter(BitmapImage original)
        {
            double cropSize = Math.Min(original.PixelWidth, original.PixelHeight);
            int x = (original.PixelWidth - (int)cropSize) / 2;
            int y = (original.PixelHeight - (int)cropSize) / 2;

            return new CroppedBitmap(original, new Int32Rect(x, y, (int)cropSize, (int)cropSize));
        }

        private static BitmapImage ResizeImage(BitmapSource source, int width, int height)
        {
            var scaleTransform = new ScaleTransform(
                (double)width / source.PixelWidth,
                (double)height / source.PixelHeight);

            var transformedBitmap = new TransformedBitmap(source, scaleTransform);

            var encoder = new JpegBitmapEncoder
            {
                QualityLevel = DefaultJpegQuality
            };
            encoder.Frames.Add(BitmapFrame.Create(transformedBitmap));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return LoadBitmapFromStream(ms);
            }
        }

        public static bool IsValidImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return false;

            try
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static byte[] ProcessImageBeforeSaving(string imagePath)
        {
            var resizedBitmap = LoadAndResizeImage(imagePath);
            var compressedImageBytes = ImageToByteArray(resizedBitmap);

            long imageSizeInKb = compressedImageBytes.Length / 1024;

            if (!IsImageSizeValid(compressedImageBytes))
            {
                throw new InvalidOperationException("The processed image exceeds the maximum allowed size.");
            }

            return compressedImageBytes;
        }
    }
}
