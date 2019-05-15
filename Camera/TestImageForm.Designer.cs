namespace Camera
{
    partial class TestImageForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.NoiseValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ShiftValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftValue)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // NoiseValue
            // 
            this.NoiseValue.Location = new System.Drawing.Point(12, 543);
            this.NoiseValue.Name = "NoiseValue";
            this.NoiseValue.Size = new System.Drawing.Size(120, 20);
            this.NoiseValue.TabIndex = 1;
            this.NoiseValue.ValueChanged += new System.EventHandler(this.NoiseValue_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 527);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Шум, %";
            // 
            // ShiftValue
            // 
            this.ShiftValue.Location = new System.Drawing.Point(138, 543);
            this.ShiftValue.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.ShiftValue.Name = "ShiftValue";
            this.ShiftValue.Size = new System.Drawing.Size(120, 20);
            this.ShiftValue.TabIndex = 3;
            this.ShiftValue.ValueChanged += new System.EventHandler(this.ShiftValue_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 527);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Значение сдвига";
            // 
            // TestImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 571);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ShiftValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NoiseValue);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "TestImageForm";
            this.Text = "TestImageForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TestImageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown NoiseValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ShiftValue;
        private System.Windows.Forms.Label label2;
    }
}