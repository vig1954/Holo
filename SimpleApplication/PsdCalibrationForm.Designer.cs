namespace SimpleApplication
{
    partial class PsdCalibrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Graph = new System.Windows.Forms.PictureBox();
            this.DataView = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RecordList = new System.Windows.Forms.ComboBox();
            this.Shift = new System.Windows.Forms.NumericUpDown();
            this.Minimum = new System.Windows.Forms.NumericUpDown();
            this.Maximum = new System.Windows.Forms.NumericUpDown();
            this.Step = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Time = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.Calibrate = new System.Windows.Forms.Button();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Graph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Shift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Maximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Time)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // Graph
            // 
            this.Graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Graph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Graph.Location = new System.Drawing.Point(12, 12);
            this.Graph.Name = "Graph";
            this.Graph.Size = new System.Drawing.Size(776, 424);
            this.Graph.TabIndex = 0;
            this.Graph.TabStop = false;
            this.Graph.Paint += new System.Windows.Forms.PaintEventHandler(this.Graph_Paint);
            // 
            // DataView
            // 
            this.DataView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DataView.Location = new System.Drawing.Point(12, 464);
            this.DataView.Name = "DataView";
            this.DataView.Size = new System.Drawing.Size(128, 128);
            this.DataView.TabIndex = 1;
            this.DataView.TabStop = false;
            this.DataView.Paint += new System.Windows.Forms.PaintEventHandler(this.DataView_Paint);
            this.DataView.DoubleClick += new System.EventHandler(this.DataView_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выбор записи";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // RecordList
            // 
            this.RecordList.FormattingEnabled = true;
            this.RecordList.Location = new System.Drawing.Point(90, 6);
            this.RecordList.Name = "RecordList";
            this.RecordList.Size = new System.Drawing.Size(121, 21);
            this.RecordList.TabIndex = 3;
            this.RecordList.SelectedIndexChanged += new System.EventHandler(this.RecordList_SelectedIndexChanged);
            // 
            // Shift
            // 
            this.Shift.Location = new System.Drawing.Point(267, 6);
            this.Shift.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.Shift.Name = "Shift";
            this.Shift.Size = new System.Drawing.Size(121, 20);
            this.Shift.TabIndex = 4;
            // 
            // Minimum
            // 
            this.Minimum.Location = new System.Drawing.Point(93, 6);
            this.Minimum.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.Minimum.Name = "Minimum";
            this.Minimum.Size = new System.Drawing.Size(120, 20);
            this.Minimum.TabIndex = 5;
            // 
            // Maximum
            // 
            this.Maximum.Location = new System.Drawing.Point(93, 32);
            this.Maximum.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.Maximum.Name = "Maximum";
            this.Maximum.Size = new System.Drawing.Size(120, 20);
            this.Maximum.TabIndex = 6;
            this.Maximum.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            // 
            // Step
            // 
            this.Step.Location = new System.Drawing.Point(268, 30);
            this.Step.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Step.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Step.Name = "Step";
            this.Step.Size = new System.Drawing.Size(120, 20);
            this.Step.TabIndex = 7;
            this.Step.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Сдвиг";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Минимум";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Максимум";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(235, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Шаг";
            // 
            // Time
            // 
            this.Time.Location = new System.Drawing.Point(268, 56);
            this.Time.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(120, 20);
            this.Time.TabIndex = 12;
            this.Time.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(167, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "T установления, с";
            // 
            // Calibrate
            // 
            this.Calibrate.Location = new System.Drawing.Point(93, 87);
            this.Calibrate.Name = "Calibrate";
            this.Calibrate.Size = new System.Drawing.Size(120, 23);
            this.Calibrate.TabIndex = 14;
            this.Calibrate.Text = "Начать";
            this.Calibrate.UseVisualStyleBackColor = true;
            this.Calibrate.Click += new System.EventHandler(this.Calibrate_Click);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(219, 92);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(16, 13);
            this.ProgressLabel.TabIndex = 15;
            this.ProgressLabel.Text = "...";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(146, 442);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 154);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Minimum);
            this.tabPage1.Controls.Add(this.ProgressLabel);
            this.tabPage1.Controls.Add(this.Shift);
            this.tabPage1.Controls.Add(this.Calibrate);
            this.tabPage1.Controls.Add(this.Maximum);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.Step);
            this.tabPage1.Controls.Add(this.Time);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 128);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Калибровка";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.trackBar1);
            this.tabPage2.Controls.Add(this.RecordList);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(411, 128);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Просмотр";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(8, 33);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(397, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // PsdCalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 604);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.DataView);
            this.Controls.Add(this.Graph);
            this.Name = "PsdCalibrationForm";
            this.Text = "Калибровка пьезокерамики";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PsdCalibrationForm_FormClosing);
            this.Shown += new System.EventHandler(this.PsdCalibrationForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Graph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Shift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Maximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Time)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Graph;
        private System.Windows.Forms.PictureBox DataView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox RecordList;
        private System.Windows.Forms.NumericUpDown Shift;
        private System.Windows.Forms.NumericUpDown Minimum;
        private System.Windows.Forms.NumericUpDown Maximum;
        private System.Windows.Forms.NumericUpDown Step;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown Time;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Calibrate;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}