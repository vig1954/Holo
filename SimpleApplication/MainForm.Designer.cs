namespace SimpleApplication
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.PhaseDifferenceView = new UserInterface.DataEditors.DataEditorView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.LiveView = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SerialPortNames = new System.Windows.Forms.ComboBox();
            this.CalibrationButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.psiValues1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.psdValue4 = new System.Windows.Forms.NumericUpDown();
            this.psdValue3 = new System.Windows.Forms.NumericUpDown();
            this.psdValue2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.psdValue1 = new System.Windows.Forms.NumericUpDown();
            this.CaptureSecondSeriesButton = new System.Windows.Forms.Button();
            this.CaptureFirstSeriesButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.cameraStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.psdStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValues1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PhaseDifferenceView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1174, 529);
            this.splitContainer1.SplitterDistance = 768;
            this.splitContainer1.TabIndex = 0;
            // 
            // PhaseDifferenceView
            // 
            this.PhaseDifferenceView.Active = false;
            this.PhaseDifferenceView.CloseEnabled = false;
            this.PhaseDifferenceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PhaseDifferenceView.Location = new System.Drawing.Point(0, 0);
            this.PhaseDifferenceView.Name = "PhaseDifferenceView";
            this.PhaseDifferenceView.Size = new System.Drawing.Size(768, 529);
            this.PhaseDifferenceView.SplitEnabled = false;
            this.PhaseDifferenceView.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.LiveView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.CaptureSecondSeriesButton);
            this.splitContainer2.Panel2.Controls.Add(this.CaptureFirstSeriesButton);
            this.splitContainer2.Size = new System.Drawing.Size(402, 529);
            this.splitContainer2.SplitterDistance = 275;
            this.splitContainer2.TabIndex = 0;
            // 
            // LiveView
            // 
            this.LiveView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LiveView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LiveView.Location = new System.Drawing.Point(0, 0);
            this.LiveView.Name = "LiveView";
            this.LiveView.Size = new System.Drawing.Size(402, 275);
            this.LiveView.TabIndex = 0;
            this.LiveView.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.SerialPortNames);
            this.groupBox2.Controls.Add(this.CalibrationButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(378, 51);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Пьезокерамика";
            // 
            // SerialPortNames
            // 
            this.SerialPortNames.FormattingEnabled = true;
            this.SerialPortNames.Location = new System.Drawing.Point(9, 19);
            this.SerialPortNames.Name = "SerialPortNames";
            this.SerialPortNames.Size = new System.Drawing.Size(104, 21);
            this.SerialPortNames.TabIndex = 0;
            this.SerialPortNames.SelectedIndexChanged += new System.EventHandler(this.SerialPortNames_SelectedIndexChanged);
            // 
            // CalibrationButton
            // 
            this.CalibrationButton.Location = new System.Drawing.Point(119, 19);
            this.CalibrationButton.Name = "CalibrationButton";
            this.CalibrationButton.Size = new System.Drawing.Size(94, 21);
            this.CalibrationButton.TabIndex = 3;
            this.CalibrationButton.Text = "Калибровка";
            this.CalibrationButton.UseVisualStyleBackColor = true;
            this.CalibrationButton.Click += new System.EventHandler(this.CalibrationButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.psiValues1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.psdValue4);
            this.groupBox1.Controls.Add(this.psdValue3);
            this.groupBox1.Controls.Add(this.psdValue2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.psdValue1);
            this.groupBox1.Location = new System.Drawing.Point(12, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фазовый сдвиг";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Location = new System.Drawing.Point(292, 50);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown3.TabIndex = 9;
            this.numericUpDown3.Value = new decimal(new int[] {
            270,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Location = new System.Drawing.Point(230, 50);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown2.TabIndex = 8;
            this.numericUpDown2.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(168, 50);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // psiValues1
            // 
            this.psiValues1.DecimalPlaces = 2;
            this.psiValues1.Location = new System.Drawing.Point(106, 50);
            this.psiValues1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.psiValues1.Name = "psiValues1";
            this.psiValues1.Size = new System.Drawing.Size(56, 20);
            this.psiValues1.TabIndex = 6;
            this.psiValues1.ValueChanged += new System.EventHandler(this.psiValues1_ValueChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Значения\rPSI";
            // 
            // psdValue4
            // 
            this.psdValue4.Location = new System.Drawing.Point(292, 21);
            this.psdValue4.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.psdValue4.Name = "psdValue4";
            this.psdValue4.Size = new System.Drawing.Size(56, 20);
            this.psdValue4.TabIndex = 4;
            this.psdValue4.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.psdValue4.ValueChanged += new System.EventHandler(this.psdValue4_ValueChanged);
            // 
            // psdValue3
            // 
            this.psdValue3.Location = new System.Drawing.Point(230, 21);
            this.psdValue3.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.psdValue3.Name = "psdValue3";
            this.psdValue3.Size = new System.Drawing.Size(56, 20);
            this.psdValue3.TabIndex = 3;
            this.psdValue3.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.psdValue3.ValueChanged += new System.EventHandler(this.psdValue3_ValueChanged);
            // 
            // psdValue2
            // 
            this.psdValue2.Location = new System.Drawing.Point(168, 21);
            this.psdValue2.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.psdValue2.Name = "psdValue2";
            this.psdValue2.Size = new System.Drawing.Size(56, 20);
            this.psdValue2.TabIndex = 2;
            this.psdValue2.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.psdValue2.ValueChanged += new System.EventHandler(this.psdValue2_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Значения\r\nпьезокерамики";
            // 
            // psdValue1
            // 
            this.psdValue1.Location = new System.Drawing.Point(106, 21);
            this.psdValue1.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.psdValue1.Name = "psdValue1";
            this.psdValue1.Size = new System.Drawing.Size(56, 20);
            this.psdValue1.TabIndex = 0;
            this.psdValue1.ValueChanged += new System.EventHandler(this.psdValue1_ValueChanged);
            // 
            // CaptureSecondSeriesButton
            // 
            this.CaptureSecondSeriesButton.Location = new System.Drawing.Point(145, 3);
            this.CaptureSecondSeriesButton.Name = "CaptureSecondSeriesButton";
            this.CaptureSecondSeriesButton.Size = new System.Drawing.Size(136, 23);
            this.CaptureSecondSeriesButton.TabIndex = 1;
            this.CaptureSecondSeriesButton.Text = "Захват второй серии";
            this.CaptureSecondSeriesButton.UseVisualStyleBackColor = true;
            this.CaptureSecondSeriesButton.Click += new System.EventHandler(this.CaptureSecondSeriesButton_Click);
            // 
            // CaptureFirstSeriesButton
            // 
            this.CaptureFirstSeriesButton.Location = new System.Drawing.Point(3, 3);
            this.CaptureFirstSeriesButton.Name = "CaptureFirstSeriesButton";
            this.CaptureFirstSeriesButton.Size = new System.Drawing.Size(136, 23);
            this.CaptureFirstSeriesButton.TabIndex = 0;
            this.CaptureFirstSeriesButton.Text = "Захват первой серии";
            this.CaptureFirstSeriesButton.UseVisualStyleBackColor = true;
            this.CaptureFirstSeriesButton.Click += new System.EventHandler(this.CaptureFirstSeriesButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cameraStatusLabel,
            this.psdStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 505);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1174, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // cameraStatusLabel
            // 
            this.cameraStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.cameraStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.cameraStatusLabel.Name = "cameraStatusLabel";
            this.cameraStatusLabel.Size = new System.Drawing.Size(148, 19);
            this.cameraStatusLabel.Text = "Камера - не подключена";
            // 
            // psdStatusLabel
            // 
            this.psdStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.psdStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.psdStatusLabel.Name = "psdStatusLabel";
            this.psdStatusLabel.Size = new System.Drawing.Size(193, 19);
            this.psdStatusLabel.Text = "Пьезокерамика - не подключена";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 529);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Simple App";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValues1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psdValue1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserInterface.DataEditors.DataEditorView PhaseDifferenceView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox LiveView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown psiValues1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown psdValue4;
        private System.Windows.Forms.NumericUpDown psdValue3;
        private System.Windows.Forms.NumericUpDown psdValue2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown psdValue1;
        private System.Windows.Forms.Button CaptureSecondSeriesButton;
        private System.Windows.Forms.Button CaptureFirstSeriesButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel cameraStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel psdStatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox SerialPortNames;
        private System.Windows.Forms.Button CalibrationButton;
    }
}

