namespace SimpleApplication
{
    partial class SegmentSelectionForm
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
            this.LiveView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).BeginInit();
            this.SuspendLayout();
            // 
            // LiveView
            // 
            this.LiveView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LiveView.Location = new System.Drawing.Point(0, 0);
            this.LiveView.Name = "LiveView";
            this.LiveView.Size = new System.Drawing.Size(800, 450);
            this.LiveView.TabIndex = 0;
            this.LiveView.TabStop = false;
            this.LiveView.Paint += new System.Windows.Forms.PaintEventHandler(this.LiveView_Paint);
            // 
            // SegmentSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LiveView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SegmentSelectionForm";
            this.Text = "Выбор среза";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SegmentSelectionForm_FormClosing);
            this.Load += new System.EventHandler(this.SegmentSelectionForm_Load);
            this.Shown += new System.EventHandler(this.SegmentSelectionForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.LiveView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox LiveView;
    }
}