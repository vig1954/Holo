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
            this.SaveData = new System.Windows.Forms.Button();
            this.CameraSettingsButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.UpdateBoth = new System.Windows.Forms.CheckBox();
            this.ToggleAccumulation = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.FreshnelDistanceDecimals = new System.Windows.Forms.NumericUpDown();
            this.freshnelObjectSize = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.freshnelDistance = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SerialPortNames = new System.Windows.Forms.ComboBox();
            this.CalibrationButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.psiValue4 = new System.Windows.Forms.NumericUpDown();
            this.psiValue3 = new System.Windows.Forms.NumericUpDown();
            this.psiValue2 = new System.Windows.Forms.NumericUpDown();
            this.psiValue1 = new System.Windows.Forms.NumericUpDown();
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ShowFunctions = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FreshnelDistanceDecimals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.freshnelObjectSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.freshnelDistance)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue1)).BeginInit();
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
            this.splitContainer1.Size = new System.Drawing.Size(1174, 601);
            this.splitContainer1.SplitterDistance = 768;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // PhaseDifferenceView
            // 
            this.PhaseDifferenceView.Active = false;
            this.PhaseDifferenceView.CloseEnabled = false;
            this.PhaseDifferenceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PhaseDifferenceView.Location = new System.Drawing.Point(0, 0);
            this.PhaseDifferenceView.Name = "PhaseDifferenceView";
            this.PhaseDifferenceView.Size = new System.Drawing.Size(768, 601);
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
            this.splitContainer2.Panel2.Controls.Add(this.SaveData);
            this.splitContainer2.Panel2.Controls.Add(this.CameraSettingsButton);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.CaptureSecondSeriesButton);
            this.splitContainer2.Panel2.Controls.Add(this.CaptureFirstSeriesButton);
            this.splitContainer2.Size = new System.Drawing.Size(402, 601);
            this.splitContainer2.SplitterDistance = 213;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // LiveView
            // 
            this.LiveView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LiveView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LiveView.Location = new System.Drawing.Point(0, 0);
            this.LiveView.Name = "LiveView";
            this.LiveView.Size = new System.Drawing.Size(402, 213);
            this.LiveView.TabIndex = 0;
            this.LiveView.TabStop = false;
            // 
            // SaveData
            // 
            this.SaveData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveData.Location = new System.Drawing.Point(255, 301);
            this.SaveData.Name = "SaveData";
            this.SaveData.Size = new System.Drawing.Size(135, 35);
            this.SaveData.TabIndex = 7;
            this.SaveData.Text = "Сохранить данные";
            this.SaveData.UseVisualStyleBackColor = true;
            this.SaveData.Click += new System.EventHandler(this.SaveData_Click);
            // 
            // CameraSettingsButton
            // 
            this.CameraSettingsButton.Location = new System.Drawing.Point(287, 3);
            this.CameraSettingsButton.Name = "CameraSettingsButton";
            this.CameraSettingsButton.Size = new System.Drawing.Size(136, 23);
            this.CameraSettingsButton.TabIndex = 6;
            this.CameraSettingsButton.Text = "Настройки камеры";
            this.CameraSettingsButton.UseVisualStyleBackColor = true;
            this.CameraSettingsButton.Click += new System.EventHandler(this.CameraSettingsButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ShowFunctions);
            this.groupBox3.Controls.Add(this.UpdateBoth);
            this.groupBox3.Controls.Add(this.ToggleAccumulation);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.FreshnelDistanceDecimals);
            this.groupBox3.Controls.Add(this.freshnelObjectSize);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.freshnelDistance);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 195);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 100);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Преобразование Френеля";
            // 
            // UpdateBoth
            // 
            this.UpdateBoth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdateBoth.AutoSize = true;
            this.UpdateBoth.Location = new System.Drawing.Point(243, 12);
            this.UpdateBoth.Name = "UpdateBoth";
            this.UpdateBoth.Size = new System.Drawing.Size(129, 17);
            this.UpdateBoth.TabIndex = 7;
            this.UpdateBoth.Text = "Обновить обе серии";
            this.UpdateBoth.UseVisualStyleBackColor = true;
            // 
            // ToggleAccumulation
            // 
            this.ToggleAccumulation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ToggleAccumulation.Location = new System.Drawing.Point(297, 55);
            this.ToggleAccumulation.Name = "ToggleAccumulation";
            this.ToggleAccumulation.Size = new System.Drawing.Size(75, 39);
            this.ToggleAccumulation.TabIndex = 6;
            this.ToggleAccumulation.Text = "Включить накопление";
            this.ToggleAccumulation.UseVisualStyleBackColor = true;
            this.ToggleAccumulation.Click += new System.EventHandler(this.ToggleAccumulation_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Десятые";
            // 
            // FreshnelDistanceDecimals
            // 
            this.FreshnelDistanceDecimals.Location = new System.Drawing.Point(79, 32);
            this.FreshnelDistanceDecimals.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.FreshnelDistanceDecimals.Name = "FreshnelDistanceDecimals";
            this.FreshnelDistanceDecimals.Size = new System.Drawing.Size(64, 20);
            this.FreshnelDistanceDecimals.TabIndex = 4;
            this.FreshnelDistanceDecimals.ValueChanged += new System.EventHandler(this.FreshnelDistanceDecimals_ValueChanged);
            // 
            // freshnelObjectSize
            // 
            this.freshnelObjectSize.DecimalPlaces = 2;
            this.freshnelObjectSize.Location = new System.Drawing.Point(9, 74);
            this.freshnelObjectSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.freshnelObjectSize.Name = "freshnelObjectSize";
            this.freshnelObjectSize.Size = new System.Drawing.Size(64, 20);
            this.freshnelObjectSize.TabIndex = 3;
            this.freshnelObjectSize.Value = new decimal(new int[] {
            635,
            0,
            0,
            131072});
            this.freshnelObjectSize.ValueChanged += new System.EventHandler(this.freshnelObjectSize_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Размер объекта";
            // 
            // freshnelDistance
            // 
            this.freshnelDistance.Location = new System.Drawing.Point(9, 32);
            this.freshnelDistance.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.freshnelDistance.Name = "freshnelDistance";
            this.freshnelDistance.Size = new System.Drawing.Size(64, 20);
            this.freshnelDistance.TabIndex = 1;
            this.freshnelDistance.Value = new decimal(new int[] {
            135,
            0,
            0,
            0});
            this.freshnelDistance.ValueChanged += new System.EventHandler(this.freshnelDistance_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Расстояние";
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
            this.groupBox1.Controls.Add(this.psiValue4);
            this.groupBox1.Controls.Add(this.psiValue3);
            this.groupBox1.Controls.Add(this.psiValue2);
            this.groupBox1.Controls.Add(this.psiValue1);
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
            // psiValue4
            // 
            this.psiValue4.DecimalPlaces = 2;
            this.psiValue4.Location = new System.Drawing.Point(292, 50);
            this.psiValue4.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.psiValue4.Name = "psiValue4";
            this.psiValue4.Size = new System.Drawing.Size(56, 20);
            this.psiValue4.TabIndex = 9;
            this.psiValue4.Value = new decimal(new int[] {
            270,
            0,
            0,
            0});
            this.psiValue4.ValueChanged += new System.EventHandler(this.psiValue4_ValueChanged);
            // 
            // psiValue3
            // 
            this.psiValue3.DecimalPlaces = 2;
            this.psiValue3.Location = new System.Drawing.Point(230, 50);
            this.psiValue3.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.psiValue3.Name = "psiValue3";
            this.psiValue3.Size = new System.Drawing.Size(56, 20);
            this.psiValue3.TabIndex = 8;
            this.psiValue3.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.psiValue3.ValueChanged += new System.EventHandler(this.psiValue3_ValueChanged);
            // 
            // psiValue2
            // 
            this.psiValue2.DecimalPlaces = 2;
            this.psiValue2.Location = new System.Drawing.Point(168, 50);
            this.psiValue2.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.psiValue2.Name = "psiValue2";
            this.psiValue2.Size = new System.Drawing.Size(56, 20);
            this.psiValue2.TabIndex = 7;
            this.psiValue2.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.psiValue2.ValueChanged += new System.EventHandler(this.psiValue2_ValueChanged);
            // 
            // psiValue1
            // 
            this.psiValue1.DecimalPlaces = 2;
            this.psiValue1.Location = new System.Drawing.Point(106, 50);
            this.psiValue1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.psiValue1.Name = "psiValue1";
            this.psiValue1.Size = new System.Drawing.Size(56, 20);
            this.psiValue1.TabIndex = 6;
            this.psiValue1.ValueChanged += new System.EventHandler(this.psiValue1_ValueChanged);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 577);
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
            // ShowFunctions
            // 
            this.ShowFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowFunctions.Location = new System.Drawing.Point(216, 55);
            this.ShowFunctions.Name = "ShowFunctions";
            this.ShowFunctions.Size = new System.Drawing.Size(75, 39);
            this.ShowFunctions.TabIndex = 8;
            this.ShowFunctions.Text = "Функции";
            this.ShowFunctions.UseVisualStyleBackColor = true;
            this.ShowFunctions.Click += new System.EventHandler(this.ShowFunctions_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 601);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Simple App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FreshnelDistanceDecimals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.freshnelObjectSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.freshnelDistance)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.psiValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psiValue1)).EndInit();
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
        private System.Windows.Forms.NumericUpDown psiValue4;
        private System.Windows.Forms.NumericUpDown psiValue3;
        private System.Windows.Forms.NumericUpDown psiValue2;
        private System.Windows.Forms.NumericUpDown psiValue1;
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown freshnelObjectSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown freshnelDistance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown FreshnelDistanceDecimals;
        private System.Windows.Forms.Button ToggleAccumulation;
        private System.Windows.Forms.Button CameraSettingsButton;
        private System.Windows.Forms.CheckBox UpdateBoth;
        private System.Windows.Forms.Button SaveData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button ShowFunctions;
    }
}

