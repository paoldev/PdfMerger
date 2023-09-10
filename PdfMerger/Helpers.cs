using System.Drawing.Imaging;

namespace PdfMerger
{
    internal static class Helpers
    {
        //ListViewItem extensions to get and set Pdf files
        public static PdfFile GetPdf(this ListViewItem item)
        {
            return (PdfFile)item.Tag;
        }

        public static void SetPdf(this ListViewItem item, PdfFile pdf)
        {
            item.Tag = pdf;
        }

        //Get Image Encoders and Decoders as semi-colon separated file extensions (i.e. "*.jpg;*.bmp;...")
        public static string ImageEncodersExtensions => string.Join(";", ImageCodecInfo.GetImageEncoders().Select(enc => enc.FilenameExtension?.ToLowerInvariant()).Where(x => !string.IsNullOrWhiteSpace(x)));
        public static string ImageDecodersExtensions => string.Join(";", ImageCodecInfo.GetImageDecoders().Select(dec => dec.FilenameExtension?.ToLowerInvariant()).Where(x => !string.IsNullOrWhiteSpace(x)));

        //Find Image Encoder and Decoder given a file extension; the extension has to be prepended by a dot (i.e. ".jpg")
        public static ImageCodecInfo? FindImageEncoder(string extension) => ImageCodecInfo.GetImageEncoders().Where(enc => enc.FilenameExtension?.ToLowerInvariant().Split(";", StringSplitOptions.RemoveEmptyEntries).Contains($"*{extension}") ?? false).FirstOrDefault();
        public static ImageCodecInfo? FindImageDecoder(string extension) => ImageCodecInfo.GetImageDecoders().Where(dec => dec.FilenameExtension?.ToLowerInvariant().Split(";", StringSplitOptions.RemoveEmptyEntries).Contains($"*{extension}") ?? false).FirstOrDefault();

        //Fit and center a rectangle into a bounding box, preserving its original aspect ratio.
        //If the bounding box has negative width and/or height, the resulting rectangle will
        //have the same negative dimension.
        public static Rectangle FitRectInBoundingBox(Rectangle sourceRect, Rectangle boundingBox)
        {
            Rectangle rectangle = boundingBox;
            if ((Math.Abs(sourceRect.Width) != Math.Abs(boundingBox.Width)) || (Math.Abs(sourceRect.Height) != Math.Abs(boundingBox.Height)))
            {
                var fixedSourceRect = NormalizeRect(sourceRect);
                var fixedBoundingBox = NormalizeRect(boundingBox);

                int NewWidth;
                int NewHeight;
                if ((fixedBoundingBox.Width * fixedBoundingBox.Height) == 0)
                {
                    //Empty bounding box
                    NewWidth = 0;
                    NewHeight = 0;
                }
                else if ((fixedSourceRect.Width * fixedSourceRect.Height) == 0)
                {
                    //Empty source rect
                    NewWidth = (fixedSourceRect.Width == 0) ? 0 : fixedBoundingBox.Width;
                    NewHeight = (fixedSourceRect.Height == 0) ? 0 : fixedBoundingBox.Height;
                }
                else
                {
                    //Valid rects
                    NewWidth = fixedBoundingBox.Width;
                    NewHeight = fixedBoundingBox.Height;
                    double sourceAspectRatio = (double)fixedSourceRect.Width / fixedSourceRect.Height;
                    double destAspectRatio = (double)fixedBoundingBox.Width / fixedBoundingBox.Height;
                    if (Math.Abs((sourceAspectRatio / destAspectRatio) - 1.0) > 0.001)
                    {
                        if (destAspectRatio > sourceAspectRatio)
                        {
                            NewWidth = Math.Min((int)Math.Round(fixedBoundingBox.Height * sourceAspectRatio), fixedBoundingBox.Width);
                        }
                        else
                        {
                            NewHeight = Math.Min((int)Math.Round(fixedBoundingBox.Width / sourceAspectRatio), fixedBoundingBox.Height);
                        }
                    }
                }

                rectangle.X = fixedBoundingBox.X + (fixedBoundingBox.Width - NewWidth) / 2;
                rectangle.Y = fixedBoundingBox.Y + (fixedBoundingBox.Height - NewHeight) / 2;
                rectangle.Width = NewWidth;
                rectangle.Height = NewHeight;

                if (boundingBox.Width < 0)
                {
                    rectangle.X += rectangle.Width;
                    rectangle.Width = -rectangle.Width;
                }
                if (boundingBox.Height < 0)
                {
                    rectangle.Y += rectangle.Height;
                    rectangle.Height = -rectangle.Height;
                }
            }
            return rectangle;
        }

        //For rectangles with negative width and/or heigth, return a new rectangle with positive dimensions and the offset updated as well.
        private static Rectangle NormalizeRect(Rectangle rectangle)
        {
            return new()
            {
                X = rectangle.X + Math.Min(0, rectangle.Width),
                Y = rectangle.Y + Math.Min(0, rectangle.Height),
                Width = Math.Abs(rectangle.Width),
                Height = Math.Abs(rectangle.Height)
            };
        }

        //Image constructor used to detach internal referenced Stream, for images created through Image.FromStream(InStream).
        public static Image DetachStreamFromImage(Image image)
        {
            // Detach the stream from the image to avoid the following exception being thrown when saving the image,
            // because the InMemoryRandomAccessStream is disposed by closing the scope in which the stream is created.
            //      Exception thrown: 'System.ObjectDisposedException' in WinRT.Runtime.dll
            // Infact, according to Image.FromFile documentation:
            //      You must keep the stream open for the lifetime of the Image.
            //
            // Can't use "var bmp = new Bitmap(image)" because this constructor doesn't preserve
            // the source image resolution.
            var bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
            }
            return bmp;
        }

        //Image constructor used to center an image into a specific bounding box.
        public static Image CreateImage(Image image, int NewWidth, int NewHeight, bool PreserveImageAspectRatio)
        {
            var bmp = new Bitmap(NewWidth, NewHeight, PixelFormat.Format32bppArgb);
            bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                //Fit the source image into the destination image bounds, according to their aspect ratios.
                Rectangle destRect = new() { X = 0, Y = 0, Width = bmp.Width, Height = bmp.Height };
                if (PreserveImageAspectRatio && ((bmp.Width != image.Width) || (bmp.Height != image.Height)))
                {
                    Rectangle imageRect = new() { X = 0, Y = 0, Width = image.Width, Height = image.Height };
                    destRect = FitRectInBoundingBox(imageRect, destRect);
                }

                g.DrawImage(image, destRect);
            }
            return bmp;
        }
    }
}
