namespace PsdCalibration
{
    partial class CalibrationForm
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
            this.CameraSettingsButton = new System.Windows.Forms.Button();
            this.LiveView = new System.Windows.Forms.PictureBox();
            this.CameraStatusLabel = new System.Windows.Forms.Label();
            this.ToggleAutoStepping = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.PsdValueChangeInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PsdValueStep = new System.Windows.Forms.NumericUpDown();
            this.EndPsdValue = new System.Windows.Forms.NumericUpDown();
            this.StartPsdValue = new System.Windows.Forms.NumericUpDown();
            this.CurrentPsdValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.PortDropdown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Byte1Text = new System.Windows.Forms.TextBox();
            this.Byte2Text = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.WriteRawBytesButton = new System.Windows.Forms.Button();
            this.PsdResponse = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsdValueChangeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsdValueStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndPsdValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartPsdValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentPsdValue)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CameraSettingsButton);
            this.splitContainer1.Panel1.Controls.Add(this.LiveView);
            this.splitContainer1.Panel1.Controls.Add(this.CameraStatusLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.PsdResponse);
            this.splitContainer1.Panel2.Controls.Add(this.WriteRawBytesButton);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.Byte2Text);
            this.splitContainer1.Panel2.Controls.Add(this.Byte1Text);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.ToggleAutoStepping);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.PsdValueChangeInterval);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.PsdValueStep);
            this.splitContainer1.Panel2.Controls.Add(this.EndPsdValue);
            this.splitContainer1.Panel2.Controls.Add(this.StartPsdValue);
            this.splitContainer1.Panel2.Controls.Add(this.CurrentPsdValue);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.PortDropdown);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(777, 408);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 0;
            // 
            // CameraSettingsButton
            // 
            this.CameraSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CameraSettingsButton.Location = new System.Drawing.Point(641, 4);
            this.CameraSettingsButton.Name = "CameraSettingsButton";
            this.CameraSettingsButton.Size = new System.Drawing.Size(133, 23);
            this.CameraSettingsButton.TabIndex = 2;
            this.CameraSettingsButton.Text = "Настройки камеры";
            this.CameraSettingsButton.UseVisualStyleBackColor = true;
            this.CameraSettingsButton.Visible = false;
            this.CameraSettingsButton.Click += new System.EventHandler(this.CameraSettingsButton_Click);
            // 
            // LiveView
            // 
            this.LiveView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LiveView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LiveView.Location = new System.Drawing.Point(6, 33);
            this.LiveView.Name = "LiveView";
            this.LiveView.Size = new System.Drawing.Size(768, 261);
            this.LiveView.TabIndex = 1;
            this.LiveView.TabStop = false;
            // 
            // CameraStatusLabel
            // 
            this.CameraStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CameraStatusLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CameraStatusLabel.Location = new System.Drawing.Point(6, 4);
            this.CameraStatusLabel.Name = "CameraStatusLabel";
            this.CameraStatusLabel.Size = new System.Drawing.Size(160, 23);
            this.CameraStatusLabel.TabIndex = 0;
            this.CameraStatusLabel.Text = "Камера - не подключена";
            this.CameraStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToggleAutoStepping
            // 
            this.ToggleAutoStepping.Location = new System.Drawing.Point(643, 58);
            this.ToggleAutoStepping.Name = "ToggleAutoStepping";
            this.ToggleAutoStepping.Size = new System.Drawing.Size(121, 37);
            this.ToggleAutoStepping.TabIndex = 12;
            this.ToggleAutoStepping.Text = "Начать автоперебор";
            this.ToggleAutoStepping.UseVisualStyleBackColor = true;
            this.ToggleAutoStepping.Click += new System.EventHandler(this.ToggleAutoStepping_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(497, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Время между сдвигами, с";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(600, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Сдвиг";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PsdValueChangeInterval
            // 
            this.PsdValueChangeInterval.DecimalPlaces = 2;
            this.PsdValueChangeInterval.Location = new System.Drawing.Point(643, 32);
            this.PsdValueChangeInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PsdValueChangeInterval.Name = "PsdValueChangeInterval";
            this.PsdValueChangeInterval.Size = new System.Drawing.Size(121, 20);
            this.PsdValueChangeInterval.TabIndex = 9;
            this.PsdValueChangeInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PsdValueChangeInterval.ValueChanged += new System.EventHandler(this.PsdValueChangeInterval_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(252, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Начальное значение";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Конечное значение";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PsdValueStep
            // 
            this.PsdValueStep.Location = new System.Drawing.Point(643, 6);
            this.PsdValueStep.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.PsdValueStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PsdValueStep.Name = "PsdValueStep";
            this.PsdValueStep.Size = new System.Drawing.Size(121, 20);
            this.PsdValueStep.TabIndex = 6;
            this.PsdValueStep.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.PsdValueStep.ValueChanged += new System.EventHandler(this.PsdValueStep_ValueChanged);
            // 
            // EndPsdValue
            // 
            this.EndPsdValue.Location = new System.Drawing.Point(370, 32);
            this.EndPsdValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.EndPsdValue.Name = "EndPsdValue";
            this.EndPsdValue.Size = new System.Drawing.Size(121, 20);
            this.EndPsdValue.TabIndex = 5;
            this.EndPsdValue.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.EndPsdValue.ValueChanged += new System.EventHandler(this.EndPsdValue_ValueChanged);
            // 
            // StartPsdValue
            // 
            this.StartPsdValue.Location = new System.Drawing.Point(370, 6);
            this.StartPsdValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.StartPsdValue.Name = "StartPsdValue";
            this.StartPsdValue.Size = new System.Drawing.Size(121, 20);
            this.StartPsdValue.TabIndex = 4;
            this.StartPsdValue.ValueChanged += new System.EventHandler(this.StartPsdValue_ValueChanged);
            // 
            // CurrentPsdValue
            // 
            this.CurrentPsdValue.Location = new System.Drawing.Point(105, 32);
            this.CurrentPsdValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.CurrentPsdValue.Name = "CurrentPsdValue";
            this.CurrentPsdValue.Size = new System.Drawing.Size(121, 20);
            this.CurrentPsdValue.TabIndex = 3;
            this.CurrentPsdValue.ValueChanged += new System.EventHandler(this.CurrentPsdValue_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Текущее значение";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PortDropdown
            // 
            this.PortDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortDropdown.FormattingEnabled = true;
            this.PortDropdown.Location = new System.Drawing.Point(105, 6);
            this.PortDropdown.Name = "PortDropdown";
            this.PortDropdown.Size = new System.Drawing.Size(121, 21);
            this.PortDropdown.TabIndex = 1;
            this.PortDropdown.SelectedIndexChanged += new System.EventHandler(this.PortDropdown_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Пьезокерамика";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Запись без обработки";
            // 
            // Byte1Text
            // 
            this.Byte1Text.Location = new System.Drawing.Point(172, 82);
            this.Byte1Text.Name = "Byte1Text";
            this.Byte1Text.Size = new System.Drawing.Size(67, 20);
            this.Byte1Text.TabIndex = 14;
            this.Byte1Text.Text = "20";
            this.Byte1Text.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Byte1Text_KeyUp);
            // 
            // Byte2Text
            // 
            this.Byte2Text.Location = new System.Drawing.Point(269, 82);
            this.Byte2Text.Name = "Byte2Text";
            this.Byte2Text.Size = new System.Drawing.Size(67, 20);
            this.Byte2Text.TabIndex = 15;
            this.Byte2Text.Text = "00";
            this.Byte2Text.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Byte2Text_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(169, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "byte 1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(266, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "byte 2";
            // 
            // WriteRawBytesButton
            // 
            this.WriteRawBytesButton.Location = new System.Drawing.Point(342, 80);
            this.WriteRawBytesButton.Name = "WriteRawBytesButton";
            this.WriteRawBytesButton.Size = new System.Drawing.Size(75, 23);
            this.WriteRawBytesButton.TabIndex = 18;
            this.WriteRawBytesButton.Text = "->";
            this.WriteRawBytesButton.UseVisualStyleBackColor = true;
            this.WriteRawBytesButton.Click += new System.EventHandler(this.WriteRawBytesButton_Click);
            // 
            // PsdResponse
            // 
            this.PsdResponse.AutoSize = true;
            this.PsdResponse.Location = new System.Drawing.Point(423, 85);
            this.PsdResponse.Name = "PsdResponse";
            this.PsdResponse.Size = new System.Drawing.Size(43, 13);
            this.PsdResponse.TabIndex = 19;
            this.PsdResponse.Text = "[Ответ]";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(151, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "0x";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(248, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(18, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "0x";
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 408);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CalibrationForm";
            this.Text = "Калибровка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalibrationForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsdValueChangeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsdValueStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndPsdValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartPsdValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentPsdValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button CameraSettingsButton;
        private System.Windows.Forms.PictureBox LiveView;
        private System.Windows.Forms.Label CameraStatusLabel;
        private System.Windows.Forms.ComboBox PortDropdown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown PsdValueStep;
        private System.Windows.Forms.NumericUpDown EndPsdValue;
        private System.Windows.Forms.NumericUpDown StartPsdValue;
        private System.Windows.Forms.NumericUpDown CurrentPsdValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown PsdValueChangeInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ToggleAutoStepping;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Byte2Text;
        private System.Windows.Forms.TextBox Byte1Text;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button WriteRawBytesButton;
        private System.Windows.Forms.Label PsdResponse;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
    }
}

