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
        //Note 1: sourceRect offset is ignored; only Width and Height are considered.
        //Note 2: empty rects are ignored and the bounding box is returned.
        //Note 3: unexpected results may be returned if rects have negative widths and/or heights.
        public static Rectangle FitRectInBoundingBox(Rectangle sourceRect, Rectangle boundingBox)
        {
            Rectangle rectangle = boundingBox;
            if ((sourceRect.Width != boundingBox.Width) || (sourceRect.Height != boundingBox.Height))
            {
                if (((sourceRect.Width * sourceRect.Height) != 0) && ((boundingBox.Width * boundingBox.Height) != 0))
                {
                    double imageAspectRatio = (double)sourceRect.Width / sourceRect.Height;
                    double pageAspectRatio = (double)boundingBox.Width / boundingBox.Height;
                    if (Math.Abs((imageAspectRatio / pageAspectRatio) - 1.0) > 0.001)
                    {
                        if (pageAspectRatio > imageAspectRatio)
                        {
                            rectangle.Width = Math.Min((int)Math.Round(boundingBox.Height * imageAspectRatio), boundingBox.Width);
                            rectangle.Height = boundingBox.Height;
                            rectangle.X = (boundingBox.Width - rectangle.Width) / 2;
                            rectangle.Y = 0;
                        }
                        else
                        {
                            rectangle.Width = boundingBox.Width;
                            rectangle.Height = Math.Min((int)Math.Round(boundingBox.Width / imageAspectRatio), boundingBox.Height);
                            rectangle.X = 0;
                            rectangle.Y = (boundingBox.Height - rectangle.Height) / 2;
                        }

                        rectangle.Offset(boundingBox.X, boundingBox.Y);
                    }
                }
            }
            return rectangle;
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
