namespace Camera
{
    partial class PhaseShiftDeviceCalibrationForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SetAsZero = new System.Windows.Forms.Button();
            this.SaveSample = new System.Windows.Forms.Button();
            this.CapturedSamplesList = new System.Windows.Forms.ListBox();
            this.SetPhaseShiftButton = new System.Windows.Forms.Button();
            this.ShiftValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Plot = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Plot)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.SetAsZero);
            this.splitContainer1.Panel1.Controls.Add(this.SaveSample);
            this.splitContainer1.Panel1.Controls.Add(this.CapturedSamplesList);
            this.splitContainer1.Panel1.Controls.Add(this.SetPhaseShiftButton);
            this.splitContainer1.Panel1.Controls.Add(this.ShiftValue);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Plot);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 146;
            this.splitContainer1.TabIndex = 0;
            // 
            // SetAsZero
            // 
            this.SetAsZero.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetAsZero.Location = new System.Drawing.Point(12, 109);
            this.SetAsZero.Name = "SetAsZero";
            this.SetAsZero.Size = new System.Drawing.Size(120, 23);
            this.SetAsZero.TabIndex = 5;
            this.SetAsZero.Text = "Задать нуль";
            this.SetAsZero.UseVisualStyleBackColor = true;
            this.SetAsZero.Click += new System.EventHandler(this.SetAsZero_Click);
            // 
            // SaveSample
            // 
            this.SaveSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSample.Location = new System.Drawing.Point(12, 80);
            this.SaveSample.Name = "SaveSample";
            this.SaveSample.Size = new System.Drawing.Size(120, 23);
            this.SaveSample.TabIndex = 4;
            this.SaveSample.Text = "Сохранить в список";
            this.SaveSample.UseVisualStyleBackColor = true;
            this.SaveSample.Click += new System.EventHandler(this.SaveSample_Click);
            // 
            // CapturedSamplesList
            // 
            this.CapturedSamplesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CapturedSamplesList.FormattingEnabled = true;
            this.CapturedSamplesList.Location = new System.Drawing.Point(12, 145);
            this.CapturedSamplesList.Name = "CapturedSamplesList";
            this.CapturedSamplesList.Size = new System.Drawing.Size(120, 290);
            this.CapturedSamplesList.TabIndex = 3;
            this.CapturedSamplesList.SelectedIndexChanged += new System.EventHandler(this.CapturedSamplesList_SelectedIndexChanged);
            // 
            // SetPhaseShiftButton
            // 
            this.SetPhaseShiftButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetPhaseShiftButton.Location = new System.Drawing.Point(12, 51);
            this.SetPhaseShiftButton.Name = "SetPhaseShiftButton";
            this.SetPhaseShiftButton.Size = new System.Drawing.Size(120, 23);
            this.SetPhaseShiftButton.TabIndex = 2;
            this.SetPhaseShiftButton.Text = "Задать сдвиг";
            this.SetPhaseShiftButton.UseVisualStyleBackColor = true;
            this.SetPhaseShiftButton.Click += new System.EventHandler(this.SetPhaseShiftButton_Click);
            // 
            // ShiftValue
            // 
            this.ShiftValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShiftValue.Location = new System.Drawing.Point(12, 25);
            this.ShiftValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ShiftValue.Name = "ShiftValue";
            this.ShiftValue.Size = new System.Drawing.Size(120, 20);
            this.ShiftValue.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Значение сдвига";
            // 
            // Plot
            // 
            this.Plot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Plot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Plot.Location = new System.Drawing.Point(3, 3);
            this.Plot.Name = "Plot";
            this.Plot.Size = new System.Drawing.Size(644, 444);
            this.Plot.TabIndex = 0;
            this.Plot.TabStop = false;
            this.Plot.Paint += new System.Windows.Forms.PaintEventHandler(this.Plot_Paint);
            // 
            // PhaseShiftDeviceCalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PhaseShiftDeviceCalibrationForm";
            this.Text = "Калибровка УФЗ";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ShiftValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Plot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox CapturedSamplesList;
        private System.Windows.Forms.Button SetPhaseShiftButton;
        private System.Windows.Forms.NumericUpDown ShiftValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox Plot;
        private System.Windows.Forms.Button SetAsZero;
        private System.Windows.Forms.Button SaveSample;
    }
}