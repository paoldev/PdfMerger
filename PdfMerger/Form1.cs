using System.Collections.Immutable;

namespace PdfMerger
{
    public partial class Form1 : Form
    {
        private readonly List<PdfFile> pdfFiles = [];
        private CancellationTokenSource? cts = null;

        public Form1()
        {
            InitializeComponent();

            comboBoxPageFilter.SelectedIndex = 0;   //Select "All pages" entry
            listViewPdf.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void OnaboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox1
            {
                StartPosition = FormStartPosition.CenterParent
            };
            aboutBox.ShowDialog(this);
        }

        private void OnexitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void OnbuttonSelect_Click(object sender, EventArgs e)
        {
            //get supported image decoders extensions
            var loadImageExtensions = Helpers.ImageDecodersExtensions;

            openFileDialog1.Filter = $"PDF files (*.pdf)|*.pdf|Image files ({loadImageExtensions})|{loadImageExtensions}|All files|*.*";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                await LoadPdfAsync(openFileDialog1.FileNames.AsEnumerable());
            }
        }

        private async void OnbuttonSave_Click(object sender, EventArgs e)
        {
            //get supported image encoders extensions
            var saveImageExtensions = Helpers.ImageEncodersExtensions;

            saveFileDialog1.Filter = $"PDF files (*.pdf)|*.pdf|One PDF per page (*.pdf)|*.pdf|OpenXPS files (*.oxps)|*.oxps|XPS files (*.xps)|*.xps|Image files ({saveImageExtensions})|{saveImageExtensions}|All files|*.*";
            saveFileDialog1.FilterIndex = 1;
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                var onePdfPerPage = saveFileDialog1.FilterIndex == 2;
                var sourcePages = listViewPreview.Items.Cast<ListViewItem>().Select(item => (Image)item.Tag!);
                await MergePdfAsync(sourcePages, saveFileDialog1.FileName, onePdfPerPage);
            }
        }

        private async Task LoadPdfAsync(IEnumerable<string> fileNames)
        {
            if (fileNames.Any())
            {
                UseWaitCursor = true;

                EnableAllControlsExceptCancel(false);
                progressBarPages.Maximum = 100;
                int currFile = 0;
                int OldPdfCount = pdfFiles.Count;
                cts = new CancellationTokenSource();
                foreach (var f in fileNames)
                {
                    currFile++;

                    var fileName = Path.GetFullPath(f);

                    void ReportProgress(int progress)
                    {
                        labelPages.Text = $"{currFile}/{fileNames.Count()} {fileName} {progress} %";
                        progressBarPages.Value = progress;
                    }

                    try
                    {
                        //Load pdf files
                        var pdfFile = await PdfFile.LoadFromFile(fileName, new Progress<int>(ReportProgress), cts.Token);
                        pdfFiles.Add(pdfFile);
                    }
                    catch (OperationCanceledException)
                    {
                        if (pdfFiles.Count > OldPdfCount)
                        {
                            //Remove Pdfs loaded before cancelling the task
                            if (MessageBox.Show($"Operation cancelled loading {fileName}.\n\nKeep previously loaded files?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            {
                                pdfFiles.RemoveRange(OldPdfCount, pdfFiles.Count - OldPdfCount);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Operation cancelled loading {fileName}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading file {fileName}.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //Reset the UI after previous tasks UI operations have been completed.
                await ClearUIAync(() =>
                {
                    labelPages.Text = string.Empty;
                    progressBarPages.Value = 0;
                });

                cts = null;

                UpdateListView();

                EnableAllControlsExceptCancel(true);
                UseWaitCursor = false;
            }
        }

        private async Task MergePdfAsync(IEnumerable<Image> sourcePages, string outputFile, bool onePdfPerPage)
        {
            UseWaitCursor = true;

            List<Image> pages = [.. sourcePages];

            progressBarPages.Maximum = pages.Count;
            void ReportProgress(int progress)
            {
                labelPages.Text = $"{outputFile} {progress}/{pages.Count}";
                progressBarPages.Value = progress;
            }

            EnableAllControlsExceptCancel(false);

            try
            {
                cts = new CancellationTokenSource();

                var extension = Path.GetExtension(outputFile).ToLowerInvariant();
                if (extension.Equals(".pdf") || extension.Equals(".xps") || extension.Equals(".oxps"))
                {
                    if (onePdfPerPage)
                    {
                        // Save each page as a separated pdf
                        var fileDir = Path.GetDirectoryName(outputFile) ?? Environment.CurrentDirectory;
                        var fileName = Path.GetFileNameWithoutExtension(outputFile);
                        var filePattern = Path.Combine(fileDir, fileName);

                        IProgress<int> progress = new Progress<int>(ReportProgress);
                        int pageCount = 1;
                        foreach (var page in pages)
                        {
                            var fullFileName = $"{filePattern}_pag_{pageCount}{extension}";
                            await PdfFile.CreateFileAsync(fullFileName, [page]);

                            progress?.Report(pageCount);
                            cts.Token.ThrowIfCancellationRequested();

                            pageCount++;
                        }
                        MessageBox.Show($"Files successfully saved in {fileDir}.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start("explorer.exe", fileDir);
                        }
                    }
                    else
                    {
                        // Save all pages into one file
                        await PdfFile.CreateFileAsync(outputFile, pages.AsEnumerable(), new Progress<int>(ReportProgress), cts.Token);

                        MessageBox.Show($"File {outputFile} successfully saved.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start("explorer.exe", outputFile);
                        }
                    }
                }
                else
                {
                    // Save each page as separated images, according to the selected extension
                    var encoder = Helpers.FindImageEncoder(extension);
                    if (encoder != null)
                    {
                        var fileDir = Path.GetDirectoryName(outputFile) ?? Environment.CurrentDirectory;
                        var fileName = Path.GetFileNameWithoutExtension(outputFile);
                        var filePattern = Path.Combine(fileDir, fileName);

                        IProgress<int> progress = new Progress<int>(ReportProgress);
                        await Task.Run(() =>
                        {
                            int pageCount = 1;
                            foreach (var page in pages)
                            {
                                var fullFileName = $"{filePattern}_pag_{pageCount}{extension}";
                                using (var imageStream = File.OpenWrite(fullFileName))
                                {
                                    page.Save(imageStream, encoder, null);
                                }

                                progress?.Report(pageCount);
                                cts.Token.ThrowIfCancellationRequested();

                                pageCount++;
                            }
                        }, cts.Token);
                        MessageBox.Show($"Files successfully saved in {fileDir}.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start("explorer.exe", fileDir);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Invalid file extension {extension}.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation cancelled.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating file " + outputFile + "." + Environment.NewLine + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Reset the UI after previous tasks UI operations have been completed.
            await ClearUIAync(() =>
            {
                labelPages.Text = string.Empty;
                progressBarPages.Value = 0;
            });

            cts = null;

            EnableAllControlsExceptCancel(true);

            UseWaitCursor = false;
        }

        private void OnbuttonMoveUp_Click(object sender, EventArgs e)
        {
            var selectedItems = listViewPdf.SelectedItems.Cast<ListViewItem>().ToList();

            if (selectedItems.Count > 0)
            {
                //Order by ascending item index
                selectedItems.Sort((ListViewItem x, ListViewItem y) => { return listViewPdf.Items.IndexOf(x).CompareTo(listViewPdf.Items.IndexOf(y)); });

                //Compute new indices
                var positions = selectedItems.Select(x => listViewPdf.Items.IndexOf(x)).ToList();
                for (int i = 0; i < positions.Count; i++)
                {
                    if (positions[i] > 0)
                    {
                        if ((i == 0) || (positions[i - 1] < (positions[i] - 1)))
                        {
                            positions[i]--;
                        }
                    }
                }

                //Move items
                bool bUpdatePreview = false;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var item = selectedItems[i];
                    if (listViewPdf.Items.IndexOf(item) != positions[i])
                    {
                        listViewPdf.Items.Remove(item);
                        listViewPdf.Items.Insert(positions[i], item);
                        bUpdatePreview = true;
                    }
                }
                if (bUpdatePreview)
                {
                    UpdatePreview();
                }
            }
        }

        private void OnbuttonMoveDown_Click(object sender, EventArgs e)
        {
            var selectedItems = listViewPdf.SelectedItems.Cast<ListViewItem>().ToList();

            if (selectedItems.Count > 0)
            {
                //Order by descending item index
                selectedItems.Sort((ListViewItem x, ListViewItem y) => { return listViewPdf.Items.IndexOf(y).CompareTo(listViewPdf.Items.IndexOf(x)); });

                //Compute new indices
                int maxIndex = listViewPdf.Items.Count - 1;
                var positions = selectedItems.Select(x => listViewPdf.Items.IndexOf(x)).ToList();
                for (int i = 0; i < positions.Count; i++)
                {
                    if (positions[i] < maxIndex)
                    {
                        if ((i == 0) || (positions[i - 1] > (positions[i] + 1)))
                        {
                            positions[i]++;
                        }
                    }
                }

                //Move items
                bool bUpdatePreview = false;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var item = selectedItems[i];
                    if (listViewPdf.Items.IndexOf(item) != positions[i])
                    {
                        listViewPdf.Items.Remove(item);
                        listViewPdf.Items.Insert(positions[i], item);
                        bUpdatePreview = true;
                    }
                }
                if (bUpdatePreview)
                {
                    UpdatePreview();
                }
            }
        }

        private void OnbuttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewPdf.SelectedItems.Count > 0)
            {
                //Remove some Pdfs from listViewPdf
                if (MessageBox.Show("Remove selected files?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var selectedItems = listViewPdf.SelectedItems.Cast<ListViewItem>();
                    foreach (var item in selectedItems)
                    {
                        pdfFiles.Remove(item.GetPdf());
                        listViewPdf.Items.Remove(item);
                    }
                    if (listViewPdf.Items.Count == 0)
                    {
                        listViewPdf.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }

                    UpdatePreview();
                }
            }
        }

        private void OnbuttonClear_Click(object sender, EventArgs e)
        {
            if (pdfFiles.Count > 0)
            {
                //Remove all Pdfs and start a new project
                if (MessageBox.Show("Clear the whole list?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pdfFiles.Clear();

                    UpdateListView();
                }
            }
        }

        private void UpdateListView()
        {
            listViewPdf.Items.Clear();
            foreach (var pdf in pdfFiles)
            {
                var listViewItem = new ListViewItem(Path.GetFileName(pdf.FileName));
                listViewItem.SetPdf(pdf);
                listViewItem.SubItems.Add(pdf.FileName);
                listViewPdf.Items.Add(listViewItem);
            }
            if (listViewPdf.Items.Count == 0)
            {
                listViewPdf.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else
            {
                listViewPdf.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }

            UpdatePreview();
        }

        private void UpdatePreview()
        {
            //Get all pages sorted by listViewPdf
            Dictionary<Image, PdfFile> pdfForGroups = [];
            List<Image> allPdfPages = [];
            foreach (ListViewItem item in listViewPdf.Items)
            {
                var pdf = item.GetPdf();
                allPdfPages.AddRange(pdf.Pages);

                foreach (var page in pdf.Pages)
                {
                    pdfForGroups.Add(page, pdf);
                }
            }

            //Extract only pages selected through the current filter
            var pageIndices = GetCustomFilterPageList();
            List<Image> filteredPages = [];
            foreach (var p in pageIndices)
            {
                filteredPages.Add(allPdfPages[p - 1]);
            }

            if (filteredPages.Count == 0)
            {
                groupBoxPreview.Text = "Preview";
            }
            else if (filteredPages.Count == 1)
            {
                groupBoxPreview.Text = "Preview: 1 page";
            }
            else
            {
                groupBoxPreview.Text = $"Preview: {filteredPages.Count} pages";
            }

            ImageList imageListLarge = new()
            {
                ImageSize = new Size(192, 192),
                ColorDepth = ColorDepth.Depth32Bit
            };

            listViewPreview.BeginUpdate();
            listViewPreview.Items.Clear();
            listViewPreview.Columns.Clear();
            listViewPreview.Columns.Add("Page");
            ListViewGroup? group = null;
            for (var pageIndex = 0; pageIndex < pageIndices.Length; pageIndex++)
            {
                var sourcePageIndex = pageIndices[pageIndex];
                var page = filteredPages[pageIndex];

                PdfFile? pdf = pdfForGroups.GetValueOrDefault(page);

                if (!(group?.Header?.Equals(pdf?.FileName) ?? false))
                {
                    group = new ListViewGroup(pdf?.FileName);
                    listViewPreview.Groups.Add(group);
                }

                ListViewItem pvitem = new((sourcePageIndex).ToString())
                {
                    Group = group,
                    Tag = page  //the original image
                };

                //Generate the thumbnail for the imageListLarge
                //var thumbnail = page.GetThumbnailImage(imageListLarge.ImageSize.Width, imageListLarge.ImageSize.Height, null, IntPtr.Zero);
                var thumbnail = Helpers.CreateImage(page, imageListLarge.ImageSize.Width, imageListLarge.ImageSize.Height, true);

                imageListLarge.Images.Add(thumbnail);
                pvitem.ImageIndex = imageListLarge.Images.Count - 1;
                listViewPreview.Items.Add(pvitem);
            }
            listViewPreview.LargeImageList = imageListLarge;
            listViewPreview.EndUpdate();

            UpdatePictureBox();
        }

        private void UpdatePictureBox()
        {
            //Show page preview
            if (listViewPreview.SelectedItems.Count > 0)
            {
                var image = listViewPreview.SelectedItems[0].Tag as Image;
                pictureBoxPreview.Image = image;
            }
            else
            {
                pictureBoxPreview.Image = null;
            }
        }

        private int[] GetCustomFilterPageList()
        {
            int minPage = 1;
            int maxPage = 0;

            foreach (ListViewItem item in listViewPdf.Items)
            {
                var pdf = item.GetPdf();
                var pages = pdf.Pages;
                maxPage += pages.Count();
            }

            if (maxPage < minPage)
            {
                //Empty array
                return [];
            }

            List<int> pageIndices = [];

            if ((comboBoxPageFilter.SelectedIndex == 0) || ((comboBoxPageFilter.SelectedIndex == 3) && string.IsNullOrWhiteSpace(textBoxFilter.Text)))
            {
                //All pages
                pageIndices = [.. Enumerable.Range(minPage, maxPage - minPage + 1)];
            }
            else if (comboBoxPageFilter.SelectedIndex == 1)
            {
                //Even pages
                pageIndices = [.. Enumerable.Range(minPage, maxPage - minPage + 1).Where(x => ((x % 2) == 0))];
            }
            else if (comboBoxPageFilter.SelectedIndex == 2)
            {
                //Odd pages
                pageIndices = [.. Enumerable.Range(minPage, maxPage - minPage + 1).Where(x => ((x % 2) != 0))];
            }
            else if (comboBoxPageFilter.SelectedIndex != 3)
            {
                //Empty array
                return [];
            }
            else
            {
                const string rangeSeparator = ",";
                const string intervalSeparator = "-";
                var ranges = textBoxFilter.Text.Split(rangeSeparator).Select(x => x.Trim());
                if (ranges.Any())
                {
                    foreach (var r in ranges)
                    {
                        if (string.IsNullOrWhiteSpace(r))
                        {
                            //Empty array
                            return [];
                        }

                        if (!r.Contains(intervalSeparator))
                        {
                            if (!int.TryParse(r, out int first))
                            {
                                //Empty array
                                return [];
                            }

                            if ((first < minPage) || (first > maxPage))
                            {
                                //Empty array
                                return [];
                            }
                            pageIndices.Add(first);
                        }
                        else
                        {
                            var rr = r.Split(intervalSeparator);
                            if (rr.Length == 2)
                            {
                                if (!int.TryParse(rr[0], out int first))
                                {
                                    //Empty array
                                    return [];
                                }

                                if (!int.TryParse(rr[1], out int second))
                                {
                                    //Empty array
                                    return [];
                                }

                                if (first <= second)
                                {
                                    if ((first < minPage) || (second > maxPage))
                                    {
                                        //Empty array
                                        return [];
                                    }

                                    pageIndices.AddRange([.. Enumerable.Range(first, second - first + 1)]);
                                }
                                else
                                {
                                    if ((second < minPage) || (first > maxPage))
                                    {
                                        //Empty array
                                        return [];
                                    }

                                    pageIndices.AddRange([.. Enumerable.Range(second, first - second + 1).Reverse()]);
                                }
                            }
                            else
                            {
                                //Empty array
                                return [];
                            }
                        }
                    }
                }
            }

            //Reverse page order
            if (checkBoxInvertPageOrder.Checked)
            {
                pageIndices.Reverse();
            }

            return [.. pageIndices];
        }

        private void EnableAllControlsExceptCancel(bool bEnable)
        {
            foreach (var c in Controls)
            {
                if (c is Control ctrl)
                {
                    if (ctrl != buttonCancel)
                    {
                        ctrl.Enabled = bEnable;
                    }
                }
            }
        }

        private static Task ClearUIAync(Action action)
        {
            IProgress<int> clearUI = new Progress<int>((int progress) =>
            {
                action();
            });
            return Task.Run(() =>
            {
                clearUI?.Report(0);
            });
        }

        private void OnbuttonCancel_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        private void OnlistViewPreview_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdatePictureBox();
        }

        private void OncomboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Recompute pages to be merged
            switch (comboBoxPageFilter.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                    textBoxFilter.Text = string.Empty;
                    textBoxFilter.Enabled = false;
                    break;

                case 3:
                    textBoxFilter.Enabled = true;
                    break;
            }
            UpdatePreview();
        }

        private void OntextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void OncheckBoxInvertPageOrder_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }
    }
}
