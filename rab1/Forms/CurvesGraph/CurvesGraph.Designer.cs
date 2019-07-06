namespace rab1
{
    partial class CurvesGraph
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
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelArrow = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApplyAll = new System.Windows.Forms.Button();
            this.txtStartImageNumber = new System.Windows.Forms.TextBox();
            this.txtEndImageNumber = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(430, 430);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // labelFrom
            // 
            this.labelFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelFrom.Location = new System.Drawing.Point(228, 458);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(74, 23);
            this.labelFrom.TabIndex = 9;
            // 
            // labelTo
            // 
            this.labelTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelTo.Location = new System.Drawing.Point(344, 458);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(86, 23);
            this.labelTo.TabIndex = 8;
            // 
            // labelArrow
            // 
            this.labelArrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelArrow.Location = new System.Drawing.Point(309, 457);
            this.labelArrow.Name = "labelArrow";
            this.labelArrow.Size = new System.Drawing.Size(30, 24);
            this.labelArrow.TabIndex = 7;
            this.labelArrow.Text = "->";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(228, 500);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(355, 500);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApplyAll
            // 
            this.btnApplyAll.Location = new System.Drawing.Point(12, 500);
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.Size = new System.Drawing.Size(134, 23);
            this.btnApplyAll.TabIndex = 3;
            this.btnApplyAll.Text = "Применить ко всем";
            this.btnApplyAll.UseVisualStyleBackColor = true;
            this.btnApplyAll.Click += new System.EventHandler(this.btnApplyAll_Click);
            // 
            // txtStartImageNumber
            // 
            this.txtStartImageNumber.Location = new System.Drawing.Point(12, 461);
            this.txtStartImageNumber.Name = "txtStartImageNumber";
            this.txtStartImageNumber.Size = new System.Drawing.Size(62, 20);
            this.txtStartImageNumber.TabIndex = 2;
            // 
            // txtEndImageNumber
            // 
            this.txtEndImageNumber.Location = new System.Drawing.Point(80, 461);
            this.txtEndImageNumber.Name = "txtEndImageNumber";
            this.txtEndImageNumber.Size = new System.Drawing.Size(66, 20);
            this.txtEndImageNumber.TabIndex = 1;
            // 
            // CurvesGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(437, 535);
            this.Controls.Add(this.txtEndImageNumber);
            this.Controls.Add(this.txtStartImageNumber);
            this.Controls.Add(this.btnApplyAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.labelArrow);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurvesGraph";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurvesGraph_FormClosing);
            this.Load += new System.EventHandler(this.CurvesGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelArrow;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApplyAll;
        private System.Windows.Forms.TextBox txtStartImageNumber;
        private System.Windows.Forms.TextBox txtEndImageNumber;
    }
}