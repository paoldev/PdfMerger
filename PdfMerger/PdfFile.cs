using System.Drawing.Printing;
using Windows.Storage;
using Windows.Storage.Streams;

namespace PdfMerger
{
    internal class PdfFile
    {
        public string FileName => _fileName;
        public IReadOnlyList<Image> Pages => _pages;
        public static async Task<PdfFile> LoadFromFile(string fileName, IProgress<int>? progress = null, CancellationToken ct = default)
        {
            progress?.Report(0);
            var pdfFile = new PdfFile(fileName);
            if (Path.GetExtension(fileName).ToLower().Equals(".pdf"))
            {
                var file = await StorageFile.GetFileFromPathAsync(fileName);
                var pdf = await Windows.Data.Pdf.PdfDocument.LoadFromFileAsync(file);
                for (uint i = 0; i < pdf.PageCount; i++)
                {
                    using (var p = pdf.GetPage(i))
                    using (var memStream = new InMemoryRandomAccessStream())
                    {
                        await p.RenderToStreamAsync(memStream);
                        var pageWithStream = Image.FromStream(memStream.AsStreamForRead());

                        // Detach the stream from the image to avoid the following exception being thrown when saving the image,
                        // because the InMemoryRandomAccessStream is disposed by closing the current "using memStream" scope.
                        //      Exception thrown: 'System.ObjectDisposedException' in WinRT.Runtime.dll
                        // Infact, according to Image.FromFile documentation:
                        //      You must keep the stream open for the lifetime of the Image.
                        var pageImage = Helpers.DetachStreamFromImage(pageWithStream);

                        pdfFile._pages.Add(pageImage);
                    }

                    progress?.Report((int)((i + 1) * 100 / pdf.PageCount));
                    ct.ThrowIfCancellationRequested();
                }
            }
            else
            {
                //try to load the file as an image
                await Task.Run(() =>
                {
                    var pageImage = Image.FromFile(fileName);
                    pdfFile._pages.Add(pageImage);

                    progress?.Report(100);
                    ct.ThrowIfCancellationRequested();
                }, ct);
            }
            return pdfFile;
        }

        public static async Task<PdfFile> CreateFileAsync(string fileName, IReadOnlyList<Image> pages, IProgress<int>? progress = null, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var extension = Path.GetExtension(fileName).ToLowerInvariant();
                    bool bPdf = extension.Equals(".pdf");
                    using (PrintDocument printDocument = new())
                    {
                        PrinterSettings printerSettings = new()
                        {
                            PrinterName = bPdf ? "Microsoft Print To PDF" : "Microsoft XPS Document Writer",
                            PrintToFile = true,
                            PrintFileName = fileName
                        };
                        progress?.Report(0);
                        int currentPage = 0;
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DocumentName = Path.GetFileNameWithoutExtension(fileName);
                        printDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                        {
                            if (currentPage < pages.Count)
                            {
                                var pageImage = pages[currentPage];

                                //Fit the image into printed page bounds, according to their aspect ratios.
                                Rectangle imageRect = new() { X = 0, Y = 0, Width = pageImage.Width, Height = pageImage.Height };
                                Rectangle rectangle = Helpers.FitRectInBoundingBox(imageRect, e.PageBounds);
                                e.Graphics?.DrawImage(pageImage, rectangle);

                                currentPage++;
                            }

                            e.HasMorePages = currentPage < pages.Count;
                            e.Cancel = ct.IsCancellationRequested;
                            progress?.Report(currentPage);
                        };
                        printDocument.QueryPageSettings += (object sender, QueryPageSettingsEventArgs e) =>
                        {
                            if (currentPage < pages.Count)
                            {
                                var pageImage = pages[currentPage];
                                //Preserve original page orientation
                                e.PageSettings.Landscape = (pageImage.Width > pageImage.Height);
                            }
                        };
                        printDocument.BeginPrint += (object sender, PrintEventArgs e) => { currentPage = 0; };
                        printDocument.EndPrint += (object sender, PrintEventArgs e) => { };
                        printDocument.Print();
                    }

                    ct.ThrowIfCancellationRequested();

                    var pdfFile = new PdfFile(fileName);
                    pdfFile._pages.AddRange(pages);
                    return pdfFile;
                }
                catch
                {
                    try
                    {
                        // If the file does not exist, Delete succeeds without throwing an exception.
                        File.Delete(fileName);
                    }
                    catch { }
                    throw;
                }
            });
        }

        private PdfFile(string fileName)
        {
            _fileName = fileName;
        }

        private readonly string _fileName;
        private readonly List<Image> _pages = [];
    }
}
