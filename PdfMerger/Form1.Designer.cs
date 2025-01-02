namespace PdfMerger
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ColumnHeader columnHeader1;
            ColumnHeader columnHeader2;
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            buttonSelect = new Button();
            openFileDialog1 = new OpenFileDialog();
            listViewPdf = new ListView();
            buttonSave = new Button();
            checkBox1 = new CheckBox();
            saveFileDialog1 = new SaveFileDialog();
            progressBarPages = new ProgressBar();
            labelPages = new Label();
            buttonMoveUp = new Button();
            buttonMoveDown = new Button();
            buttonCancel = new Button();
            buttonDelete = new Button();
            groupBoxPreview = new GroupBox();
            splitContainer1 = new SplitContainer();
            listViewPreview = new ListView();
            pictureBoxPreview = new PictureBox();
            buttonClear = new Button();
            groupBox2 = new GroupBox();
            checkBoxInvertPageOrder = new CheckBox();
            textBoxFilter = new TextBox();
            comboBoxPageFilter = new ComboBox();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            menuStrip1.SuspendLayout();
            groupBoxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "FileName";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "FullPath";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(4, 1, 0, 1);
            menuStrip1.Size = new Size(984, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 22);
            fileToolStripMenuItem.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += OnaboutToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(104, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(107, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += OnexitToolStripMenuItem_Click;
            // 
            // buttonSelect
            // 
            buttonSelect.Location = new Point(8, 28);
            buttonSelect.Margin = new Padding(2);
            buttonSelect.Name = "buttonSelect";
            buttonSelect.Size = new Size(78, 30);
            buttonSelect.TabIndex = 1;
            buttonSelect.Text = "Select files";
            buttonSelect.UseVisualStyleBackColor = true;
            buttonSelect.Click += OnbuttonSelect_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "pdf";
            openFileDialog1.FileName = "*.pdf";
            openFileDialog1.Filter = "Pdf files|*.pdf|All files|*.*";
            openFileDialog1.Multiselect = true;
            // 
            // listViewPdf
            // 
            listViewPdf.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listViewPdf.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listViewPdf.FullRowSelect = true;
            listViewPdf.Location = new Point(8, 64);
            listViewPdf.Margin = new Padding(2);
            listViewPdf.Name = "listViewPdf";
            listViewPdf.Size = new Size(847, 167);
            listViewPdf.TabIndex = 2;
            listViewPdf.UseCompatibleStateImageBehavior = false;
            listViewPdf.View = View.Details;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(8, 241);
            buttonSave.Margin = new Padding(2);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(78, 30);
            buttonSave.TabIndex = 3;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += OnbuttonSave_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(104, 248);
            checkBox1.Margin = new Padding(2);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(74, 19);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "Open file";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.FileName = "*.pdf";
            saveFileDialog1.Filter = "Pdf files|*.pdf|All files|*.*";
            // 
            // progressBarPages
            // 
            progressBarPages.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBarPages.Location = new Point(6, 287);
            progressBarPages.Margin = new Padding(2);
            progressBarPages.Name = "progressBarPages";
            progressBarPages.Size = new Size(967, 20);
            progressBarPages.TabIndex = 6;
            // 
            // labelPages
            // 
            labelPages.AutoSize = true;
            labelPages.Location = new Point(6, 271);
            labelPages.Margin = new Padding(2, 0, 2, 0);
            labelPages.Name = "labelPages";
            labelPages.Size = new Size(0, 15);
            labelPages.TabIndex = 8;
            // 
            // buttonMoveUp
            // 
            buttonMoveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonMoveUp.Location = new Point(859, 70);
            buttonMoveUp.Margin = new Padding(2);
            buttonMoveUp.Name = "buttonMoveUp";
            buttonMoveUp.Size = new Size(117, 30);
            buttonMoveUp.TabIndex = 9;
            buttonMoveUp.Text = "Move up";
            buttonMoveUp.UseVisualStyleBackColor = true;
            buttonMoveUp.Click += OnbuttonMoveUp_Click;
            // 
            // buttonMoveDown
            // 
            buttonMoveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonMoveDown.Location = new Point(859, 104);
            buttonMoveDown.Margin = new Padding(2);
            buttonMoveDown.Name = "buttonMoveDown";
            buttonMoveDown.Size = new Size(117, 30);
            buttonMoveDown.TabIndex = 10;
            buttonMoveDown.Text = "Move down";
            buttonMoveDown.UseVisualStyleBackColor = true;
            buttonMoveDown.Click += OnbuttonMoveDown_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(265, 242);
            buttonCancel.Margin = new Padding(2);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(78, 30);
            buttonCancel.TabIndex = 11;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += OnbuttonCancel_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDelete.Location = new Point(859, 144);
            buttonDelete.Margin = new Padding(2);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(117, 30);
            buttonDelete.TabIndex = 12;
            buttonDelete.Text = "Remove selected";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += OnbuttonRemove_Click;
            // 
            // groupBoxPreview
            // 
            groupBoxPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxPreview.Controls.Add(splitContainer1);
            groupBoxPreview.Location = new Point(8, 396);
            groupBoxPreview.Margin = new Padding(2);
            groupBoxPreview.Name = "groupBoxPreview";
            groupBoxPreview.Padding = new Padding(2);
            groupBoxPreview.Size = new Size(988, 238);
            groupBoxPreview.TabIndex = 13;
            groupBoxPreview.TabStop = false;
            groupBoxPreview.Text = "Preview";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(2, 18);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listViewPreview);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBoxPreview);
            splitContainer1.Size = new Size(984, 218);
            splitContainer1.SplitterDistance = 645;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 1;
            // 
            // listViewPreview
            // 
            listViewPreview.Dock = DockStyle.Fill;
            listViewPreview.Location = new Point(0, 0);
            listViewPreview.Margin = new Padding(2);
            listViewPreview.Name = "listViewPreview";
            listViewPreview.Size = new Size(645, 218);
            listViewPreview.TabIndex = 0;
            listViewPreview.UseCompatibleStateImageBehavior = false;
            listViewPreview.ItemSelectionChanged += OnlistViewPreview_ItemSelectionChanged;
            // 
            // pictureBoxPreview
            // 
            pictureBoxPreview.BorderStyle = BorderStyle.Fixed3D;
            pictureBoxPreview.Dock = DockStyle.Fill;
            pictureBoxPreview.Location = new Point(0, 0);
            pictureBoxPreview.Margin = new Padding(2);
            pictureBoxPreview.Name = "pictureBoxPreview";
            pictureBoxPreview.Size = new Size(336, 218);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.TabIndex = 0;
            pictureBoxPreview.TabStop = false;
            // 
            // buttonClear
            // 
            buttonClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonClear.Location = new Point(859, 198);
            buttonClear.Margin = new Padding(2);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(115, 30);
            buttonClear.TabIndex = 14;
            buttonClear.Text = "Clear list";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += OnbuttonClear_Click;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(checkBoxInvertPageOrder);
            groupBox2.Controls.Add(textBoxFilter);
            groupBox2.Controls.Add(comboBoxPageFilter);
            groupBox2.Location = new Point(8, 320);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(965, 73);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Text = "Filter";
            // 
            // checkBoxInvertPageOrder
            // 
            checkBoxInvertPageOrder.AutoSize = true;
            checkBoxInvertPageOrder.Location = new Point(181, 30);
            checkBoxInvertPageOrder.Margin = new Padding(2);
            checkBoxInvertPageOrder.Name = "checkBoxInvertPageOrder";
            checkBoxInvertPageOrder.Size = new Size(116, 19);
            checkBoxInvertPageOrder.TabIndex = 2;
            checkBoxInvertPageOrder.Text = "Invert page order";
            checkBoxInvertPageOrder.UseVisualStyleBackColor = true;
            checkBoxInvertPageOrder.CheckedChanged += OncheckBoxInvertPageOrder_CheckedChanged;
            // 
            // textBoxFilter
            // 
            textBoxFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxFilter.Location = new Point(318, 29);
            textBoxFilter.Margin = new Padding(2);
            textBoxFilter.Name = "textBoxFilter";
            textBoxFilter.PlaceholderText = "for example 2-5, 7, 20-15, 1, 3";
            textBoxFilter.Size = new Size(645, 23);
            textBoxFilter.TabIndex = 1;
            textBoxFilter.TextChanged += OntextBoxFilter_TextChanged;
            // 
            // comboBoxPageFilter
            // 
            comboBoxPageFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPageFilter.FormattingEnabled = true;
            comboBoxPageFilter.Items.AddRange(new object[] { "All pages", "All even pages", "All odd pages", "Custom page ranges" });
            comboBoxPageFilter.Location = new Point(8, 29);
            comboBoxPageFilter.Margin = new Padding(2);
            comboBoxPageFilter.Name = "comboBoxPageFilter";
            comboBoxPageFilter.Size = new Size(162, 23);
            comboBoxPageFilter.TabIndex = 0;
            comboBoxPageFilter.SelectedIndexChanged += OncomboBoxFilter_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 637);
            Controls.Add(groupBox2);
            Controls.Add(buttonClear);
            Controls.Add(groupBoxPreview);
            Controls.Add(buttonDelete);
            Controls.Add(buttonCancel);
            Controls.Add(buttonMoveDown);
            Controls.Add(buttonMoveUp);
            Controls.Add(labelPages);
            Controls.Add(progressBarPages);
            Controls.Add(checkBox1);
            Controls.Add(buttonSave);
            Controls.Add(listViewPdf);
            Controls.Add(buttonSelect);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2);
            MinimumSize = new Size(1000, 676);
            Name = "Form1";
            Text = "Pdf Merger";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBoxPreview.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Button buttonSelect;
        private OpenFileDialog openFileDialog1;
        private ListView listViewPdf;
        private CheckBox checkBox1;
        private SaveFileDialog saveFileDialog1;
        private ProgressBar progressBarPages;
        private Label labelPages;
        private Button buttonMoveUp;
        private Button buttonMoveDown;
        private Button buttonCancel;
        private Button buttonDelete;
        private GroupBox groupBoxPreview;
        private ListView listViewPreview;
        private Button buttonClear;
        private SplitContainer splitContainer1;
        private PictureBox pictureBoxPreview;
        private GroupBox groupBox2;
        private TextBox textBoxFilter;
        private ComboBox comboBoxPageFilter;
        private CheckBox checkBoxInvertPageOrder;
        private Button buttonSave;
    }
}